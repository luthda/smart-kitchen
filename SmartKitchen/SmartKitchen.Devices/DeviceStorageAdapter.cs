using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class DeviceStorageAdapter : TableEntity
    {
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

        public string ObjectJson { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DeviceStorageAdapter"/>
        /// Seems unused, but is actually used by the azure storage stuff.
        /// </summary>
        public DeviceStorageAdapter()
        {

        }

        /// <summary>
        /// Creates a <see cref="DeviceStorageAdapter"/> from a <see cref="device"/>.
        /// </summary>
        /// <param name="device"></param>
        public DeviceStorageAdapter(DeviceBase device)
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