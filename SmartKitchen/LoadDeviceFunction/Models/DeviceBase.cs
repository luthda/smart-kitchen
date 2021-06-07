using System;
using Microsoft.Azure.Cosmos.Table;

namespace DeviceFunctions.Models
{
    public class DeviceBase
    {
        public Guid Id { get; set; }
        public DeviceKey Key => new DeviceKey(GetType(), Id);
    }
}
