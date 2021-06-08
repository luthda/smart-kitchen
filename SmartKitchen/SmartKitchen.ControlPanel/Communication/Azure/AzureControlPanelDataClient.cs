using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Newtonsoft.Json;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication.Azure
{
    public class AzureControlPanelDataClient
        : ClientBase
            , IControlPanelDataClient
    {
        private readonly IDialogService _dialogService; // Can display exception in a dialog.
        private readonly SmartKitchenConfiguration _config;
        private HttpClient _httpClient;

        public AzureControlPanelDataClient(
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
                _httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(_config.DeviceFunctionsString)
                };
            });
        }

        public async Task<IEnumerable<DeviceBase>> LoadDevicesAsync()
        {
            var getRequest = new HttpRequestMessage(HttpMethod.Get, "api/smartkitchen");

            var response = await _httpClient.SendAsync(getRequest);

            return await DeserializeResponseContent(response);
        }

        private async Task<IEnumerable<DeviceBase>> DeserializeResponseContent(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<DeviceCloudDto>>(stringContent)
                .Select(device => device.FromDto());
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}