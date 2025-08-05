using FlexTFTP.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Net;
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
        private static string GetArgByIndex(string[] args, int index)
        {
            if(index >= args.Length)
            { 
                return ""; 
            }

            string arg = args[index];

            if(arg == null || arg.Equals("colors") || arg.Equals("wait") || arg.Equals("test"))
            {
                return "";
            }

            return arg;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            string[] args = Environment.GetCommandLineArgs();
            bool pingDevice = false;
            bool infinitTransferTest = false;

            foreach(var arg in args)
            {
                if(arg.Equals("colors"))
                {
                    Utils.ConsoleColors = true;
                }

                if(arg.Equals("wait"))
                {
                    pingDevice = true;
                }

                if(arg.Equals("test"))
                {
                    infinitTransferTest = true;
                }
            }

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
                string file = GetArgByIndex(args, 1);

                // Print info
                //-----------
                string appTitle = "FlexTFTP";
                appTitle += " " + Utils.CurrentVersionString;
#if DEBUG
                appTitle += " [DEBUG]";
#endif
                Utils.WriteLine("****************************************");
                Utils.WriteLine(appTitle + " (started via CLI)");
                Utils.WriteLine("****************************************");

                Utils.WriteLine("(i) File: " + file);

                Settings.Default.AutoForce = false;

                // Get path as second argument
                //----------------------------
                {
                    string mode = GetArgByIndex(args, 2);
                    if (mode.Equals("auto", StringComparison.OrdinalIgnoreCase) || mode.Equals("auto-force", StringComparison.OrdinalIgnoreCase))
                    {
                        string parsedPath = TargetPathParser.GetTargetPath(file);
                        if (parsedPath != null)
                        {
                            targetPath = parsedPath;
                            targetPathInfo = targetPath + " (" + mode + ")";
                        }
                        else
                        {
                            Utils.WriteLine("(x) Unable to determine auto path (" + mode + "). Exitcode 1");
                            NativeMethods.FreeConsole();
                            Environment.Exit(1);
                        }

                        if (mode.Equals("auto-force", StringComparison.OrdinalIgnoreCase))
                        {
                            targetPath += " -force";
                        }
                    }
                    else
                    {
                        Settings.Default.AutoPath = false;
                        Settings.Default.TypeDependendAutpPath = false; // Prevent reenabling auto path due to other checks
                        targetPath = mode;
                        targetPathInfo = mode;
                    }
                }

                // Get target IP as third argument
                //--------------------------------
                string targetIp = "192.168.1.15";
                if (args.Length > 3)
                {
                    string targetIpArg = GetArgByIndex(args, 3);
                    if (targetIpArg.Length > 0 && !targetIpArg.Equals("last", StringComparison.OrdinalIgnoreCase))
                    {
                        targetIp = targetIpArg;
                    }
                }

                IPAddress address;
                if (!IPAddress.TryParse(targetIp, out address))
                {
                    Utils.WriteLine("(x) Target IP is not a valid IPv4 (" + targetIp + "). Exitcode 1");
                    NativeMethods.FreeConsole();
                    Environment.Exit(1);
                }

                // Get target port as fourth argument
                //-----------------------------------
                int port = 69;
                if (args.Length > 4)
                {
                    string portArg = GetArgByIndex(args, 4);
                    if (portArg.Length > 0 && !portArg.Equals("last", StringComparison.OrdinalIgnoreCase))
                    {
                        port = int.Parse(portArg);
                    }
                }

                Utils.WriteLine("(i) Target: " + targetIp + ":" + port + " " + targetPathInfo);

                transfer.ToggleState(file, targetPath, targetIp, port);

                Utils.WriteLine("Transfer started at " + DateTime.Now.ToString("HH:mm:ss") + "...");
                DateTime startTime = DateTime.UtcNow;

                int lastPercentage = 0;
                bool anyProgress = false;
                DateTime lastProgressUpdate = DateTime.UtcNow;
                int testIteration = 1;
                while (true)
                {
                    int percentage = 0;

                    if(transfer.ActiveError)
                    {
                        break;
                    }

                    if (!transfer.InProgress())
                    {
                        percentage = 100;
                    }
                    else
                    {
                        percentage = transfer.Percentage;

                        if(infinitTransferTest && transfer.Percentage >= 60)
                        {
                            transfer.StopTransfer();
                            Thread.Sleep(100);
                            transfer.ToggleState(file, targetPath, targetIp, port);
                            testIteration++;
                        }
                    }

                    if (lastPercentage != percentage)
                    {
                        lastPercentage = percentage;

                        string iterationInfo = "";
                        if(infinitTransferTest)
                        {
                            iterationInfo = " (" + testIteration + ")";
                        }

                        Utils.Write("\r");
                        Utils.Write("                                                              ");
                        Utils.Write("\r");
                        if (lastPercentage < 100)
                        {
                            Utils.Write("(!) Progress" + iterationInfo + ": " + lastPercentage + "%");
                        }
                        else
                        {
                            Utils.Write("(+) Progress" + iterationInfo + ": " + lastPercentage + "%");
                        }
                        lastProgressUpdate = DateTime.UtcNow;
                        anyProgress = true;
                    }
                    else if(infinitTransferTest)
                    {
                        DateTime now = DateTime.UtcNow;
                        if((now - lastProgressUpdate).TotalSeconds > 2)
                        {
                            Console.WriteLine();
                            Utils.WriteLine("(x) Detected hanging transmission");
                            transfer.StopTransfer();
                            break;
                        }
                    }

                    if (lastPercentage >= 100)
                    {
                        break;
                    }
                    
                    Thread.Sleep(100);
                }

                TimeSpan transferTime = DateTime.UtcNow - startTime;

                if(anyProgress)
                {
                    Console.WriteLine();
                }

                if (transfer.ActiveError)
                {
                    Utils.WriteLine("(x) Error: " + transfer.LastError);
                    Utils.WriteLine("(x) Transfer failed after " + Math.Round(transferTime.TotalSeconds) + "s!");
                }
                else
                {
                    double speed = Math.Round(transfer.LastFileSize / 1024D / 1024D / transferTime.TotalSeconds, 2);
                    Utils.WriteLine("(+) Transfer finished in " + Math.Round(transferTime.TotalSeconds) + "s (" + speed + "MB/s)");

                    if(pingDevice)
                    {
                        Thread.Sleep(500);

                        try
                        {
                            PingDevice.Ping(targetIp, 60, 3);
                        }
                        catch(Exception e)
                        {
                            Utils.WriteLine("(x) Exception occurred while waiting for device");
                        }
                    }
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
