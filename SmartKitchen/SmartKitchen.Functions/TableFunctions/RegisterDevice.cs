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
    public static class RegisterDevice
    {
        private const string Route = "smartkitchen";

        [FunctionName("RegisterDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function register device");

            var cloudTable = await CloudTableUtil.GetCloudTable();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic device = JsonConvert.DeserializeObject<DeviceCloudDto>(requestBody);

            if (device == null)
            {
                return new NotFoundObjectResult(nameof(device));
            }

            await cloudTable.ExecuteAsync(TableOperation.InsertOrReplace(device));

            return new OkObjectResult(device);
        }
    }
}
