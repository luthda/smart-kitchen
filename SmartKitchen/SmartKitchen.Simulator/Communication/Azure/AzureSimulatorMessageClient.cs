using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication.Azure
{
    /// <summary>
    /// This class is used to receive commands and send notifications.
    /// </summary>
    /// <typeparam name="T">The device this client is used for.</typeparam>
    public class AzureSimulatorMessageClient<T> 
        : ClientBase
        , ISimulatorMessageClient<T>
        where T : DeviceBase
    {
        private readonly IDialogService _dialogService; // Can be used to display dialogs when exceptions occur.
        private readonly SmartKitchenConfiguration _config;

        public AzureSimulatorMessageClient(
            IDialogService dialogService,
            SmartKitchenConfiguration config)
        {
            _dialogService = dialogService;
            _config = config;
        }

        /// <summary>
        /// Establishes the connections used to talk to the Cloud.
        /// </summary>
        /// <param name="device">The device this client is used for.</param>
        public Task InitAsync(T device)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Checks if a command should be executed.
        /// </summary>
        /// <param name="device">The device to check for commands.</param>
        /// <returns>A received command or NullCommandDto&lt;T&gt;</returns>
        public Task<ICommand<T>> CheckCommandsAsync(T device)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Sends a notification to the control panel.
        /// </summary>
        /// <param name="notification">The notification to send.</param>
        public Task SendNotificationAsync(INotification<T> notification)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Use this method to tear down established connections.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
