using System.Collections.Generic;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator
{
    public interface IDeviceLoader
    {
        Task<IEnumerable<ISimDevice>> LoadDevicesAsync();
    }
}
