using System;
using System.IO;
using System.Threading.Tasks;
using DeviceFunctions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeviceFunctions
{
    public static class RegisterDevice
    {
        private const string Route = "smartkitchen";
        private const string TableName = "smartdevices";

        [FunctionName("RegisterDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = Route)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function register device");

            var cloudStorageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var cloudTable = tableClient.GetTableReference(TableName);
            if (!cloudTable.Exists())
            {
                await cloudTable.CreateAsync();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic device = JsonConvert.DeserializeObject(requestBody);

            if (device == null)
            {
                return new NotFoundObjectResult(nameof(device));
            }

            await cloudTable.ExecuteAsync(TableOperation.InsertOrReplace(device));

            return new OkObjectResult(device);
        }
    }
}
