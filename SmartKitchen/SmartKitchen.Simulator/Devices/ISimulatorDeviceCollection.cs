using System.Collections.Generic;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface ISimulatorDeviceCollection 
        : ISimulatorDevices
    {
        void Add(ISimDevice device);
        void Remove(ISimDevice device);

        IEnumerable<ISimDevice> Devices { get; } 
    }
}
