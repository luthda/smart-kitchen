using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication.Azure
{
    /// <summary>
    /// This class is used to send commands to devices and for receiving their notifications.
    /// </summary>
    /// <typeparam name="T">The type of DeviceBase this client is used for.</typeparam>
    public class AzureControlPanelMessageClient<T>
        : ClientBase
            , IControlPanelMessageClient<T>
        where T : DeviceBase
    {
        private readonly IDialogService _dialogService; // Can display exception in a dialog.
        private readonly SmartKitchenConfiguration _config;

        public AzureControlPanelMessageClient(
            IDialogService dialogService,
            SmartKitchenConfiguration config)
        {
            _dialogService = dialogService;
            _config = config;
        }

        /// <summary>
        /// Used to establish the communication.
        /// </summary>
        /// <param name="device">The device this client is responsible for.</param>
        public Task InitAsync(T device)
        {
            IsInitialized = true;
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// True if InitAsync was called and client is initialized.
        /// </summary>
        public bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Checks if a notification for the <paramref name="device" /> is pending.
        /// </summary>
        /// <param name="device">The device to check notifications for.</param>
        /// <returns>A received notification or NullNotification&lt;T&gt;</returns>
        public Task<INotification<T>> CheckNotificationsAsync(T device)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Send a command to the simulator.
        /// </summary>
        /// <param name="command">Command to send</param>
        public Task SendCommandAsync(ICommand<T> command)
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