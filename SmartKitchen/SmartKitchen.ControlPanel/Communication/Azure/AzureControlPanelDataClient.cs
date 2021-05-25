using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Azure.Cosmos.Table;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication.Azure
{
    public class AzureControlPanelDataClient
        : ClientBase
            , IControlPanelDataClient
    {
        private readonly IDialogService _dialogService; // Can display exception in a dialog.
        private readonly SmartKitchenConfiguration _config;
        private CloudStorageAccount _cloudStorageAccount;

        public AzureControlPanelDataClient(
            IDialogService dialogService,
            SmartKitchenConfiguration config)
        {
            _dialogService = dialogService;
            _config = config;
        }

        public async Task InitAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    _cloudStorageAccount = CloudStorageAccount.Parse(_config.StorageConnectionString);
                }
                catch (Exception)
                {
                    Console.WriteLine(
                        "Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                    throw;
                }
            });
        }

        public async Task<IEnumerable<DeviceBase>> LoadDevicesAsync()
        {
            var cloudTable = await GetCloudTable();
            var tableQuery = new TableQuery<DeviceCloudDto>();

            return cloudTable.ExecuteQuery(tableQuery)
                .Select(deviceStorageAdapter => deviceStorageAdapter.ToDevice());
        }

        private async Task<CloudTable> GetCloudTable()
        {
            var tableClient = _cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var cloudTable = tableClient.GetTableReference(_config.CloudTableName);
            await cloudTable.CreateIfNotExistsAsync();

            return cloudTable;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}