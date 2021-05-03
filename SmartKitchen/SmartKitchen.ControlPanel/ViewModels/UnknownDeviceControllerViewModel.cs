using Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using Hsr.CloudSolutions.SmartKitchen.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public class UnknownDeviceControllerViewModel
        : BaseDeviceControllerViewModel<DeviceBase>
    {
        public UnknownDeviceControllerViewModel(IControlPanelMessageClient<DeviceBase> client) 
            : base(client, d => d) { }

        protected override void Configure(DeviceBase config) { }
        protected override void OnUpdate(DeviceBase update) { }
        protected override void Prepare(DeviceBase device) { }
    }

    public class UnknownDeviceControllerViewModel<TGarbage> : UnknownDeviceControllerViewModel
        where TGarbage : DeviceBase
    {
        public UnknownDeviceControllerViewModel(IControlPanelMessageClient<DeviceBase> client) 
            : base(client) { }
    }
}
