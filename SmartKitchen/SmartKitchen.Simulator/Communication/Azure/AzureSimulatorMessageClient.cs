using System;
using System.Text;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

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
        private TopicClient _notificationTopicClient;
        private ManagementClient _subscriptionManagementClient;
        private SubscriptionClient _commandSubscriptionClient;
        private ICommand<T> _command;
        private const string CommandTopic = "commandtopic";
        private const string NotificationTopic = "notificationtopic";
        private string _subscriptionName;

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
        public async Task InitAsync(T device)
        {
            _subscriptionName = $"command_{device.Key}";
            _notificationTopicClient = new TopicClient(_config.ServicesBusConnectionString, NotificationTopic);
            _commandSubscriptionClient =
                new SubscriptionClient(_config.ServicesBusConnectionString, CommandTopic, _subscriptionName);
            await InitializeSubscription();
            InitializeReceiver();
        }

        /// <summary>
        /// Checks if a command should be executed.
        /// </summary>
        /// <param name="device">The device to check for commands.</param>
        /// <returns>A received command or NullCommandDto&lt;T&gt;</returns>
        public async Task<ICommand<T>> CheckCommandsAsync(T device)
        {
            if (device == null) return NullCommand<T>.Empty;

            return await Task.Run(() => _command);
        }

        /// <summary>
        /// Sends a notification to the control panel.
        /// </summary>
        /// <param name="notification">The notification to send.</param>
        public async Task SendNotificationAsync(INotification<T> notification)
        {
            if (notification == null) return;

            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notification)))
            {
                ContentType = "application/json",
                Label = "notification"
            };

            await _notificationTopicClient.SendAsync(message);
        }

        private async Task InitializeSubscription()
        {
            _subscriptionManagementClient = new ManagementClient(_config.ServicesBusConnectionString);

            if (!await _subscriptionManagementClient.SubscriptionExistsAsync(CommandTopic, _subscriptionName))
            {
                var subscription = new SubscriptionDescription(CommandTopic, _subscriptionName);
                await _subscriptionManagementClient.CreateSubscriptionAsync(subscription);
            }
        }

        private void InitializeReceiver()
        {
            _commandSubscriptionClient.RegisterMessageHandler(async (message, cancelToken) =>
            {
                if (message.Label != null &&
                    message.ContentType != null &&
                    message.Label.Equals("command", StringComparison.InvariantCultureIgnoreCase) &&
                    message.ContentType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                {
                    var body = message.Body;
                    _command = JsonConvert.DeserializeObject<DeviceCommand<T>>(Encoding.UTF8.GetString(body));
                    await _commandSubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                }
            }, new MessageHandlerOptions(LogMessageHandlerException)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1
            });
        }

        /// <summary>
        /// Use this method to tear down established connections.
        /// </summary>
        protected override async void OnDispose()
        {
            await _commandSubscriptionClient.CloseAsync();
            base.OnDispose();
        }
    }
}