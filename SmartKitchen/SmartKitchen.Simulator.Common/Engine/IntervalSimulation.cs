using System;
using System.Diagnostics;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Engine
{
    public class IntervalSimulation<T> 
        : ISimulation
    {
        private readonly Action<T> _onChange;
        private readonly Func<T> _change;
        private readonly ITimer _timer;
        private readonly Func<bool> _isCompleted;
        private readonly Action _onStarted;
        private readonly Action _onCompleted;

        public IntervalSimulation(
            Action<T> onChange, 
            Func<T> change, 
            TimeSpan interval, 
            Func<bool> isCompleted = null, 
            Action onStarted = null, 
            Action onCompleted = null)
        {
            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentException(nameof(interval));
            }
            _onChange = onChange ?? throw new ArgumentNullException(nameof(onChange));
            _change = change ?? throw new ArgumentNullException(nameof(change));
            _isCompleted = isCompleted ?? (() => false);
            _onStarted = onStarted ?? (() => { });
            _onCompleted = onCompleted ?? (() => { });
            _timer = new Timer(interval);
            _timer.Tick += (s, a) =>
            {
                try
                {
                    _onChange(_change());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Simulation execution resulted in an exception!\r\n\t{ex.GetType()}: {ex.Message}");
                }
                finally
                {
                    if (_isCompleted())
                    {
                        _timer.Stop();
                        Executing = false;
                        _onCompleted();
                    }
                }
            };
            _timer.Start();
            Executing = true;
            _onStarted();
        }

        public bool Executing { get; private set; }

        public void Dispose()
        {
            _timer.Dispose();
            Executing = false;
        }
    }
}
