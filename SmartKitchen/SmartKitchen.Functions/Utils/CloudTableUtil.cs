using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace SmartKitchen.Functions.Utils
{
    public static class CloudTableUtil
    {
        private const string TableName = "smartdevices";

        public static async Task<CloudTable> GetCloudTable()
        {
            var cloudStorageAccount =
                CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var cloudTable = tableClient.GetTableReference(TableName);

            if (!cloudTable.Exists())
            {
                await cloudTable.CreateAsync();
            }

            return cloudTable;
        }
    }
}