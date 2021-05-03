using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public interface IDeviceControllerViewModel<in T> 
        : IDeviceControllerViewModel 
        where T : DeviceBase
    {
        Guid Id { get; }

        Task InitAsync(T config);
        void Update(T update);
    }
}
