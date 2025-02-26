using FlexTFTP.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FlexTFTP
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const uint ATTACH_PARENT_PROCESS = 0xFFFFFFFF;

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 2)
            {
                // Check if console is available
                if (!NativeMethods.AttachConsole(NativeMethods.ATTACH_PARENT_PROCESS))
                {
                    // otherwise create new one
                    NativeMethods.AllocConsole();
                }

                Transfer transfer = new Transfer();
                string targetPath = "";
                string targetPathInfo = "";
                string file = args[1];

                // Print info
                //-----------
                string appTitle = "FlexTFTP";
                appTitle += " " + Utils.CurrentVersionString;
#if DEBUG
                appTitle += " [DEBUG]";
#endif
                Console.WriteLine("****************************************");
                Console.WriteLine(appTitle + " (started via CLI)");
                Console.WriteLine("****************************************");

                Console.WriteLine("File: " + file);

                Settings.Default.AutoForce = false;

                // Get path as second argument
                //----------------------------
                {
                    if (args[2].Equals("auto", StringComparison.OrdinalIgnoreCase) || args[2].Equals("auto-force", StringComparison.OrdinalIgnoreCase))
                    {
                        string parsedPath = TargetPathParser.GetTargetPath(file);
                        if (parsedPath != null)
                        {
                            targetPath = parsedPath;
                            targetPathInfo = targetPath + " (" + args[2] + ")";
                        }
                        else
                        {
                            Console.WriteLine("Unable to determine auto path (" + args[2] + "). Exitcode 1");
                            NativeMethods.FreeConsole();
                            Environment.Exit(1);
                        }

                        if (args[2].Equals("auto-force", StringComparison.OrdinalIgnoreCase))
                        {
                            targetPath += " -force";
                        }
                    }
                    else
                    {
                        Settings.Default.AutoPath = false;
                        Settings.Default.TypeDependendAutpPath = false; // Prevent reenabling auto path due to other checks
                        targetPath = args[2];
                        targetPathInfo = args[2];
                    }
                }

                // Get target IP as third argument
                //--------------------------------
                string targetIp = "192.168.1.15";
                if (args.Length > 3)
                {
                    if (!args[3].Equals("last", StringComparison.OrdinalIgnoreCase))
                    {
                        targetIp = args[3];
                    }
                }

                // Get target port as fourth argument
                //-----------------------------------
                int port = 69;
                if (args.Length > 4)
                {
                    if (!args[4].Equals("last", StringComparison.OrdinalIgnoreCase))
                    {
                        port = int.Parse(args[4]);
                    }
                }

                Console.WriteLine("Target: " + targetIp + ":" + port + " " + targetPathInfo);

                transfer.ToggleState(file, targetPath, targetIp, port);

                Console.WriteLine("Transfer started...");
                DateTime startTime = DateTime.UtcNow;

                int lastPercentage = 0;
                while (true)
                {
                    int percentage = 0;

                    if (!transfer.InProgress())
                    {
                        percentage = 100;
                    }
                    else
                    {
                        percentage = transfer.Percentage;
                    }

                    if (lastPercentage != percentage)
                    {
                        lastPercentage = percentage;
                        Console.Write("\rProgress: " + lastPercentage + "%");
                    }

                    if (lastPercentage >= 100)
                    {
                        break;
                    }

                    Thread.Sleep(100);
                }

                Console.WriteLine();

                if (transfer.ActiveError)
                {
                    Console.WriteLine("Error: " + transfer.LastError);
                    Console.WriteLine("Transfer failed!");
                }
                else
                {
                    TimeSpan transferTime = DateTime.UtcNow - startTime;
                    double speed = Math.Round(transfer.LastFileSize / 1024D / 1024D / transferTime.TotalSeconds, 2);
                    Console.WriteLine("Transfer finished in " + Math.Round(transferTime.TotalSeconds) + "s (" + speed + "MB/s)");
                }

                int exitCode = transfer.ActiveError ? 1 : 0;
                NativeMethods.FreeConsole();
                Environment.Exit(exitCode);
            }
            else
            {
                var handle = NativeMethods.GetConsoleWindow();
                NativeMethods.ShowWindow(handle, NativeMethods.SW_HIDE);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                FlexTftpForm form = new FlexTftpForm();

                Application.Run(form);

                int exitCode = form.ActiveError ? 1 : 0;
                Console.WriteLine("Exitcode: " + exitCode);
                Environment.Exit(exitCode);
            }
        }
    }
}
