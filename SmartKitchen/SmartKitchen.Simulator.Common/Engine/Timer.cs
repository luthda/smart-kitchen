using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Engine
{
    public class Timer 
        : ITimer
    {
        public event EventHandler<TimerEventArgs> Tick;

        public bool IsRunning { get; private set; }

        private TimeSpan _interval = TimeSpan.Zero;

        public TimeSpan Interval
        {
            get => _interval;
            private set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentException(nameof(Interval));
                }
                _interval = value;
            }
        }

        private Task _action;
        private CancellationTokenSource _cancellationHandler;
        

        public Timer() { }
        public Timer(TimeSpan interval)
        {
            Interval = interval;
        }

        public void Start()
        {
            if (IsRunning || Interval == TimeSpan.Zero)
            {
                return;
            }
            IsRunning = true;
            _cancellationHandler = new CancellationTokenSource();
            _action = Task.Run(async () =>
            {
                while (!_cancellationHandler.Token.IsCancellationRequested)
                {
                    await Task.Delay(Interval);
                    OnTick();
                }
            }, _cancellationHandler.Token);
        }

        public void Stop()
        {
            _cancellationHandler?.Cancel();
            IsRunning = false;
        }

        public void Reset(TimeSpan interval)
        {
            Interval = interval;
        }

        private void OnTick()
        {
            if (_cancellationHandler.IsCancellationRequested || !IsRunning)
            {
                return;
            }
            if (Tick == null)
            {
                return;
            }
            foreach (var handler in Tick.GetInvocationList().OfType<EventHandler<TimerEventArgs>>())
            {
                try
                {
                    handler.Invoke(this, new TimerEventArgs(Interval));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Tick handler resulted in exception:\r\n\t{ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            Stop();
            try
            {
                _action?.Wait();
            }
            catch
            {
                // ignored
            }
        }
    }
}
