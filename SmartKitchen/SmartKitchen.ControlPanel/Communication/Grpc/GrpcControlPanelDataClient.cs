using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Hsr.CloudSolutions.SmartKitchen.Service.Utils;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication.Grpc
{
    /// <summary>
    /// This class is used to retrieve a full ist of devices.
    /// </summary>
    public class GrpcControlPanelDataClient 
        : ClientBase
        , IControlPanelDataClient
    {
        private readonly IDialogService _dialogService;
        private readonly SmartKitchenConfiguration _config;

        private GrpcChannel _channel;
        private ControlPanelService.ControlPanelServiceClient _client;

        public GrpcControlPanelDataClient(
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
                _client = new ControlPanelService.ControlPanelServiceClient(_channel);
            });
        }

        public async Task<IEnumerable<DeviceBase>> LoadDevicesAsync()
        {
            var devices = new List<DeviceBase>();
            try
            {
                RegisteredDevicesResponse response = await _client.GetRegisteredDevicesAsync(
                    new Empty()
                );
                devices.AddRange(response.Devices.ToEntities());
            }
            catch (Exception ex)
            {
                LogException("Loading devices failed.", ex);
            }
            return devices;
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
