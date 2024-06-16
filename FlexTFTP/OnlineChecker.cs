using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace FlexTFTP
{
    class OnlineChecker
    {
        readonly Action<IPAddress, bool> _callback;
        IPAddress _address;
        Thread _cyclicThread;
        int _cyclicIntervalMs;

        public OnlineChecker(IPAddress address, Action<IPAddress, bool> callback)
        {
            _address = address;
            _callback = callback;
        }

        public void ChangeAddress(IPAddress address)
        {
            _address = address;
        }

        public void SetCyclicInterval(int intervalMs)
        {
            if(0 == intervalMs)
            {
                return;
            }
            _cyclicIntervalMs = intervalMs;
        }

        public void StartCyclicCheck(int intervalMs)
        {
            if (0 == intervalMs)
            {
                return;
            }

            StopCyclicCheck();

            _cyclicIntervalMs = intervalMs;

            if (_cyclicThread == null || _cyclicThread.ThreadState == ThreadState.Aborted ||
                _cyclicThread.ThreadState == ThreadState.AbortRequested)
            { 
                _cyclicThread = new Thread(AsyncOnlineCyclicCheck);
                _cyclicThread.Start();
            }
        }

        public void StopCyclicCheck()
        {
            if(_cyclicThread != null)
            {
                _cyclicThread.Abort();
                _cyclicThread = null;
            }
        }

        public void StartCheckOnce()
        {
            Thread thread = new Thread(AsyncOnlineCheck);
            thread.Start();
        }

        private void AsyncOnlineCyclicCheck()
        {
            while(true)
            {
                AsyncOnlineCheck();

                Thread.Sleep(_cyclicIntervalMs);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void AsyncOnlineCheck()
        {
            if(_address == null)
            {
                return;
            }

            //Utils.ClearArpTable(); // Clearing ARP cache each time we ping is too heavy...

            try
            {
                Ping p = new Ping();
                PingReply reply = p.Send(_address);

                _callback(_address, reply != null && reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
