using FlexTFTP.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
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

            Text += " v" + Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 4);
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
            // Process command line parameters
            //--------------------------------
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                _openedByCommandLine = true;

                // Print info
                //-----------
                OutputBox.AddLine("Opened by command line", Color.Black, true);

                // Close application afterwards if started by command line
                //---------------------------------------------
                Transfer.CloseAfterwards(true, 3);

                Settings.Default.AutoForce = false;

                // Get path as second argument
                //----------------------------
                if (args.Length > 2)
                {
                    OutputBox.AddLine("Target path is set to: " + args[2], Color.Black, true);
                    if (args[2].Equals("auto", StringComparison.OrdinalIgnoreCase))
                    {
                        SetSettingAutoPath(true);
                    }
                    else
                    {
                        OutputBox.AddLine("Disable auto path", Color.Black, true);
                        SetSettingAutoPath(false);
                        Settings.Default.TypeDependendAutpPath = false; // Prevent reenabling auto path due to other checks
                        SetTargetPath(args[2]);
                    }
                }
                else
                {
                    SetSettingAutoPath(true);
                }

                // Get target IP as third argument
                //--------------------------------
                if (args.Length > 3)
                {
                    if (!args[3].Equals("last", StringComparison.OrdinalIgnoreCase))
                    {
                        textBoxAddress.Text = args[3];
                    }
                }

                // Get target port as fourth argument
                //-----------------------------------
                if (args.Length > 4)
                {
                    if (!args[4].Equals("last", StringComparison.OrdinalIgnoreCase))
                    {
                        maskedTextBoxPort.Text = args[4];
                    }
                }

                SetFilePath(args[1]);

                // Close GUI after transfer
                //-------------------------
                if (args.Length > 5)
                {
                    if (args[5].Equals("notclose", StringComparison.OrdinalIgnoreCase))
                    {
                        Transfer.CloseAfterwards(false, 0);
                        _openedByCommandLine = false;
                    }
                    else if (args[5].Equals("close", StringComparison.OrdinalIgnoreCase))
                    {
                        Transfer.CloseAfterwards(true, 0);
                    }
                }

                Transfer.ToggleState(_openedPath, _targetPath, textBoxAddress.Text, int.Parse(maskedTextBoxPort.Text));
            }
            // Normal operation
            //-----------------
            else
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

                // Load last opened file
                //----------------------
                if (Settings.Default.RestoreLastOpenedFile &&
                    Settings.Default.LastOpenedFile.Length != 0 &&
                    File.Exists(Settings.Default.LastOpenedFile))
                {
                    SetFilePath(Settings.Default.LastOpenedFile);
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

            if (!_openedByCommandLine)
            {
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
}
