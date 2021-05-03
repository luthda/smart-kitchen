using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI.ViewModels;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ISimulatorDeviceCollection _devices;
        private readonly IDeviceLoader _loader;

        public MainWindowViewModel(ISimulatorDeviceCollection devices, IDeviceLoader loader)
        {
            _devices = devices ?? throw new ArgumentNullException(nameof(devices));
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        public ISimulatorDevices Devices => _devices;

        public async Task InitAsync()
        {
            var devices = await _loader.LoadDevicesAsync();
            if (devices == null)
            {
                return;
            }
            foreach (var device in devices)
            {
                _devices.Add(device);
            }
            OnPropertyChanged(nameof(Devices));
        }

        public async Task UnloadAsync()
        {
            foreach (var device in _devices.Devices)
            {
                await device.UnregisterAsync();
            }
        }
    }
}