using System;
using System.Threading;

namespace BruteGamingMacros.Core.Utils
{
    public class ThreadRunner
    {
        private readonly Thread thread;
        private readonly ManualResetEventSlim suspendEvent = new ManualResetEventSlim(true); // Initially set
        private volatile bool running = true;

        public ThreadRunner(Func<int, int> toRun)
        {
            this.thread = new Thread(() =>
            {
                while (running)
                {
                    try
                    {
                        suspendEvent.Wait(); // This will "pause" execution when Reset() is called
                        toRun(0);
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.Error("[ThreadRunner Exception] Error while executing thread method: " + ex.Message);
                    }
                    finally
                    {
                        Thread.Sleep(5);
                    }
                }
            });

            this.thread.IsBackground = true;
            this.thread.SetApartmentState(ApartmentState.STA);
        }

        public static void Start(ThreadRunner runner)
        {
            if (runner != null)
            {
                runner.suspendEvent.Set(); // Resume execution
                if (!runner.thread.IsAlive)
                {
                    runner.thread.Start();
                }
            }
        }

        public static void Stop(ThreadRunner runner)
        {
            if (runner != null && runner.thread.IsAlive)
            {
                try
                {
                    runner.suspendEvent.Reset(); // This will "pause" the thread
                }
                catch (Exception ex)
                {
                    DebugLogger.Error("[ThreadRunner Exception] Could not suspend thread: " + ex);
                }
            }
        }

        public void Terminate()
        {
            running = false;
            suspendEvent.Set(); // Unblock the wait if paused
        }
    }
}