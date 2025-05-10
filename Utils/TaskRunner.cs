using System;
using System.Threading;
using System.Threading.Tasks;

namespace _4RTools.Utils
{
    public class TaskRunner
    {
        private readonly Func<int, int> _action;
        private CancellationTokenSource _cts;
        private Task _runningTask;

        public TaskRunner(Func<int, int> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Start()
        {
            if (_runningTask == null || _runningTask.IsCompleted)
            {
                _cts = new CancellationTokenSource();
                _runningTask = Task.Factory.StartNew(() =>
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        try
                        {
                            _action(0);
                            Thread.Sleep(5); // Maintain existing delay behavior
                        }
                        catch (OperationCanceledException)
                        {
                            break; // Expected during cancellation
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.Error("[TaskRunner Exception] Error executing task: " + ex.Message);
                        }
                    }
                }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public void Stop()
        {
            if (_cts != null)
            {
                try
                {
                    _cts.Cancel();
                    _runningTask.Wait(1000); // Allow graceful shutdown
                }
                catch (Exception ex)
                {
                    DebugLogger.Error("[TaskRunner Exception] Error stopping task: " + ex.Message);
                }
                finally
                {
                    _cts.Dispose();
                    _cts = null;
                    _runningTask = null;
                }
            }
        }

        public void Terminate()
        {
            Stop(); // Reuse Stop logic for termination
        }
    }
}