using System.Collections.Generic;
using System.Collections.Specialized;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface ISimulatorDevices 
        : IEnumerable<ISimulatorDevice>
        , INotifyCollectionChanged
    {
        
    }
}
