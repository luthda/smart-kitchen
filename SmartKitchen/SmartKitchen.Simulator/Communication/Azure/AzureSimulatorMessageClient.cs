using System;
using System.Collections.Generic;
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
        private string _subscriptionName;
        private readonly List<IObserver<ICommand<T>>> _observers = new List<IObserver<ICommand<T>>>();

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
            _subscriptionName = device.Key.ToString();
            _notificationTopicClient = new TopicClient(_config.ServicesBusConnectionString, _config.NotificationTopic);
            _commandSubscriptionClient =
                new SubscriptionClient(_config.ServicesBusConnectionString, _config.CommandTopic, _subscriptionName);
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
            if (device == null || _command == null) return NullCommand<T>.Empty;

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
                Label = $"notification_{_subscriptionName}"
            };
            await _notificationTopicClient.SendAsync(message);
        }

        private async Task InitializeSubscription()
        {
            _subscriptionManagementClient = new ManagementClient(_config.ServicesBusConnectionString);

            if (!await _subscriptionManagementClient.SubscriptionExistsAsync(_config.CommandTopic, _subscriptionName))
            {
                var rule = new RuleDescription("DeviceFilter", new SqlFilter($"sys.Label = 'command_{_subscriptionName}'"));
                var subscription = new SubscriptionDescription(_config.CommandTopic, _subscriptionName)
                {
                    DefaultMessageTimeToLive = new TimeSpan(1, 0, 0, 0),
                    MaxDeliveryCount = 100
                };
                await _subscriptionManagementClient.CreateSubscriptionAsync(subscription, rule);
            }
        }

        private void InitializeReceiver()
        {
            _commandSubscriptionClient.RegisterMessageHandler(async (message, cancelToken) =>
            {
                if (message.Label != null &&
                    message.ContentType != null &&
                    message.Label.Equals($"command_{_subscriptionName}", StringComparison.InvariantCultureIgnoreCase) &&
                    message.ContentType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                {
                    var body = message.Body;
                    _command = JsonConvert.DeserializeObject<DeviceCommand<T>>(Encoding.UTF8.GetString(body));

                    foreach (var observer in _observers)
                    {
                        observer.OnNext(_command);
                    }

                    await _commandSubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                }
                else
                {
                    await _commandSubscriptionClient.DeadLetterAsync(message.SystemProperties.LockToken,
                        "ProcessingError", "Don't know what to do");
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
            await _notificationTopicClient.CloseAsync();
            await _commandSubscriptionClient.CloseAsync();
            await _subscriptionManagementClient.CloseAsync();
            base.OnDispose();
        }

        public IDisposable Subscribe(IObserver<ICommand<T>> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }


        private class Unsubscriber : IDisposable
        {
            private IList<IObserver<ICommand<T>>> _observers;
            private IObserver<ICommand<T>> _observer;

            public Unsubscriber
                (IList<IObserver<ICommand<T>>> observers, IObserver<ICommand<T>> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}