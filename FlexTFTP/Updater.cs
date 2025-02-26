using FlexTFTP.Properties;
using MS.WindowsAPICodePack.Internal;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using Nito.AsyncEx;

namespace FlexTFTP
{
    public class Updater
    {
        bool _updateAvailable;
        UpdateVersion _newestVersion;
        readonly string _downloadPath;
        string _downloadedFilePath;
        bool _asyncInProcess;

        bool _isBeta;

        public Updater(string downloadPath)
        {
            _downloadPath = downloadPath;
        }

        public bool Beta => _isBeta;

        public string NewestDate => (_newestVersion != null) ? _newestVersion.DateString : "-";

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

        public bool CheckForUpdate(bool acceptBeta)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // User agent must be set for GitHub API otherwise 403 Error is returned
                    // See: https://stackoverflow.com/questions/76490209/c-sharp-403-whith-github-api-request
                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                  "Windows NT 5.2; .NET CLR 1.0.3705;)");

                    var apiResponse = AsyncContext.Run(() => client.GetStringAsync(Settings.Default.UpdateURL));

                    List<JObject>? jsonArray = JsonConvert.DeserializeObject<List<JObject>>(apiResponse);

                    if (jsonArray != null)
                    {
                        foreach (var jsonObject in jsonArray)
                        {
                            UpdateVersion releaseVersion = new UpdateVersion();

                            releaseVersion.Name = jsonObject["name"].ToString();
                            releaseVersion.IsBeta = releaseVersion.Name.Contains("beta");
                            releaseVersion.UpdateLink = jsonObject["html_url"].ToString();
                            releaseVersion.DateString = jsonObject["published_at"].ToString();

                            // Find version
                            {
                                string pattern = @".*v(\d+\.\d+).*";
                                Regex regex = new Regex(pattern);
                                Match match = regex.Match(releaseVersion.Name);
                                if (match.Success)
                                {
                                    releaseVersion.Version = double.Parse(match.Groups[1].Captures[0].Value, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    continue; // Skip release without parsable version
                                }
                            }

                            // Find update link
                            {
                                string pattern = @".*\.zip]\((.*)\)";
                                Regex regex = new Regex(pattern);
                                Match match = regex.Match(jsonObject["body"].ToString());
                                if (match.Success)
                                {
                                    releaseVersion.DownloadLink = match.Groups[1].Captures[0].Value;
                                }

                                if (!match.Success || !releaseVersion.DownloadLink.EndsWith(".zip"))
                                {
                                    continue; // Skip release without download link
                                }
                            }

                            if (releaseVersion.IsBeta && !acceptBeta)
                            {
                                continue;
                            }

                            if (_newestVersion == null || releaseVersion.Version > _newestVersion.Version)
                            {
                                _newestVersion = releaseVersion;
                            }
                        }
                    }
                }
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

            if (_newestVersion.Version > Utils.CurrentVersion)
            {
                _isBeta = _newestVersion.IsBeta;
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

            // Download file
            //--------------
            try
            {
                using (var client = new WebClient())
                {
                    string localFile = Path.Combine(_downloadPath, Path.GetFileName(_newestVersion.DownloadLink));
                    if(File.Exists(localFile))
                    {
                        File.Delete(localFile);
                    }
                    client.DownloadFile(_newestVersion.DownloadLink, localFile);

                    string localFolder = localFile.Replace(".zip", "");
                    if (Directory.Exists(localFolder))
                    {
                        Directory.Delete(localFolder, true);
                    }
                    Directory.CreateDirectory(localFolder);
                    ZipFile.ExtractToDirectory(localFile, localFolder);

                    // Search for .exe in unpacked folder
                    _downloadedFilePath = Directory.EnumerateFiles(localFolder, "*.exe", SearchOption.AllDirectories).FirstOrDefault();
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

            if (string.IsNullOrEmpty(_downloadedFilePath))
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
            updateDoStream.WriteLine("copy \"" + _downloadedFilePath + "\" \"" + currentRunningFilePath + "\" >nul 2>&1");

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
