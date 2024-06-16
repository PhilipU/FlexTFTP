using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FlexTFTP
{
    class Utils
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private const int WmVscroll = 0x115;
        private const int SbBottom = 7;

        /// <summary>
        /// Scrolls the vertical scroll bar of a multi-line text box to the bottom.
        /// </summary>
        /// <param name="tb">The text box to scroll</param>
        public static void ScrollToBottom(RichTextBox tb)
        {
            SendMessage(tb.Handle, WmVscroll, (IntPtr)SbBottom, IntPtr.Zero);
        }

        [DllImport("user32.dll", EntryPoint = "GetKeyboardState", SetLastError = true)]
        private static extern bool NativeGetKeyboardState([Out] byte[] keyStates);

        private static bool GetKeyboardState(byte[] keyStates)
        {
            if (keyStates == null)
                throw new ArgumentNullException(nameof(keyStates));
            if (keyStates.Length != 256)
                throw new ArgumentException("The buffer must be 256 bytes long.", nameof(keyStates));
            return NativeGetKeyboardState(keyStates);
        }

        private static byte[] GetKeyboardState()
        {
            byte[] keyStates = new byte[256];
            if (!GetKeyboardState(keyStates))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return keyStates;
        }

        public static bool AnyKeyPressed()
        {
            byte[] keyState = GetKeyboardState();
            // skip the mouse buttons
            return keyState.Skip(8).Any(state => (state & 0x80) != 0);
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public static double CurrentVersion => Convert.ToDouble(Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 4), CultureInfo.InvariantCulture);

        public static string GetReadableSize(double bytes)
        {
            string[] sizes = { " Byte", " KB", " MB", " GB", " TB" };
            int order = 0;
            while (bytes >= 1024 && ++order < sizes.Length)
            {
                bytes = bytes / 1024;
            }
            return Math.Round(bytes, 2) + sizes[order];
        }

        public static string GetReadableTime(long seconds)
        {
            int min = 0;
            int hours = 0;
            while (seconds >= 60)
            {
                seconds -= 60;
                min++;
                if (min >= 60)
                {
                    min -= 60;
                    hours++;
                }
            }
            return hours.ToString("D2") + ":" + min.ToString("D2") + ":" + seconds.ToString("D2");
        }

        public static string GetStringBeforeSecondSlash(string input)
        {
            if (input == null) return null;

            int firstSlashIndex = input.IndexOf('/');
            if (firstSlashIndex <= 0) return null;

            int secondSlashIndex = input.IndexOf('/', firstSlashIndex + 1);
            // If there is no second slash return whole input string
            if (secondSlashIndex <= 0)
            {
                return input;
            }

            return input.Substring(0, secondSlashIndex);
        }

        public static void ClearArpTable()
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd")
            {
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            using (Process process = Process.Start(processInfo))
            {
                if (process == null) return;
                process.StandardInput.WriteLine("arp -d *");
                process.StandardInput.WriteLine("exit");
            }
        }
    }
}
