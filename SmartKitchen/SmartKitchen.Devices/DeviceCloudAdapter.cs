using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class DeviceCloudAdapter : TableEntity
    {
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

        public string ObjectJson { get; set; }

        public DeviceCloudAdapter()
        {

        }

        public DeviceCloudAdapter(DeviceBase device)
        {
            ObjectJson = JsonConvert.SerializeObject(device, _jsonSettings);
            RowKey = device.Id.ToString();
            PartitionKey = device.Key.ToString();
        }

        public DeviceBase ToDevice()
        {
            return JsonConvert.DeserializeObject<DeviceBase>(ObjectJson, _jsonSettings);
        }
    }
}