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

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication.Grpc
{
    public class GrpcSimulatorMessageClient<T> 
        : ClientBase
        , ISimulatorMessageClient<T>
        where T : DeviceBase
    {
        private readonly IDialogService _dialogService;
        private readonly SmartKitchenConfiguration _config;

        private GrpcChannel _channel;
        private SimulatorService.SimulatorServiceClient _client;

        public GrpcSimulatorMessageClient(
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
                _client = new SimulatorService.SimulatorServiceClient(_channel);
            });
        }

        public async Task<ICommand<T>> CheckCommandsAsync(T device)
        {
            if (device == null)
            {
                return NullCommand<T>.Empty;
            }
            try
            {
                PeekCommandResponse response = await _client.PeekCommandAsync(
                    device.Key.ToPeekCommandRequest()
                );

                return response.ToCommand<T>();
            }
            catch (Exception ex)
            {
                LogException("Checking for commands failed.", ex);
            }
            return NullCommand<T>.Empty;
        }

        public async Task SendNotificationAsync(INotification<T> notification)
        {
            if (notification == null)
            {
                return;
            }
            try
            {
                await _client.SendNotificationAsync(notification.ToSendNotificationRequest());
            }
            catch (Exception ex)
            {
                LogException("Send notification failed.", ex);
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
