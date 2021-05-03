namespace Hsr.CloudSolutions.SmartKitchen.Devices.Communication
{
    public class NullCommand<T>
        : ICommand<T>
        where T : DeviceBase
    {
        public static readonly NullCommand<T> Empty = new NullCommand<T>();

        public string Type { get; set; } = Commands.None;
        public T DeviceState { get; set; } = null;
        public bool HasDeviceInfo { get; set; } = false;
    }
}
