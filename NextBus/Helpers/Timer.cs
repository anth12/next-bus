using System;
using System.Threading;
using System.Threading.Tasks;

namespace NextBus.Helpers
{
    internal delegate void TimerCallback(object state);

    internal sealed class Timer : CancellationTokenSource, IDisposable
    {
        private readonly SynchronizationContext Context;

        internal static Timer StopDetails { get; set; }

        public Timer(Action callback, int initialDelay, int executionPeriod)
        {
            Context = SynchronizationContext.Current;

            Task.Delay(initialDelay, Token).ContinueWith(async (t, s) =>
            {
                var action = (Action)s;

                while (true)
                {
                    if (IsCancellationRequested)
                        break;

                    Context.Send(state =>
                    {
                        action.Invoke();
                    }, null);
                    
                    await Task.Delay(executionPeriod);
                }

            }, callback, CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        public new void Dispose() { base.Cancel(); }
    }

}
