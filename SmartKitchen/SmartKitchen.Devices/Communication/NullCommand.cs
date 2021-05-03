namespace Hsr.CloudSolutions.SmartKitchen.Devices.Communication
{
    public class NullCommand 
        : ICommand<DeviceBase>
    {
        public string Type { get; set; } = Commands.None;
        public DeviceBase DeviceState { get; set; } = null;
        public bool HasDeviceInfo { get; set; } = false;
    }
}
