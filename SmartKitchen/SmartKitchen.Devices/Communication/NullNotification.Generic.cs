namespace Hsr.CloudSolutions.SmartKitchen.Devices.Communication
{
    public class NullNotification<T> 
        : INotification<T>
        where T : DeviceBase
    {
        public static readonly NullNotification<T> Empty = new NullNotification<T>();

        public T DeviceState { get; set; } = null;
        public bool HasDeviceInfo { get; set; } = false;
        public string Type { get; set; } = Notifications.None;
    }
}
