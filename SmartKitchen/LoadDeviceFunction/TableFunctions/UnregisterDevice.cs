using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartKitchen.Functions.Models;
using SmartKitchen.Functions.Utils;

namespace SmartKitchen.Functions.TableFunctions
{
    public static class UnregisterDevice
    {
        private const string Route = "smartkitchen";

        [FunctionName("UnregisterDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route)]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function unregister device");

            var cloudTable = await CloudTableUtil.GetCloudTable();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic device = JsonConvert.DeserializeObject<DeviceCloudDto>(requestBody);

            if (device == null)
            {
                return new NotFoundObjectResult(nameof(device));
            }

            await cloudTable.ExecuteAsync(TableOperation.Delete(device));


            return new NoContentResult();
        }
    }
}