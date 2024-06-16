using FlexTFTP.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FlexTFTP
{
    public class Updater
    {
        bool _updateAvailable;
        UpdateVersion _newestVersion;
        readonly string _downloadPath;
        string _downloadedFileName;
        string _currentDateString;
        double _newestDate;
        bool _asyncInProcess;

        bool _isBeta;

        public Updater(string downloadPath)
        {
            _downloadPath = downloadPath;
        }

        public bool Beta => _isBeta;

        public string NewestDate => (_newestVersion != null) ? _newestVersion.DateString : "-";

        public string CurrentDate => _currentDateString;

        public string NewestVersionName => (_newestVersion != null) ? _newestVersion.Name : "-";

        public double NewestVersion => _newestVersion?.Version ?? 0;

        public string UpdateLink => (_newestVersion != null) ? _newestVersion.UpdateLink : "-";

        public delegate void UpdateCheckHandler(Updater updater, Form form);

        public void CheckForUpdateAsync(UpdateCheckHandler handler, Form form, bool autoUpdate, bool beta)
        {
            if (_asyncInProcess) return;
            _asyncInProcess = true;

            new Task(() => 
            {
                if (CheckForUpdate(beta))
                {
                    if(autoUpdate)
                    {
                        DownloadUpdate();
                    }

                    object[] parametersArray = { this, form };
                    try
                    {
                        form.Invoke(handler, parametersArray);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    _asyncInProcess = false;
                }
            }).Start();
        }

        public bool CheckForUpdate(bool beta)
        {
            DateTime currentDateTime = File.GetLastWriteTime(GetType().Assembly.Location);
            _currentDateString = currentDateTime.ToString("dd.MM.yyyy HH:mm");
            double currentDate = Utils.DateTimeToUnixTimestamp(currentDateTime);

            try
            {
                var document = XDocument.Load(Settings.Default.UpdateURL);
                XElement versions = document.Element("versions");

                do
                {
                    UpdateVersion ringVersion = new UpdateVersion();
                    XElement updateRing = null;
                    if (versions != null) updateRing = versions.Element(beta ? "beta" : "stable");

                    XElement newest = updateRing?.Element("newest");
                    if (newest != null)
                    {
                        ringVersion.Version = (double) newest.Element("version");
                        ringVersion.Name = (string) newest.Element("name");
                        ringVersion.UpdateLink = (string) newest.Element("link");
                        ringVersion.DownloadLink = (string) newest.Element("direct-link");
                        ringVersion.DateString = (string) newest.Element("date");
                    }

                    if (_newestVersion == null || ringVersion.Version > _newestVersion.Version)
                    {
                        _newestVersion = ringVersion;
                    }
                    if (ringVersion.Version >= Utils.CurrentVersion || !beta) break;
                    beta = false;
                } while (true);
            }
#if !DEBUG
            catch (Exception)
            {
#else
            catch (Exception e)
            {
                MessageBox.Show("Sorry something went wrong!" +
                    "\r\nTry to update to newest version manually." +
                    "\r\nException occurred in update process:" +
                    "\r\n" + e.Message +
                    "\r\nUpdate information corrupted.", "Error occurred", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
                _updateAvailable = false;
                return false;
            }

            if(_newestVersion == null)
            {
                _updateAvailable = false;
                return false;
            }
            _newestDate = Utils.DateTimeToUnixTimestamp(DateTime.Parse(_newestVersion.DateString));
            if (_newestVersion.Version > Utils.CurrentVersion || (beta && _newestDate > currentDate))
            {
                _isBeta = beta;
                _updateAvailable = true;
                return true;
            }

            _updateAvailable = false;
            return false;
        }

        public bool DownloadUpdate()
        {
            if(!_updateAvailable || _newestVersion.DownloadLink == null || _newestVersion.DownloadLink.Length == 0)
            {
                return false;
            }

            _downloadedFileName = (_newestVersion.Name + ".exe").Replace(' ', '_');

            // Download file
            //--------------
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(_newestVersion.DownloadLink, _downloadPath + "/" + _downloadedFileName);
                }
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public bool ProcessDownload()
        {
            StreamWriter updateDoStream = new StreamWriter(_downloadPath + "\\" + "update_do.cmd");
            StreamWriter updateStartStream = new StreamWriter(_downloadPath + "\\" + "update_start.cmd");

            if (string.IsNullOrEmpty(_downloadedFileName))
            {
                return false;
            }

            var currentRunningFilePath = GetType().Assembly.Location;
            var currentRunningFileName = Path.GetFileName(currentRunningFilePath);

            //--------------------------------------------
            // Create batch to start update process script
            //--------------------------------------------

            // Reduce output
            //--------------
            updateStartStream.WriteLine("@echo off");

            // Start updated application
            //--------------------------
            updateStartStream.WriteLine("start /b \"\" \"" + _downloadPath + "/update_do.cmd\"");

            // Exit update script
            //-------------------
            updateStartStream.WriteLine("exit");

            updateStartStream.Close();

            //------------------------------------------------------------
            // Create batch to override currently running application file
            //------------------------------------------------------------

            // Reduce output
            //--------------
            updateDoStream.WriteLine("@echo off");

            // Some text
            //----------
            updateDoStream.WriteLine("echo ########################################");
            updateDoStream.WriteLine("echo ## Update process for " + _newestVersion.Name);
            updateDoStream.WriteLine("echo ########################################");

            // Kill current process
            //---------------------
            updateDoStream.WriteLine("echo Kill FlexTFTP process...");
            updateDoStream.WriteLine("taskkill /F /IM " + currentRunningFileName + " /T >nul 2>&1");

            // Wait
            //-----
            updateDoStream.WriteLine("ping 127.0.0.1 -n 2 >nul 2>&1");

            // Delete old file
            //----------------
            updateDoStream.WriteLine("echo Delete old version...");
            updateDoStream.WriteLine("del /f \"" + currentRunningFilePath + "\" >nul 2>&1");

            // Copy new files
            //---------------
            updateDoStream.WriteLine("echo Copy new version...");
            updateDoStream.WriteLine("copy \"" + (_downloadPath + "\\" + _downloadedFileName) + "\" \"" + currentRunningFilePath + "\" >nul 2>&1");

            // Start updated application
            //--------------------------
            updateDoStream.WriteLine("echo Start new version...");
            updateDoStream.WriteLine("start /b \"\" \"" + currentRunningFilePath + "\" >nul 2>&1");

            // Exit update script
            //-------------------
            updateDoStream.WriteLine("exit");

            updateDoStream.Close();

            // Start update script
            //--------------------
            //System.Diagnostics.Process.Start(downloadPath + "/" + "update.cmd");

            try
            {
                ProcessStartInfo procInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "update_start.cmd",
                    WorkingDirectory = _downloadPath,
                    Verb = "runas"
                };
                //The file in that DIR.
                //The working DIR.
                Process.Start(procInfo);  //Start that process.
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
