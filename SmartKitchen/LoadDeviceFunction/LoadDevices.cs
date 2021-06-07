using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeviceFunctions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace DeviceFunctions
{
    public static class LoadDevices
    {
        private const string Route = "smartkitchen";
        private const string TableName = "smartdevices";

        [FunctionName("LoadDevices")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var cloudStorageAccount = CloudStorageAccount.Parse("AzureWebJobsStorage");
            var tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var cloudTable = tableClient.GetTableReference(TableName);
            if (!cloudTable.Exists())
            {
                await cloudTable.CreateAsync();
            }

            var tableQuery = new TableQuery<DeviceCloudDto>();

            var segment = await cloudTable.ExecuteQuerySegmentedAsync(tableQuery, null);

            return new OkObjectResult(segment.Select(device => device.ToDevice()));
        }
    }
}
