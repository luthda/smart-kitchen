namespace Hsr.CloudSolutions.SmartKitchen.Devices.Communication
{
    public class DeviceCommand<T> 
        : ICommand<T> 
        where T : DeviceBase
    {
        public DeviceCommand(T device, string type)
        {
            Type = type ?? Commands.None;
            DeviceState = device;
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
