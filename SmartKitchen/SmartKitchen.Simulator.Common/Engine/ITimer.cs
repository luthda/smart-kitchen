using System;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Engine
{
    public interface ITimer 
        : IDisposable
    {
        event EventHandler<TimerEventArgs> Tick;
        void Start();
        void Stop();
        bool IsRunning { get; }
        void Reset(TimeSpan interval);
        TimeSpan Interval { get; }
    }
}
