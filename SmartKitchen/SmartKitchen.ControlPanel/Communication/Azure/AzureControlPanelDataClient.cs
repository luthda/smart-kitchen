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
    /// <summary>
    /// This class is used to receive the registered devices.
    /// </summary>
    public class AzureControlPanelDataClient 
        : ClientBase
        , IControlPanelDataClient
    {
        private readonly IDialogService _dialogService; // Can display exception in a dialog.
        private readonly SmartKitchenConfiguration _config;
        private CloudStorageAccount _cloudStorageAccount;
        private const string TableName = "smartdevices";

        public AzureControlPanelDataClient(
            IDialogService dialogService,
            SmartKitchenConfiguration config)
        {
            _dialogService = dialogService;
            _config = config;
        }

        /// <summary>
        /// Used to establish the communication.
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
        /// Loads the registerd devices from the simulator.
        /// </summary>
        /// <returns>The list of all known devices.</returns>
        public async Task<IEnumerable<DeviceBase>> LoadDevicesAsync()
        {
            var cloudTable = await GetCloudTable();
            var tableQuery = new TableQuery<DeviceStorageAdapter>();

            return cloudTable.ExecuteQuery(tableQuery)
                .Select(deviceStorageAdapter => deviceStorageAdapter.ToDevice());
        }

        private async Task<CloudTable> GetCloudTable()
        {
            var tableClient = _cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var cloudTable = tableClient.GetTableReference(TableName);
            await cloudTable.CreateIfNotExistsAsync();

            return cloudTable;
        }

        /// <summary>
        /// Use this method to tear down any established connections.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
