using FlexTFTP.Properties;
using Microsoft.WindowsAPICodePack.Taskbar;
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
        private string _openedPath;
        private string _targetPath = "";
        public Transfer Transfer;
        private string _processedPath;
        private Updater _updater;
        private string _hotInfoLink = "";
        private readonly string _historyFolderPath;
        private readonly string _settingsDataRootPath;
        private readonly string _pathAutoCompleteHistoryFilePath;
        private readonly string _ipAutoCompleteHistoryFilePath;
        private readonly string _lockedSettingsHistoryFilePath;
        private bool _openedByCommandLine;
        private AutoCompleteList _pathAutoCompleteList;
        private AutoCompleteList _ipAutoCompleteList;
        private LockedSettingsHistory _lockedSettingsHistory;
        private bool _settingsLocked;
        private FileWatcher _fileWatcher;
        private OnlineChecker _onlineChecker;
        private IPAddress _currentIpAddress;
        private bool _currentOnlineState;
        private bool _activeError = false;
        
        public bool ActiveError
        {
            get
            {
                return _activeError | Transfer.ActiveError;
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(Keys key);

        public Updater Updater => _updater;

        public FlexTftpForm()
        {
            InitializeComponent();

            DoubleBuffered = true;

            _historyFolderPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            _settingsDataRootPath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(_historyFolderPath)).FullName).FullName;
            _pathAutoCompleteHistoryFilePath = _historyFolderPath + ".path_auto_complete.conf";
            _ipAutoCompleteHistoryFilePath = _historyFolderPath + ".address_auto_complete.conf";
            _lockedSettingsHistoryFilePath = _historyFolderPath + ".locked_settings.conf";
        }

        public void UpdateSettings()
        {
            // Online Check
            //-------------
            _currentOnlineState = false;
            if (Settings.Default.OnlineCheck)
            {
                pictureBoxOnlineState.Image = Resources.disconnected;
                _onlineChecker.StartCyclicCheck(Settings.Default.OnlineCheckIntervalMs);
            }
            else
            {
                pictureBoxOnlineState.Image = null;
                _onlineChecker.StopCyclicCheck();
            }

            // Presets
            //--------
            pictureBoxPreset1.Visible = Settings.Default.PresetEnabled;
            pictureBoxPreset2.Visible = Settings.Default.PresetEnabled;
            CheckPresetActive();

            // Updater
            //--------
            TriggerUpdateCheck();
        }

        private static void SetUpdateInfo(Updater updater, Form form)
        {
            FlexTftpForm flexTftpForm = (FlexTftpForm)form;
            flexTftpForm._hotInfoLink = updater.UpdateLink;
            flexTftpForm.hotInfo.Text = "Update available! New Version: " + updater.NewestVersionName;
            flexTftpForm.hotInfo.Visible = true;

            if(Settings.Default.AutoUpdate)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to update to " + updater.NewestVersionName + 
                    " (" + ((updater.Beta)? "Beta" : "Stable") + ")\r\nfrom " + updater.NewestDate
#if DEBUG 
                    + " [Current: " + updater.CurrentDate + "]" 
#endif
                    ,"Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    updater.ProcessDownload();
                }
            }
        }

        public void TriggerUpdateCheck()
        {
            hotInfo.Text = "";
            if (Settings.Default.UpdateEnabled)
            {
                _updater.CheckForUpdateAsync(SetUpdateInfo, this, Settings.Default.AutoUpdate, Settings.Default.UpdateBetaRing);
            }
        }

        public OutputBox OutputBox { get; set; }

        private void CheckTextBox(TextBox textBox)
        {
            if(textBox.ForeColor == Color.Silver)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        public int GetProgress()
        {
            return progressBarDownload.Value;
        }

        public void SetProgress(int value)
        {
            //if(value == 0)
            //{
            //    progressBarDownload.DisplayStyle = ProgressBarDisplayText.CustomText;
            //    progressBarDownload.CustomText = "";
            //}
            //else
            {
                progressBarDownload.DisplayStyle = ProgressBarDisplayText.Percentage;
                progressBarDownload.Value = value;
            }
        }

        public void ShowConnectScreen(bool show)
        {
            if (show)
            {
                progressBarDownload.DisplayStyle = ProgressBarDisplayText.CustomText;
                progressBarDownload.CustomText = "Connect...";
            }
            else
            {
                progressBarDownload.DisplayStyle = ProgressBarDisplayText.Percentage;
            }
        }

        public void SetTransferStateButtonText(string text)
        {
            buttonDownload.Text = text;
        }

        private void SetTargetPath(string path)
        {
            CheckTextBox(textBoxPath);

            textBoxPath.Text = path;
            _targetPath = path;
        }

        public void SetAutoPath(string filePath)
        {
            string parsedPath = TargetPathParser.GetTargetPath(_openedPath);
            if (parsedPath == null)
            {
                OutputBox.AddLine("Warning: Could not determine target path!", Color.Orange, true);
                SetTargetPath("");
            }
            else
            {
                OutputBox.Add("Auto path: ");
                OutputBox.AddLine(parsedPath, Color.Green);
                SetTargetPath(parsedPath);

                // Get list of all headers in file.
                // If multiple header found we have to inform user
                //------------------------------------------------
                double len = new FileInfo(_openedPath).Length;
                int minSize = Settings.Default.MultiFileMinSize;
                if (len >= minSize)
                {
                    SRecord srecord = new SRecord(_openedPath);
                    List<string> multiHeaders = srecord.GetHeaderPathList();
                    if (multiHeaders.Count > 1)
                    {
                        OutputBox.AddLine("This file contains multiple flash images:");
                        foreach (var header in multiHeaders)
                        {
                            String flashPath = TargetPathParser.GetPathByName(header);
                            OutputBox.AddLine(header + " -> " + flashPath);
                        }
                    }
                }
            }
        }

        public void SetFilePath(string filePath)
        {
            string previousTargetPath = _targetPath;

            // Prevent event loop by loosing focus
            if (filePath.Equals(_processedPath))
            {
                return;
            }

            _processedPath = filePath;
            string parsedPath = TargetPathParser.GetTargetPath(filePath);


            if (filePath.Equals(_openedPath) && OutputBox.PathIsKnown(filePath))
            {
                _processedPath = null;

                if (Settings.Default.AutoPath)
                {
                    if (parsedPath != null && !_targetPath.Equals(parsedPath))
                    {
                        SetAutoPath(filePath);
                    }
                }
                return;
            }

            if(!File.Exists(filePath))
            {
                if (filePath.Length > 0)
                {
                    OutputBox.AddLine("Error: File does not exist!", Color.Red, true);
                }
                _processedPath = null;
                _activeError = true;
                return;
            }

            CheckTextBox(textBoxFilePath);
            _openedPath = filePath;
            textBoxFilePath.Text = filePath;

            // Scroll right
            textBoxFilePath.Select(textBoxFilePath.Text.Length, 0);
            textBoxFilePath.Focus();
            outputTextBox.Focus();

            OutputBox.AddLine("Opened file:");
            OutputBox.AddFilePath(filePath);

            // Show file size
            double fileSizeBytes = new FileInfo(filePath).Length;
            DateTime lastWriteTime = File.GetLastWriteTime(filePath);
            string lastWriteString = lastWriteTime.ToString("dd.MM.yyyy - HH:mm:ss");
            OutputBox.AddLine("Size: " + Utils.GetReadableSize(fileSizeBytes));
            OutputBox.AddLine("Last Save: " + lastWriteString);

            // Parse S-Record target path
            //---------------------------

            // If base file target changes (e.g. cpu/application/3 -> fpga/application)
            // we should reenable Auto Path option even if it was disabled by user
            // if TypDependendAutoPath settings option is enabled.
            bool typeDependendAutoPath = false;
            if (!Settings.Default.AutoPath && Settings.Default.TypeDependendAutpPath && 
                parsedPath != null && previousTargetPath != null && parsedPath.Length > 0 && previousTargetPath.Length > 0)
            {
                string previousTargetPathPart = Utils.GetStringBeforeSecondSlash(previousTargetPath);
                string currentTargetPathPart = Utils.GetStringBeforeSecondSlash(parsedPath);

                if(previousTargetPathPart != currentTargetPathPart)
                {
                    typeDependendAutoPath = true;
                    SetSettingAutoPath(true); // This will cause SetAutoPath() -> Skip the call below
                }
            }
            if (Settings.Default.AutoPath && !typeDependendAutoPath)
            {
                SetAutoPath(_openedPath);
            }

            _processedPath = null;

            // Handle Locked Settings
            //-----------------------
            LockedSettings lockedSettings = _lockedSettingsHistory.GetEntry(_openedPath);
            if(lockedSettings != null)
            {
                LoadLockedSettings(lockedSettings);
            }
            else
            {
                UnlockSettings();
            }

            // Register file at FileWatcher
            //-----------------------------
            _fileWatcher.SetFile(_openedPath);

            // Save as last opened file
            Settings.Default.LastOpenedFile = _openedPath;
            Settings.Default.Save();
        }

        private void LockSettings()
        {
            if(_settingsLocked || string.IsNullOrEmpty(_openedPath))
            {
                return;
            }
            LockedSettings newLockedSettings = new LockedSettings(_openedPath, 
                _targetPath, textBoxAddress.Text, maskedTextBoxPort.Text, Settings.Default.AutoPath);
            _lockedSettingsHistory.AddEntry(newLockedSettings);
            pictureBox_lockSettings.Image = Resources.lock_closed;
            textBoxPath.Enabled = false;
            textBoxAddress.Enabled = false;
            maskedTextBoxPort.Enabled = false;
            autoPathCheckBox.Enabled = false;
            _settingsLocked = true;
        }

        private void UnlockSettings()
        {
            if(!_settingsLocked)
            {
                return;
            }
            _lockedSettingsHistory.RemoveEntry(_openedPath);
            pictureBox_lockSettings.Image = Resources.lock_open;
            textBoxPath.Enabled = true;
            textBoxAddress.Enabled = true;
            maskedTextBoxPort.Enabled = true;
            autoPathCheckBox.Enabled = true;
            _settingsLocked = false;
        }

        private void LoadLockedSettings(LockedSettings lockedSettings)
        {
            SetSettingAutoPath(lockedSettings.AutoPath);
            textBoxPath.Text = lockedSettings.TargetPath;
            textBoxAddress.Text = lockedSettings.Ip;
            maskedTextBoxPort.Text = lockedSettings.Port;

            LockSettings();
        }

        private void OnlineCheckerCallback(IPAddress address, bool online)
        {
            // Check if result of previous IP address
            if(!Equals(address, _currentIpAddress))
            {
                return;
            }

            // Ignore same online state
            if(_currentOnlineState == online)
            {
                return;
            }

            _currentOnlineState = online;

            Invoke(new MethodInvoker(OnlineStateChangeInvoker));
        }

        private void OnlineStateChangeInvoker()
        {
            if(Settings.Default.OnlineCheck)
            {
                pictureBoxOnlineState.Image = _currentOnlineState ? Resources.connected : Resources.disconnected;
            }
        }

        private void CheckPresetActive()
        {
            // Preset 1
            //---------
            if (textBoxAddress.Text != Settings.Default.Preset1Address ||
                (!Settings.Default.Preset1AutoPath && textBoxPath.Text != Settings.Default.Preset1Path) ||
                Settings.Default.AutoPath != Settings.Default.Preset1AutoPath ||
                maskedTextBoxPort.Text != Settings.Default.Preset1Port)
            {
                pictureBoxPreset1.Image = Resources.preset1_inactive;
            }
            else
            {
                pictureBoxPreset1.Image = Resources.preset1_active;
            }

            // Preset 2
            //---------
            if (textBoxAddress.Text != Settings.Default.Preset2Address ||
                (!Settings.Default.Preset2AutoPath && textBoxPath.Text != Settings.Default.Preset2Path) ||
                Settings.Default.AutoPath != Settings.Default.Preset2AutoPath ||
                maskedTextBoxPort.Text != Settings.Default.Preset2Port)
            {
                pictureBoxPreset2.Image = Resources.preset2_inactive;
            }
            else
            {
                pictureBoxPreset2.Image = Resources.preset2_active;
            }
        }

        private void SetSettingAutoPath(bool autoPath)
        {
            //OutputBox.AddLine("Auto Path Set:" + autoPath, Color.Black, true);
            Settings.Default.AutoPath = autoPath;
            autoPathCheckBox.Checked = autoPath;
        }

        private void FlexTftpForm_Enter(object sender, EventArgs e)
        {
            
        }

        private void FlexTftpForm_Activated(object sender, EventArgs e)
        {
            var prog = TaskbarManager.Instance;
            if (!Transfer.InProgress())
            {
                prog.SetProgressValue(0, 100);
                prog.SetProgressState(TaskbarProgressBarState.NoProgress);
            }
        }
    }
}
