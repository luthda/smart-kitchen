using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication
{
    public interface IControlPanelMessageClient<T> 
        : IObservable<INotification<T>>, IDisposable 
        where T : DeviceBase
    {
        Task InitAsync(T device);
        bool IsInitialized { get; }
        Task<INotification<T>> CheckNotificationsAsync(T device);
        Task SendCommandAsync(ICommand<T> command);
    }
}
