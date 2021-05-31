using System;
using System.Text;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

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
        private TopicClient _commandTopicClient;
        private SubscriptionClient _notificationSubscriptionClient;
        private INotification<T> _notification;
        private const string CommandTopic = "commandtopic";
        private const string NotificationTopic = "notification";

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
        public async Task InitAsync(T device)
        {
            await Task.Run((() =>
            {
                _commandTopicClient = new TopicClient(_config.ServicesBusConnectionString, CommandTopic);
                _notificationSubscriptionClient = new SubscriptionClient(_config.ServicesBusConnectionString,
                    NotificationTopic, device.Key.ToString());
                InitializeReceiver(_notificationSubscriptionClient);
            }));
            IsInitialized = true;
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
        public async Task<INotification<T>> CheckNotificationsAsync(T device)
        {
            if (device == null) return NullNotification<T>.Empty;

            return await Task.Run(() => _notification);
        }

        /// <summary>
        /// Send a command to the simulator.
        /// </summary>
        /// <param name="command">Command to send</param>
        public async Task SendCommandAsync(ICommand<T> command)
        {
            if (command == null) return;

            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command)))
            {
                ContentType = "application/json",
                Label = $"command_{command.DeviceState.Key}",
            };

            await _commandTopicClient.SendAsync(message);
        }

        private void InitializeReceiver(SubscriptionClient receiverClient)
        {
            receiverClient.RegisterMessageHandler(async (message, cancelToken) =>
            {
                if (message.Label != null &&
                    message.ContentType != null &&
                    message.Label.Equals("notification", StringComparison.InvariantCultureIgnoreCase) &&
                    message.ContentType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                {
                    var body = message.Body;
                    _notification = JsonConvert.DeserializeObject<DeviceNotification<T>>(Encoding.UTF8.GetString(body));
                    await receiverClient.CompleteAsync(message.SystemProperties.LockToken);
                }
            }, new MessageHandlerOptions(LogMessageHandlerException)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1
            });
        }

        /// <summary>
        /// Use this method to tear down any established connections.
        /// </summary>
        protected override async void OnDispose()
        {
            await _notificationSubscriptionClient.CloseAsync();
            base.OnDispose();
        }
    }
}