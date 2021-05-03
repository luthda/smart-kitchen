using System;

namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class DeviceBase 
    {
        public Guid Id { get; set; }
        public DeviceKey Key => new DeviceKey(GetType(), Id);
    }
}
