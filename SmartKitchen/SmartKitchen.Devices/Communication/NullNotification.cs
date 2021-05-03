namespace Hsr.CloudSolutions.SmartKitchen.Devices.Communication
{
    public class NullNotification
        : INotification<DeviceBase>
    {
        public DeviceBase DeviceState { get; set; } = null;
        public bool HasDeviceInfo { get; set; } = false;
        public string Type { get; set; } = Notifications.None;
    }
}
