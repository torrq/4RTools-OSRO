using System;
using System.Threading;

namespace _ORTools.Utils
{
    public class ThreadRunner
    {
        private readonly Thread thread;
        private readonly ManualResetEventSlim suspendEvent = new ManualResetEventSlim(true); // Initially set
        private volatile bool running = true;

        public ThreadRunner(Func<int, int> toRun, string name = "Unnamed ThreadRunner")
        {
            this.thread = new Thread(() =>
            {
                while (running)
                {
                    try
                    {
                        suspendEvent.Wait(); // This will "pause" execution when Reset() is called
                        int result = toRun(0);
                        if (result < 0)
                        {
                            DebugLogger.Debug($"[ThreadRunner] '{Thread.CurrentThread.Name ?? "Unnamed"}' requested termination (code {result})");
                            running = false;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.Error($"[ThreadRunner Exception on '{Thread.CurrentThread.Name}'] Error while executing thread method: {ex.Message}");
                    }
                    finally
                    {
                        Thread.Sleep(5);
                    }
                }

            });

            this.thread.Name = name;
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