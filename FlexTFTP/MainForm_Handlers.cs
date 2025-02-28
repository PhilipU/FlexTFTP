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
        private void textBoxFilePath_Enter(object sender, EventArgs e)
        {
            CheckTextBox((TextBox)sender);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            CheckTextBox((TextBox)sender);
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Firmware Files|*.s19;*.fpga;*.fpga2|All Files|*.*";
            openFileDialog1.Title = "Select a file to download";
            openFileDialog1.Reset();
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(_openedPath);

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SetFilePath(openFileDialog1.FileName);
            }
        }

        private void outputTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if(e.LinkText == null)
            {
                return;
            }

            if (e.LinkText.StartsWith("http"))
            {
                var ps = new ProcessStartInfo(e.LinkText)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
                return;
            }
            string path = e.LinkText.Substring("file://".Length);
            path = path.Replace("%20", " ");
            SetFilePath(path);
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            if (Transfer.ToggleState(_openedPath, _targetPath, textBoxAddress.Text, int.Parse(maskedTextBoxPort.Text)))
            {
                _pathAutoCompleteList.AddEntry(_targetPath);
                _ipAutoCompleteList.AddEntry(textBoxAddress.Text);

                if (!Transfer.InProgress())
                {
                    OutputBox.AddLine("Transfer cancelled!", Color.Orange, true);
                }
            }
        }

        private void textBoxFilePath_Leave(object sender, EventArgs e)
        {
            SetFilePath(textBoxFilePath.Text);
        }

        private void autoPathCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoPath = autoPathCheckBox.Checked;
            Settings.Default.Save();

            if (autoPathCheckBox.Checked)
            {
                string parsedPath = TargetPathParser.GetTargetPath(_openedPath);

                if (parsedPath != null && !_targetPath.Equals(parsedPath))
                {
                    SetAutoPath(parsedPath);
                }
            }

            CheckPresetActive();
        }

        private void maskedTextBoxPort_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            int.TryParse(maskedTextBoxPort.Text, out var port);
            Settings.Default.Port = port;
            Settings.Default.Save();

            CheckPresetActive();
        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {
            _currentOnlineState = false;
            if (Settings.Default.OnlineCheck)
            {
                pictureBoxOnlineState.Image = Resources.disconnected;
            }

            if (textBoxAddress.Text == "localhost")
            {
                textBoxAddress.Text = "127.0.0.1";
            }

            IPAddress.TryParse(textBoxAddress.Text, out var ipAddress);
            if (ipAddress == null)
            {
                return;
            }

            if (Equals(_currentIpAddress, ipAddress))
            {
                return;
            }

            _currentIpAddress = ipAddress;

            if (textBoxAddress.Text.Length >= 7)
            {
                _currentOnlineState = false;
                OnlineStateChangeInvoker();
                _onlineChecker.ChangeAddress(_currentIpAddress);
                Settings.Default.HostAddress = textBoxAddress.Text;
                Settings.Default.Save();
            }

            CheckPresetActive();
        }

        private void FlexTFTPForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void FlexTFTPForm_DragDrop(object sender, DragEventArgs e)
        {
            if(e.Data == null)
            { 
                return;
            }
            string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);
            if(files == null)
            {
                return;
            }

            foreach (string file in files)
            {
                SetFilePath(file);
                return;
            }
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            string oldPath = _targetPath;

            SetTargetPath(textBoxPath.Text);
            Settings.Default.Path = textBoxPath.Text;

            if (Settings.Default.AutoPath && !oldPath.Equals(textBoxPath.Text))
            {
                string parsedPath = TargetPathParser.GetTargetPath(_openedPath);

                if (parsedPath != null && !_targetPath.Equals(parsedPath))
                {
                    SetSettingAutoPath(false);
                    OutputBox.AddLine("Auto path option disabled.");
                }
            }

            CheckPresetActive();
        }

        private void ToggleKeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(_openedPath) && GetKeyState(Keys.Return) < 0 && (e.KeyCode == Keys.Return))
            {
                Transfer.StartTransfer(_openedPath, _targetPath, textBoxAddress.Text, int.Parse(maskedTextBoxPort.Text));
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (Transfer.StopTransfer())
                {
                    OutputBox.AddLine("Transfer cancelled!", Color.Orange, true);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OutputBox.Clear();
            OutputBox.AddLine(Resources.Changelog, FontFamily.GenericMonospace, 7F, false);
            OutputBox.AddLine("------------------------------------------------------------------------------------------", false);
        }

        private void pictureBoxSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ShowDialog(this);
        }

        private void hotInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var ps = new ProcessStartInfo(_hotInfoLink)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            }
            catch (InvalidOperationException)
            {

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (_settingsLocked)
            {
                UnlockSettings();
            }
            else
            {
                LockSettings();
            }
        }

        private void pictureBoxPreset1_Click(object sender, EventArgs e)
        {
            if (textBoxAddress.Text != Settings.Default.Preset1Address)
            {
                textBoxAddress.Text = Settings.Default.Preset1Address;
            }

            if (!Settings.Default.Preset1AutoPath && textBoxPath.Text != Settings.Default.Preset1Path)
            {
                textBoxPath.Text = Settings.Default.Preset1Path;
            }

            if (Settings.Default.AutoPath != Settings.Default.Preset1AutoPath)
            {
                SetSettingAutoPath(Settings.Default.Preset1AutoPath);
            }

            if (maskedTextBoxPort.Text != Settings.Default.Preset1Port)
            {
                maskedTextBoxPort.Text = Settings.Default.Preset1Port;
            }

            LockSettings();

            CheckPresetActive();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            if (textBoxAddress.Text != Settings.Default.Preset2Address)
            {
                textBoxAddress.Text = Settings.Default.Preset2Address;
            }

            if (!Settings.Default.Preset2AutoPath && textBoxPath.Text != Settings.Default.Preset2Path)
            {
                textBoxPath.Text = Settings.Default.Preset2Path;
            }

            if (Settings.Default.AutoPath != Settings.Default.Preset2AutoPath)
            {
                SetSettingAutoPath(Settings.Default.Preset2AutoPath);
            }

            if (maskedTextBoxPort.Text != Settings.Default.Preset2Port)
            {
                maskedTextBoxPort.Text = Settings.Default.Preset2Port;
            }

            LockSettings();

            CheckPresetActive();
        }
    }
}
