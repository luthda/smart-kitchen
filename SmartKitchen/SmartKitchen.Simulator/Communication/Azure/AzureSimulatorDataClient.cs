using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;

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
        public Task InitAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Registers a <paramref name="device"/> to be used with the control panel.
        /// </summary>
        /// <param name="device">The device to register.</param>
        public Task RegisterDeviceAsync(T device)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Deregisters a <paramref name="device"/> to no longer be used with the control panel.
        /// </summary>
        /// <param name="device">The device to deregister.</param>
        public Task UnregisterDeviceAsync(T device)
        {
            throw new System.NotImplementedException();
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
