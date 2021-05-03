namespace Hsr.CloudSolutions.SmartKitchen.Devices.Communication
{
    public interface ICommand<out T>
        where T : DeviceBase
    {
        T DeviceState { get; }
        string Type { get; }
        bool HasDeviceInfo { get; }
    }
}
