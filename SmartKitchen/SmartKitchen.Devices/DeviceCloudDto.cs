using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class DeviceCloudDto : TableEntity
    {
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public string JsonObject { get; set; }

        public DeviceCloudDto()
        {
        }

        public DeviceCloudDto(DeviceBase device)
        {
            JsonObject = device.ToString();
            RowKey = device.Id.ToString();
            PartitionKey = device.Key.ToString();
        }
        public DeviceBase ToDevice()
        {
            var device = JsonConvert.DeserializeObject<DeviceBase>(JsonObject);
            return device;
        }
    }
}