using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Azure.Cosmos.Table;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication.Azure
{
    public class AzureSimulatorDataClient<T>
        : ClientBase
            , ISimulatorDataClient<T>
        where T : DeviceBase
    {
        private readonly IDialogService _dialogService; // Can be used to display dialogs when exceptions occur.
        private readonly SmartKitchenConfiguration _config;
        private CloudStorageAccount _cloudStorageAccount;

        public AzureSimulatorDataClient(
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

        public async Task RegisterDeviceAsync(T device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            var cloudTable = await GetCloudTable();
            await cloudTable.ExecuteAsync(TableOperation.InsertOrReplace(new DeviceCloudDto(device)));
        }

        public async Task UnregisterDeviceAsync(T device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            var cloudTable = await GetCloudTable();
            await cloudTable.ExecuteAsync(TableOperation.Delete(new DeviceCloudDto(device)));
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