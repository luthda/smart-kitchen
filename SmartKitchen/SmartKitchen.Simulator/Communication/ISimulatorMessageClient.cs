using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication
{
    public interface ISimulatorMessageClient<T> 
        : IObservable<ICommand<T>>, IDisposable
        where T : DeviceBase
    {
        Task InitAsync(T device);
        Task<ICommand<T>> CheckCommandsAsync(T device);
        Task SendNotificationAsync(INotification<T> notification);
    }
}
