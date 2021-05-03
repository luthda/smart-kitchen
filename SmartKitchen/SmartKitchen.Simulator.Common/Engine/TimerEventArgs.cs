using System;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Engine
{
    public class TimerEventArgs : EventArgs
    {
        public TimerEventArgs(TimeSpan timespan)
        {
            ElapsedTime = timespan;
        }

        public TimeSpan ElapsedTime { get; }
    }
}
