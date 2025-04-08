using System;
using System.Threading;

namespace _4RTools.Utils
{
    public class _4RThread
    {
        private readonly Thread thread;
        private readonly ManualResetEventSlim suspendEvent = new ManualResetEventSlim(true); // Initially set

        public _4RThread(Func<int, int> toRun)
        {
            this.thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        suspendEvent.Wait(); // This will "pause" execution when Reset() is called
                        toRun(0);
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.Error("[4RThread Exception] Error while Executing Thread Method ==== " + ex.Message);
                    }
                    finally
                    {
                        Thread.Sleep(5);
                    }
                }
            });
            this.thread.SetApartmentState(ApartmentState.STA);
        }

        public static void Start(_4RThread _4RThread)
        {
            if (_4RThread != null)
            {
                _4RThread.suspendEvent.Set(); // Resume execution
                _4RThread.thread.Start();
            }
        }

        public static void Stop(_4RThread _4RThread)
        {
            if (_4RThread != null && _4RThread.thread.IsAlive)
            {
                try
                {
                    _4RThread.suspendEvent.Reset(); // This will "pause" the thread
                }
                catch (Exception ex)
                {
                    DebugLogger.Error("[4R Thread Exception] =========== We could not suspend current thread: " + ex);
                }
            }
        }
    }
}
