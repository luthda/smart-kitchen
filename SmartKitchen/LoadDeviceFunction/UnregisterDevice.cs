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
    public static class UnregisterDevice
    {
        private const string Route = "smartkitchen";
        private const string TableName = "smartdevices";

        [FunctionName("UnregisterDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function unregister device");

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

            await cloudTable.ExecuteAsync(TableOperation.Delete(device));


            return new NoContentResult();
        }
    }
}
