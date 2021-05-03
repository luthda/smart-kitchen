using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Hsr.CloudSolutions.SmartKitchen.Service.Utils;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication.Grpc
{
    public class GrpcControlPanelMessageClient<T> 
        : ClientBase
        , IControlPanelMessageClient<T>
        where T : DeviceBase
    {
        private readonly IDialogService _dialogService;
        private readonly SmartKitchenConfiguration _config;

        private GrpcChannel _channel;
        private ControlPanelService.ControlPanelServiceClient _client;

        public GrpcControlPanelMessageClient(
            IDialogService dialogService,
            SmartKitchenConfiguration config)
        {
            _dialogService = dialogService;
            _config = config;
        }

        public async Task InitAsync(T device)
        {
            await Task.Run(() =>
            {
                _channel = GrpcChannel.ForAddress(_config.GrpcHostAddress);
                _client = new ControlPanelService.ControlPanelServiceClient(_channel);
            });
            IsInitialized = true;
        }

        public bool IsInitialized { get; private set; }
        
        public async Task<INotification<T>> CheckNotificationsAsync(T device)
        {
            if (device == null)
            {
                return NullNotification<T>.Empty;
            }
            try
            {
                PeekNotificationResponse response = await _client.PeekNotificationAsync(
                    device.Key.ToPeekNotificationRequest()
                );

                return response.ToNotification<T>();
            }
            catch (Exception ex)
            {
                LogException("Checking for notifications failed.", ex);
            }
            return NullNotification<T>.Empty;
        }

        public async Task SendCommandAsync(ICommand<T> command)
        {
            if (command == null)
            {
                return;
            }
            try
            {
                await _client.SendCommandAsync(command.ToSendCommandRequest());
            }
            catch (Exception ex)
            {
                LogException("Sending command failed.", ex);
                _dialogService.ShowException(ex);
            }
        }

        protected override void OnDispose()
        {
            try
            {
                _channel?.Dispose();
            }
            catch (Exception ex)
            {
                LogException("Closing Channel failed.", ex);
            }
        }
    }
}
