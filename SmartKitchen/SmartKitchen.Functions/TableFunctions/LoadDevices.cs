using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SmartKitchen.Functions.Models;
using SmartKitchen.Functions.Utils;

namespace SmartKitchen.Functions.TableFunctions
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
            log.LogInformation("C# HTTP trigger function load devices.");

            var cloudTable = await CloudTableUtil.GetCloudTable();

            var tableQuery = new TableQuery<DeviceCloudDto>();
            var devices = cloudTable.ExecuteQuery(tableQuery);

            return new OkObjectResult(devices);
        }
    }
}
