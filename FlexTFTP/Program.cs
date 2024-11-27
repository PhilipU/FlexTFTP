using System;
using System.Windows.Forms;

namespace FlexTFTP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
