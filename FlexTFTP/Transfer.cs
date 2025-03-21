﻿using FlexTFTP.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tftp.Net;

namespace FlexTFTP
{
    public class Transfer
    {
        private readonly FlexTftpForm _form;
        private TftpClient _tftpClient;
        private ITftpTransfer _transfer;
        private bool _transferInProgress;
        private delegate void DelegateProgress(TftpTransferProgress progress);
        private delegate void DelegateError(TftpTransferError error);
        private delegate void DelegateText(string text, Color color);
        private readonly DelegateProgress _delegateProgress;
        private readonly DelegateError _delegateError;
        private readonly DelegateText _delegateText;
        private DateTime _startTime;
        private bool _closeAfterwards;
        private int _closeTimerCount;
        private long _transferTotalTimeSec;
        private long _transferTotalkiloByte;
        private long _lastFileSize = 0;
        private string _path = "";
        private string _file = "";
        private string _address = "";
        private int _port;
        private FileStream _fileStream;
        private string _tempFilePath1;
        private string _tempFilePath2;
        private int _currentTempFile = 1;
        private bool _activeError = false;
        private System.Threading.Timer _closeTimer = null;
        private int _percentage;
        private string _lastError = "";

        public long LastFileSize
        {
            get
            {
                return _lastFileSize;
            }
        }

        public int Percentage
        {
            get
            {
                return _percentage;
            }
        }

        public string LastError
        {
            get
            {
                return _lastError;
            }
        }

        public bool ActiveError
        {
            get
            {
                return _activeError;
            }
        }

        public long TransferTotalTimeSec
        {
            get => _transferTotalTimeSec;
            set => _transferTotalTimeSec = value;
        }

        public long TransferTotalKiloByte
        {
            get => _transferTotalkiloByte;
            set => _transferTotalkiloByte = value;
        }

        public Transfer(FlexTftpForm form)
        {
            _form = form;
            _delegateProgress = UpdateProgress;
            _delegateError = SetTransferFailed;
            _delegateText = UpdateText;
            _tempFilePath1 = Path.GetTempFileName();
            _tempFilePath2 = Path.GetTempFileName();
        }

        public Transfer()
        {
            _form = null;
            _delegateProgress = UpdateProgress;
            _delegateError = SetTransferFailed;
            _delegateText = UpdateText;
            _tempFilePath1 = Path.GetTempFileName();
            _tempFilePath2 = Path.GetTempFileName();
        }

        private void Restart()
        {
            ToggleState(_file, _path, _address, _port);
        }

        public void Cleanup()
        {
            if (File.Exists(_tempFilePath1))
            {
                File.Delete(_tempFilePath1);
            }

            if (File.Exists(_tempFilePath2))
            {
                File.Delete(_tempFilePath2);
            }
        }

        public bool ToggleState(string file, string path, string address, int port)
        {
            if (_transferInProgress)
            {
                return StopTransfer();
            }

            _transferInProgress = true;

            if (!File.Exists(file))
            {
                if(_form != null) _form.OutputBox.AddLine("Error: File does not exist!", Color.Red, true);
                _activeError = true;
                StopTransfer();
                return false;
            }

            try
            {
                _tftpClient = new TftpClient(address, port);
            }
            catch
            {
                if (_form != null) _form.OutputBox.AddLine("Error: Invalid host address!", Color.Red, true);
                _activeError = true;
                StopTransfer();
                return false;
            }

            _path = path;
            _file = file;
            _address = address;
            _port = port;
            _transfer = _tftpClient.Upload(path);
            _transfer.RetryCount = Settings.Default.TransferRetryCount;
            _transfer.RetryTimeout = TimeSpan.FromSeconds(Settings.Default.TransferTimeoutSec);
            _transfer.UserContext = _form;
            _transfer.OnProgress += transfer_OnProgress;
            _transfer.OnFinished += transfer_OnFinshed;
            _transfer.OnError += transfer_OnError;

            try
            {
                string tempFilePath;
                if(_currentTempFile == 1)
                {
                    tempFilePath = _tempFilePath2;
                    _currentTempFile = 2;
                }
                else
                {
                    tempFilePath = _tempFilePath1;
                    _currentTempFile = 1;
                }

                // Delete existing temporary file
                //-------------------------------
                if(File.Exists(tempFilePath))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(tempFilePath);
                }

                // Copy given file to temp to avoid locking original file
                //-------------------------------------------------------
#if DEBUG
                if(_form != null) _form.OutputBox.AddLine("[DEBUG] Created temp file: " + tempFilePath, Color.Gray, true);
#endif
                File.Copy(file, tempFilePath);

                // Create file stream from temp file
                //----------------------------------
                _fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                _lastFileSize = _fileStream.Length;
                _startTime = DateTime.UtcNow;
                _transfer.Start(_fileStream);
                if (_form != null)
                {
                    _form.SetProgress(1);
                    // Task bar progress
                    var prog = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                    prog.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.Normal);
                    prog.SetProgressValue(1, 100);
                }

                // Show connect info
                //------------------
                if (_form != null) _form.ShowConnectScreen(true);
            }
            catch
            {
                if (_form != null) _form.OutputBox.AddLine("Error: Can not open file!", Color.Red, true);
                _activeError = true;
                StopTransfer();
                return false;
            }

            if (_form != null) _form.OutputBox.AddLine("Transfer started (" + address + ")", Color.Black, true);
            if (_form != null) _form.SetTransferStateButtonText("Cancel");
            _activeError = false;

            return true;
        }

        public bool StartTransfer(string file, string path, string address, int port)
        {
            if (_transferInProgress) return false;
            return ToggleState(file, path, address, port);
        }

        public bool StopTransfer()
        {
            if(!_transferInProgress) return false;

            _transferInProgress = false;
            if (_transfer != null)
            {
                new Task(() => 
                {
                    _transfer.Cancel(TftpErrorPacket.NoSuchUser);
                }).Start();
            }

            // Hide connect info
            //------------------
            if (_form != null) _form.ShowConnectScreen(false);

            if (_form != null)
            {
                _form.SetProgress(0);
                var prog = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                prog.SetProgressState(Microsoft.WindowsAPICodePack.Taskbar.TaskbarProgressBarState.NoProgress);
                _form.SetTransferStateButtonText("Download");
            }

            if(_closeAfterwards)
            {
                if (_closeTimerCount == 0)
                {
                    CloseTimerRunnable(null);
                }
                else
                {
                    if (_form != null) _form.OutputBox.AddLine();
                    if (_form != null) _form.OutputBox.AddLine("Application will be closed in " + _closeTimerCount + "sec", Color.Black);

                    if(_closeTimerCount > 0)
                    {
                        _closeTimerCount--;
                    }

                    if(_closeTimer == null)
                    {
                        _closeTimer = new System.Threading.Timer(CloseTimerRunnable, null, 1000, 1000);
                    }
                }
            }

            return true;
        }

        private void CloseTimerRunnable(object state)
        {
            if (0 == _closeTimerCount)
            {
                if(_closeTimer != null)
                {
                    _closeTimer.Dispose();
                }

                if (_form != null)
                {
                    _form.Invoke((MethodInvoker)delegate { _form.Close(); });
                }
            }
            else
            {
                _closeTimerCount--;
                object[] parametersArray = new object[] { _closeTimerCount + " ", Color.Red };
                if (_form != null)
                {
                    _form.Invoke(_delegateText, parametersArray);
                }
            }
        }

        public bool InProgress()
        {
            return _transferInProgress;
        }

        private void UpdateText(string text, Color color)
        {
            if (_form != null) _form.OutputBox.AddLine(text, color);
        }

        public void UpdateProgress(TftpTransferProgress progress)
        {
            if (!_transferInProgress) return;
            _percentage = Convert.ToInt32((Convert.ToDouble(progress.TransferredBytes) / (Convert.ToDouble(progress.TotalBytes)) * 100D));
            if (_percentage == 0) _percentage = 1;

            if (_form != null)
            {
                if (_form.GetProgress() == _percentage)
                {
                    return;
                }

                // Hide connect info
                //------------------
                _form.ShowConnectScreen(false);

                _form.SetProgress(_percentage);
                _form.SetProgress((_percentage > 0 && _percentage < 100) ? _percentage - 1 : _percentage);

                var prog = Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager.Instance;
                prog.SetProgressValue(_percentage, 100);
            }

        }

        public void SetTransferFailed(TftpTransferError error)
        {
            if (!_transferInProgress) return;
            string errorMessage = ErrorMessageTranslator.TranslateError(error.ToString());
            _activeError = true;
            StopTransfer();
            if((_path.StartsWith("cpu") ||_path.StartsWith("ram")) && _file.EndsWith(".s19") && Settings.Default.AutoForce && !_path.Contains("-force") && 
                error.ToString().Contains("Invalid device type"))
            {
                if (_form != null) _form.OutputBox.AddLine("Failed. Retry with '-force' option", Color.Orange, true);
                _path += " -force";
                Restart();
            }
            else
            {
                if (_form != null) _form.OutputBox.AddLine("Failed: " + errorMessage, Color.Red, true);
                _lastError = errorMessage;
            }
        }

        public void SetTransferFinished()
        {
            if (!_transferInProgress) return;
            TimeSpan transferTime = DateTime.UtcNow- _startTime;
            double speed = Math.Round(_lastFileSize/1024D/1024D/transferTime.TotalSeconds, 2);
            if (_form != null) _form.OutputBox.AddLine("Finished in " + Math.Round(transferTime.TotalSeconds) + "s (" + speed + "MB/s)", Color.Green, true);
            _transferTotalTimeSec += (int)Math.Round(transferTime.TotalSeconds);
            _transferTotalkiloByte += (int)(_lastFileSize / 1024);
            _activeError = false;
            StopTransfer();
        }

        public void transfer_OnProgress(ITftpTransfer transfer, TftpTransferProgress progress)
        {
            if (transfer.UserContext != null)
            {
                FlexTftpForm form = (FlexTftpForm)transfer.UserContext;

                if (!_transferInProgress) return;

                object[] parametersArray = new object[] { progress };
                form.Invoke(_delegateProgress, parametersArray);
            }
            else
            {
                UpdateProgress(progress);
            }
        }

        public void transfer_OnError(ITftpTransfer transfer, TftpTransferError error)
        {
            if (transfer.UserContext != null)
            {
                FlexTftpForm form = (FlexTftpForm)transfer.UserContext;

                if (!_transferInProgress) return;

                object[] parametersArray = new object[] { error };
                form.Invoke(_delegateError, parametersArray);
            }
            else
            {
                SetTransferFailed(error);
            }
        }

        public void transfer_OnFinshed(ITftpTransfer transfer)
        {
            if (transfer.UserContext != null)
            {
                FlexTftpForm form = (FlexTftpForm)transfer.UserContext;

                if (!_transferInProgress) return;

                form.Invoke(new MethodInvoker(SetTransferFinished));
            }
            else
            {
                SetTransferFinished();
            }
        }

        public void CloseAfterwards(bool enabled, int delay)
        {
            _closeAfterwards = enabled;
            _closeTimerCount = delay == 0 ? 1 : delay;
        }
    }
}
