using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication
{
    public interface ISimulatorDataClient<in T> 
        : IDisposable
        where T : DeviceBase
    {
        Task InitAsync();
        Task RegisterDeviceAsync(T device);
        Task UnregisterDeviceAsync(T device);
    }
}
