using System;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Engine
{
    public class NullSimulation 
        : ISimulation
    {
        public NullSimulation(Action action = null)
        {
            action?.Invoke();
        }

        public bool Executing => false;

        public void Dispose() { }
    }
}
