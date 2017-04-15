using System;
using System.Threading;
using System.Threading.Tasks;

namespace NextBus.Helpers
{
    internal delegate void TimerCallback(object state);

    internal sealed class Timer : CancellationTokenSource, IDisposable
    {
        public Timer(Action callback, int dueTime, int period)
        {
            Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
            {
                var tuple = (Tuple<Action, object>)s;

                while (true)
                {
                    if (IsCancellationRequested)
                        break;
                    Task.Run(() => tuple.Item1());
                    await Task.Delay(period);
                }

            }, Tuple.Create(callback), CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        public new void Dispose() { base.Cancel(); }
    }

}
