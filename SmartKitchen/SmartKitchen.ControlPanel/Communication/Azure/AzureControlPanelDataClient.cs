using System.Collections.Generic;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;

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
        public Task InitAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Loads the registerd devices from the simulator.
        /// </summary>
        /// <returns>The list of all known devices.</returns>
        public Task<IEnumerable<DeviceBase>> LoadDevicesAsync()
        {
            throw new System.NotImplementedException();
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
