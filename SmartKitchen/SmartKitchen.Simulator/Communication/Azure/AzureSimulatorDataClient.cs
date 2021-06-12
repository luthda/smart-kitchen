using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.UI.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Newtonsoft.Json;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Communication.Azure
{
    public class AzureSimulatorDataClient<T>
        : ClientBase
            , ISimulatorDataClient<T>
        where T : DeviceBase
    {
        private readonly IDialogService _dialogService; // Can be used to display dialogs when exceptions occur.
        private readonly SmartKitchenConfiguration _config;
        private HttpClient _httpClient;

        public AzureSimulatorDataClient(
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

        public async Task RegisterDeviceAsync(T device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            var putRequest = CreateHttpRequest(HttpMethod.Put, "api/smartkitchen", device);

            var response = await _httpClient.SendAsync(putRequest);
            response.EnsureSuccessStatusCode();
        }

        public async Task UnregisterDeviceAsync(T device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));

            var deleteRequest = CreateHttpRequest(HttpMethod.Delete, "api/smartkitchen", device);

            var response = await _httpClient.SendAsync(deleteRequest);
            response.EnsureSuccessStatusCode();
        }

        private HttpRequestMessage CreateHttpRequest(HttpMethod method, string requestUri, T device)
        {
            var request = new HttpRequestMessage(method, requestUri);
            var serializeContent = new StringContent(JsonConvert.SerializeObject(new DeviceCloudDto(device)),
                Encoding.UTF8, "application/json");
            request.Content = serializeContent;

            return request;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}