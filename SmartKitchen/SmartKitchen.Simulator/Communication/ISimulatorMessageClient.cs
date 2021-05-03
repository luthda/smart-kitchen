using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication
{
    public interface ISimulatorMessageClient<T> 
        : IDisposable
        where T : DeviceBase
    {
        Task InitAsync(T device);
        Task<ICommand<T>> CheckCommandsAsync(T device);
        Task SendNotificationAsync(INotification<T> notification);
    }
}
