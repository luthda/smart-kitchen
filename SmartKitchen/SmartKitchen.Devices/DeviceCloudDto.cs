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
            JsonObject = JsonConvert.SerializeObject(device, _jsonSettings);
            RowKey = device.Id.ToString();
            PartitionKey = device.Key.ToString();
        }

        public DeviceBase ToDevice()
        {
            return JsonConvert.DeserializeObject<DeviceBase>(JsonObject, _jsonSettings);
        }
    }
}