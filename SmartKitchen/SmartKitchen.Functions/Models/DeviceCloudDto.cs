using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace SmartKitchen.Functions.Models
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
    }
}