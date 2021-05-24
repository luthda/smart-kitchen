using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Azure.Cosmos.Table;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication.Azure
{
    /// <summary>
    /// This class is used for registration and deregistration of devices.
    /// </summary>
    /// <typeparam name="T">The device this client is used for.</typeparam>
    public class AzureSimulatorDataClient<T>
        : ClientBase
            , ISimulatorDataClient<T>
        where T : DeviceBase
    {
        private readonly IDialogService _dialogService; // Can be used to display dialogs when exceptions occur.
        private readonly SmartKitchenConfiguration _config;
        private CloudStorageAccount _cloudStorageAccount;
        private const string TableName = "smartdevices";

        public AzureSimulatorDataClient(
            IDialogService dialogService,
            SmartKitchenConfiguration config)
        {
            _dialogService = dialogService;
            _config = config;
        }

        /// <summary>
        /// Establishes the connections used to talk to the Cloud.
        /// </summary>
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

        /// <summary>
        /// Registers a <paramref name="device"/> to be used with the control panel.
        /// </summary>
        /// <param name="device">The device to register.</param>
        public async Task RegisterDeviceAsync(T device)
        {
            var cloudTable = await GetCloudTable();
            await cloudTable.ExecuteAsync(TableOperation.InsertOrReplace(device as ITableEntity));
        }

        /// <summary>
        /// Deregisters a <paramref name="device"/> to no longer be used with the control panel.
        /// </summary>
        /// <param name="device">The device to deregister.</param>
        public async Task UnregisterDeviceAsync(T device)
        {
            var cloudTable = await GetCloudTable();
            await cloudTable.ExecuteAsync(TableOperation.Delete(device as ITableEntity));
        }

        private async Task<CloudTable> GetCloudTable()
        {
            var tableClient = _cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var cloudTable = tableClient.GetTableReference(TableName);
            if (!await cloudTable.ExistsAsync())
            {
                await cloudTable.CreateAsync();
            }

            return cloudTable;
        }

        /// <summary>
        /// Use this method to tear down any established connection.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}