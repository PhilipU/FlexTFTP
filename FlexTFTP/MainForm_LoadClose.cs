using FlexTFTP.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace FlexTFTP
{
    public partial class FlexTftpForm : Form
    {
        private void Form1_Load(object sender, EventArgs e)
        {
            OutputBox = new OutputBox(this, outputTextBox);
            Transfer = new Transfer(this);
            _pathAutoCompleteList = new AutoCompleteList(textBoxPath);
            _ipAutoCompleteList = new AutoCompleteList(textBoxAddress);
            _lockedSettingsHistory = new LockedSettingsHistory();
            _fileWatcher = new FileWatcher(this);
            _onlineChecker = new OnlineChecker(null, OnlineCheckerCallback);

            Text += " v" + Utils.CurrentVersion.ToString(CultureInfo.InvariantCulture).Replace(',', '.');
#if DEBUG
            Text += " [DEBUG]";
#endif

            OutputBox.AddLine(Resources.Changelog, FontFamily.GenericMonospace, 7F, false);
            OutputBox.LoadClearWaepon();

            // Load all known paths for autocompletion
            //----------------------------------------
            _pathAutoCompleteList.AddEntries(TargetPathParser.GetAllPaths());

            //--------------
            // Load settings
            //--------------

            // Statistics
            //-----------
            Transfer.TransferTotalKiloByte = Settings.Default.totalBytesTransfered / 1024;
            Transfer.TransferTotalTimeSec = Settings.Default.totalTimeTransfered;

            // Host address
            //-------------
            if (Settings.Default.RestoreIPAddress)
            {
                textBoxAddress.Text = Settings.Default.HostAddress;
            }

            // Port
            //-----
            if (Settings.Default.RestorePort)
            {
                maskedTextBoxPort.Text = Settings.Default.Port.ToString();
            }

            // Auto path
            //----------
            autoPathCheckBox.Checked = Settings.Default.AutoPath;

            // Path
            //-----
            if (Settings.Default.RestorePath)
            {
                textBoxPath.Text = Settings.Default.Path;
            }

            // Window Position
            //----------------
            if (Settings.Default.RestoreWindowPosition &&
                Settings.Default.WindowPositionX != -1 &&
                Settings.Default.WindowPositionY != -1 &&
                Settings.Default.WindowPositionY > -1000 &&
                Settings.Default.WindowPositionX > -1000)
            {
                Location = new Point(Settings.Default.WindowPositionX, Settings.Default.WindowPositionY);
            }

            // Updater
            //---------------------
            _updater = new Updater(_settingsDataRootPath);

            // Load Settings
            //--------------
            UpdateSettings();
        }

        private void FlexTFTPForm_Shown(object sender, EventArgs e)
        {
            // Restore history
            //----------------
            try
            {
                if (Settings.Default.RestoreHistory)
                {
                    OutputBox.LoadFile(_historyFolderPath);
                    _pathAutoCompleteList.LoadFile(_pathAutoCompleteHistoryFilePath);
                    _ipAutoCompleteList.LoadFile(_ipAutoCompleteHistoryFilePath);
                    _lockedSettingsHistory.LoadFile(_lockedSettingsHistoryFilePath);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            OutputBox.AddLine("");
            OutputBox.AddLine("Application started.", Color.Gray, true);

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if(File.Exists(args[1]))
                {
                    SetFilePath(args[1]);
                }
            }
            else
            {
                string lastOpenedFile = Settings.Default.LastOpenedFile;

                // Load last opened file
                //----------------------
                if (Settings.Default.RestoreLastOpenedFile &&
                    lastOpenedFile.Length != 0 &&
                    File.Exists(lastOpenedFile))
                {
                    SetFilePath(lastOpenedFile);
                }
            }
        }

        private void FlexTftpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop file watcher
            //------------------
            _fileWatcher.ClearFile();
        }

        private void FlexTFTPForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Stop all running tasks
            //-----------------------
            _onlineChecker.StopCyclicCheck();

            // Statistics
            //-----------
            Settings.Default.totalBytesTransfered = Transfer.TransferTotalKiloByte * 1024;
            Settings.Default.totalTimeTransfered = Transfer.TransferTotalTimeSec;

            // Save settings
            //--------------
            Settings.Default.Save();

            // Cleanup Transfer
            //-----------------
            Transfer.Cleanup();

            if (Left > -1000 && Top > -1000)
            {
                Settings.Default.WindowPositionX = Left;
                Settings.Default.WindowPositionY = Top;
            }

            // Save history
            //-------------
            if (Settings.Default.RestoreHistory)
            {
                try
                {
                    OutputBox.SaveFile(_historyFolderPath);
                    _pathAutoCompleteList.SaveFile(_pathAutoCompleteHistoryFilePath);
                    _ipAutoCompleteList.SaveFile(_ipAutoCompleteHistoryFilePath);
                    _lockedSettingsHistory.SaveFile(_lockedSettingsHistoryFilePath);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            // Save settings
            //--------------
            Settings.Default.Save();

            //----------------
            // Backup settings
            //----------------

            // Copy all settings files in parent settings directory
            //-----------------------------------------------------
            string currentVersionFolder = Path.GetDirectoryName(_historyFolderPath);
            if (currentVersionFolder != null)
            {
                string parentFolder = Directory.GetParent(Directory.GetParent(currentVersionFolder).FullName)
                    .FullName;
                string[] files = Directory.GetFiles(currentVersionFolder);

                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);
                    File.Copy(filePath, Path.Combine(parentFolder, fileName), true);
                }
            }
        }
    }
}
