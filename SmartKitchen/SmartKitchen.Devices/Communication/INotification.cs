namespace Hsr.CloudSolutions.SmartKitchen.Devices.Communication
{
    public interface INotification<out T>
        where T : DeviceBase
    {
        T DeviceState { get; }
        string Type { get; }
        bool HasDeviceInfo { get; }
    }
}
