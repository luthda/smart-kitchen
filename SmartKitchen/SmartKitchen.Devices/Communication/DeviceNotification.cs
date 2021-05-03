namespace Hsr.CloudSolutions.SmartKitchen.Devices.Communication
{
    public class DeviceNotification<T> 
        : INotification<T>
        where T : DeviceBase
    {
        public DeviceNotification(T device, string type = Notifications.Update)
        {
            DeviceState = device;
            Type = type;
        } 

        public T DeviceState { get; set; }
        public string Type { get; set; }
        public bool HasDeviceInfo => DeviceState != null;

        public override string ToString()
        {
            return $"DeviceState: {(HasDeviceInfo ? DeviceState.Key.ToString() : "???")} / Type: {Type} / Info: {DeviceState}";
        }
    }
}
