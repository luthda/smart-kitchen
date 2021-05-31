using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
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
        private ManagementClient _subscriptionManagementClient;
        private SubscriptionClient _notificationSubscriptionClient;
        private INotification<T> _notification;
        private string _subscriptionName;
        private readonly IList<IObserver<INotification<T>>> _observers = new List<IObserver<INotification<T>>>();

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
            _subscriptionName = device.Key.ToString();
            _commandTopicClient = new TopicClient(_config.ServicesBusConnectionString, _config.CommandTopic);
            _notificationSubscriptionClient = new SubscriptionClient(_config.ServicesBusConnectionString,
                _config.NotificationTopic, _subscriptionName);
            await InitializeSubscription();
            InitializeReceiver();
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
            if (device == null || _notification == null) return NullNotification<T>.Empty;

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
                Label = $"command_{_subscriptionName}"
            };

            await _commandTopicClient.SendAsync(message);
        }

        private async Task InitializeSubscription()
        {
            _subscriptionManagementClient = new ManagementClient(_config.ServicesBusConnectionString);

            if (!await _subscriptionManagementClient.SubscriptionExistsAsync(_config.NotificationTopic,
                _subscriptionName))
            {
                var rule = new RuleDescription("DeviceFilter", new SqlFilter($"sys.Label = 'notification_{_subscriptionName}'"));
                var subscription = new SubscriptionDescription(_config.NotificationTopic, _subscriptionName)
                {
                    DefaultMessageTimeToLive = new TimeSpan(1, 0, 0, 0), MaxDeliveryCount = 100
                };
                await _subscriptionManagementClient.CreateSubscriptionAsync(subscription, rule);
            }
        }

        private void InitializeReceiver()
        {
            _notificationSubscriptionClient.RegisterMessageHandler(async (message, cancelToken) =>
            {
                if (message.Label != null &&
                    message.ContentType != null &&
                    message.Label.Equals($"notification_{_subscriptionName}", StringComparison.InvariantCultureIgnoreCase) &&
                    message.ContentType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                {
                    var body = message.Body;
                    _notification = JsonConvert.DeserializeObject<DeviceNotification<T>>(Encoding.UTF8.GetString(body));

                    foreach (var observer in _observers)
                    {
                        observer.OnNext(_notification);
                    }

                    await _notificationSubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                }
                else
                {
                    await _notificationSubscriptionClient.DeadLetterAsync(message.SystemProperties.LockToken,
                        "ProcessingError", "Don't know what to do");
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
            await _commandTopicClient.CloseAsync();
            await _notificationSubscriptionClient.CloseAsync();
            await _subscriptionManagementClient.CloseAsync();
            base.OnDispose();
        }

        public IDisposable Subscribe(IObserver<INotification<T>> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private IList<IObserver<INotification<T>>> _observers;
            private IObserver<INotification<T>> _observer;

            public Unsubscriber
                (IList<IObserver<INotification<T>>> observers, IObserver<INotification<T>> observer)
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