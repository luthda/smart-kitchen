using System;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Engine
{
    public interface ISimulation 
        : IDisposable
    {
        bool Executing { get; }
    }
}
