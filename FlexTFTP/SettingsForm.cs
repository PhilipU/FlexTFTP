using FlexTFTP.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace FlexTFTP
{
    public partial class SettingsForm : Form
    {

        FlexTftpForm _flexTftPform;
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            Settings.Default.MultiFileMinSize = Convert.ToInt32(maskedTextBoxMultiTargetFileMinSize.Text) * 1024 * 1024;
            Settings.Default.UpdateEnabled = updateCheck.Checked;
            Settings.Default.AutoUpdate = checkBoxAutoUpdate.Checked;
            Settings.Default.UpdateBetaRing = checkBoxUpdateBetaRing.Checked;

            Settings.Default.RestoreWindowPosition = checkBoxRestoreWindowPos.Checked;
            Settings.Default.RestoreLastOpenedFile = checkBoxRestoreLastFile.Checked;
            Settings.Default.RestoreIPAddress = checkBoxRestoreIP.Checked;
            Settings.Default.RestorePort = checkBoxRestorePort.Checked;
            Settings.Default.RestorePath = checkBoxRestorePath.Checked;
            Settings.Default.RestoreHistory = checkBoxRestoreHistory.Checked;
            Settings.Default.TypeDependendAutpPath = checkBoxRestoreAutoPath.Checked;

            Settings.Default.TransferRetryCount = Convert.ToInt32(maskedTextBoxTransferRetryCount.Text);
            Settings.Default.TransferTimeoutSec = Convert.ToInt32(maskedTextBoxTransferRetryTimeout.Text);
            Settings.Default.AutoForce = checkBoxAutoForce.Checked;

            Settings.Default.OnlineCheck = checkBoxOnlineCheck.Checked;
            Settings.Default.OnlineCheckIntervalMs = Convert.ToInt32(maskedTextBoxOnlineCheckInterval.Text);

            Settings.Default.ShowFullPath = checkBoxShowFullFilePath.Checked;

            // Presets
            //--------
            Settings.Default.PresetEnabled = checkBoxEnablePresets.Checked;

            Settings.Default.Preset1AutoPath = checkBoxPreset1AutoPath.Checked;
            Settings.Default.Preset1Port = maskedTextBoxPreset1Port.Text;
            Settings.Default.Preset1Path = textBoxPreset1Path.Text;
            Settings.Default.Preset1Address = textBoxPreset1Address.Text;

            Settings.Default.Preset2AutoPath = checkBoxPreset2AutoPath.Checked;
            Settings.Default.Preset2Port = maskedTextBoxPreset2Port.Text;
            Settings.Default.Preset2Path = textBoxPreset2Path.Text;
            Settings.Default.Preset2Address = textBoxPreset2Address.Text;

            Settings.Default.Save();

            Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            _flexTftPform = (FlexTftpForm)Owner;
            DateTime currentDateTime = File.GetLastWriteTime(GetType().Assembly.Location);
            string currentDateString = currentDateTime.ToString("dd.MM.yyyy HH:mm");

            maskedTextBoxMultiTargetFileMinSize.Text = Math.Round((Settings.Default.MultiFileMinSize / 1024D / 1024D), 0).ToString(CultureInfo.InvariantCulture);
            updateCheck.Checked = Settings.Default.UpdateEnabled;
            checkBoxAutoUpdate.Checked = Settings.Default.AutoUpdate;
            checkBoxUpdateBetaRing.Checked = Settings.Default.UpdateBetaRing;
            labelNewestVersion.Text = "Newest version: " + _flexTftPform.Updater.NewestVersion.ToString(CultureInfo.InvariantCulture).Replace(',','.') +
                " [" + _flexTftPform.Updater.NewestDate + "]";
            labelCurrentVersion.Text = "Current version:  " + Utils.CurrentVersion.ToString(CultureInfo.InvariantCulture).Replace(',', '.') +
                " [" + currentDateString + "]";

            checkBoxRestoreWindowPos.Checked = Settings.Default.RestoreWindowPosition;
            checkBoxRestoreLastFile.Checked = Settings.Default.RestoreLastOpenedFile;
            checkBoxRestoreIP.Checked = Settings.Default.RestoreIPAddress;
            checkBoxRestorePort.Checked = Settings.Default.RestorePort;
            checkBoxRestorePath.Checked = Settings.Default.RestorePath;
            checkBoxRestoreHistory.Checked = Settings.Default.RestoreHistory;
            checkBoxRestoreAutoPath.Checked = Settings.Default.TypeDependendAutpPath;

            maskedTextBoxTransferRetryCount.Text = Settings.Default.TransferRetryCount.ToString();
            maskedTextBoxTransferRetryTimeout.Text = Settings.Default.TransferTimeoutSec.ToString();
            checkBoxAutoForce.Checked = Settings.Default.AutoForce;

            checkBoxOnlineCheck.Checked = Settings.Default.OnlineCheck;
            maskedTextBoxOnlineCheckInterval.Text = Settings.Default.OnlineCheckIntervalMs.ToString();

            checkBoxShowFullFilePath.Checked = Settings.Default.ShowFullPath;

            // Presets
            //--------
            checkBoxEnablePresets.Checked = Settings.Default.PresetEnabled;

            checkBoxPreset1AutoPath.Checked = Settings.Default.Preset1AutoPath;
            maskedTextBoxPreset1Port.Text = Settings.Default.Preset1Port;
            textBoxPreset1Path.Text = Settings.Default.Preset1Path;
            textBoxPreset1Address.Text = Settings.Default.Preset1Address;

            checkBoxPreset2AutoPath.Checked = Settings.Default.Preset2AutoPath;
            maskedTextBoxPreset2Port.Text = Settings.Default.Preset2Port;
            textBoxPreset2Path.Text = Settings.Default.Preset2Path;
            textBoxPreset2Address.Text = Settings.Default.Preset2Address;

            groupBoxPreset1.Enabled = checkBoxEnablePresets.Checked;
            groupBoxPreset2.Enabled = checkBoxEnablePresets.Checked;

            // Session statistics
            //-------------------

            // Transfered bytes
            string readableSize = Utils.GetReadableSize(_flexTftPform.Transfer.TransferTotalKiloByte * 1024);
            labelSessionStatisticsBytes.Text = readableSize + " transfered";

            // Seconds taken to transfer
            string readableTime = Utils.GetReadableTime(_flexTftPform.Transfer.TransferTotalTimeSec);            
            labelSessionStatisticsTime.Text = readableTime + " h taken";

            // Show this window centered inside parent form
            Location = new Point(
                Owner.Left + (Owner.Width / 2) - (Width / 2),
                Owner.Top + (Owner.Height / 2) - (Height / 2));
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _flexTftPform.UpdateSettings();
        }

        private void maskedTextBoxOnlineCheckInterval_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void checkBoxEnablePresets_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxPreset1.Enabled = checkBoxEnablePresets.Checked;
            groupBoxPreset2.Enabled = checkBoxEnablePresets.Checked;
        }
    }
}
