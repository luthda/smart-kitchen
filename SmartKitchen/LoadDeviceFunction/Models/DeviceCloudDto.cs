using Microsoft.Azure.Cosmos.Table;

namespace DeviceFunctions.Models
{
    public class DeviceCloudDto : TableEntity
    {
        public string JsonObject { get; set; }
    }
}