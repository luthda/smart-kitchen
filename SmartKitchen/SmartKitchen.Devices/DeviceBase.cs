using System;
using Microsoft.Azure.Cosmos.Table;

namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class DeviceBase : TableEntity
    {
        public Guid Id { get; set; }
        public DeviceKey Key => new DeviceKey(GetType(), Id);
    }
}
