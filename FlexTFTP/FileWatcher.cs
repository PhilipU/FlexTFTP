using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FlexTFTP
{
    class FileWatcher
    {
        private readonly FlexTftpForm _form;
        string _filePath;
        DateTime _lastCheckedFileChangeTime = DateTime.MinValue;
        readonly System.Timers.Timer _timer;

        public FileWatcher(FlexTftpForm form)
        {
            _timer = new System.Timers.Timer {Interval = 1000};
            _timer.Elapsed += Timer_Tick;
            _timer.Start();
            _form = form;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(_filePath == null)
            {
                return;
            }
            _form.Invoke(new MethodInvoker(ChangeInvoker));
            _timer.Start();
        }

        public void SetFile(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                ClearFile();
                return;
            }

            if(filePath == _filePath)
            {
                return;
            }

            _timer.Stop();

            _filePath = filePath;
            if (File.Exists(_filePath))
            {
                _lastCheckedFileChangeTime = File.GetLastWriteTime(_filePath);
            }

            _timer.Start();
        }

        public void ClearFile()
        {
            _timer.Stop();
            _filePath = "";
        }

        private void ChangeInvoker()
        {
            DateTime lastFileChangeTime = File.GetLastWriteTime(_filePath);
            DateTime now = DateTime.Now;
            FileInfo fileInfo = new FileInfo(_filePath);
            double fileSizeBytes = fileInfo.Length;
            long deltaLastWrite = now.Ticks - lastFileChangeTime.Ticks;

            if (Math.Abs(fileSizeBytes) > 0 && 
                lastFileChangeTime.Ticks != _lastCheckedFileChangeTime.Ticks && 
                deltaLastWrite > TimeSpan.TicksPerMillisecond * 250 /* 250ms */)
            {
                _lastCheckedFileChangeTime = lastFileChangeTime;
                _form.OutputBox.AddLine("File was updated. New size: " + Utils.GetReadableSize(fileSizeBytes), System.Drawing.Color.Black, true);
            }
        }
    }
}
