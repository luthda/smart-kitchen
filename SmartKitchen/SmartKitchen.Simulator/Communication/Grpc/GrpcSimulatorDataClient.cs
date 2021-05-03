using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Hsr.CloudSolutions.SmartKitchen.Service.Utils;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication.Grpc
{
    public class GrpcSimulatorDataClient<T>
        : ClientBase
        , ISimulatorDataClient<T>
        where T : DeviceBase
    {
        private readonly IDialogService _dialogService;
        private readonly SmartKitchenConfiguration _config;

        private GrpcChannel _channel;
        private SimulatorService.SimulatorServiceClient _client;

        public GrpcSimulatorDataClient(
            IDialogService dialogService,
            SmartKitchenConfiguration config)
        {
            _dialogService = dialogService;
            _config = config;
        }

        public async Task InitAsync()
        {
            await Task.Run(() =>
            {
                _channel = GrpcChannel.ForAddress(_config.GrpcHostAddress);
                _client = new SimulatorService.SimulatorServiceClient(_channel);
            });
        }

        public async Task RegisterDeviceAsync(T device)
        {
            if (device == null)
            {
                return;
            }
            try
            {
                await _client.RegisterDeviceAsync(device.ToRegisterDeviceRequest());
            }
            catch (Exception ex)
            {
                LogException("Register device failed.", ex);
            }
        }

        public async Task UnregisterDeviceAsync(T device)
        {
            if (device == null)
            {
                return;
            }
            try
            {
                await _client.UnregisterDeviceAsync(device.ToUnregisterDeviceRequest());
            }
            catch (Exception ex)
            {
                LogException("Unregister device failed.", ex);
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
