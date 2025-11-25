using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FlexTFTP
{
    class PingDevice
    {
        // Copied from: https://stackoverflow.com/questions/11800958/using-ping-in-c-sharp
        private static IPStatus PingHost(string nameOrAddress)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(nameOrAddress, 1000);
                    if (reply == null)
                    {
                        return IPStatus.Unknown;
                    }
                    return reply.Status;
                }
            }
            catch (Exception)
            {
                return IPStatus.Unknown;
            }
        }

        public static bool Ping(string targetIp, int timeout, int consecutive)
        {
            Utils.WriteLine("Try to reach target...");
            int timeoutLeft = timeout;
            int reachableCount = 0;
            int errorCount = 0;
            while (timeoutLeft > 0)
            {
                DateTime startTime = DateTime.Now;
                IPStatus status = PingHost(targetIp);
                TimeSpan timeSpan = DateTime.Now - startTime;

                if (status == IPStatus.Unknown)
                {
                    if (errorCount > 0)
                    {
                        Utils.WriteLine("(x) Error occurred while trying to reach target. Target IP/Hostname invalid?");
                        return false;

                    }

                    errorCount++;
                }
                else
                {
                    errorCount = 0;
                }

                int msTaken = (int)timeSpan.TotalMilliseconds;

                Utils.Write("\r");
                Utils.Write("                                                              ");
                Utils.Write("\r");
                if (status == IPStatus.Success)
                {
                    reachableCount++;
                    Utils.Write((timeout - timeoutLeft + 1) + ": " + status + " " + reachableCount + "/" + consecutive + " (" + msTaken + "ms)");

                    if (reachableCount >= consecutive)
                    {
                        break;
                    }
                }
                else
                {
                    Utils.Write((timeout - timeoutLeft + 1) + ": " + status + " (" + msTaken + "ms)");
                }

                timeoutLeft--;

                if (msTaken < 1000)
                {
                    Thread.Sleep(1000 - msTaken);
                }
            }
            Console.WriteLine();

            if (timeoutLeft == 0)
            {
                if (reachableCount > 0)
                {
                    Utils.WriteLine("(!) Target count not reached (" + reachableCount + "/" + consecutive + ") within " + timeout + " seconds");
                    return false;
                }

                Utils.WriteLine("(x) Target not reachable after " + timeout + " seconds");
                return false;
            }

            Utils.WriteLine("(+) Successfully reached target " + reachableCount + "/" + consecutive + " within " + (timeout - timeoutLeft) + " seconds!");
            return true;
        }
    }
}
