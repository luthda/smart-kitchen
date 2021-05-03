using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using Hsr.CloudSolutions.SmartKitchen.ControlPanel.ViewModels;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI.ViewModels;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IServiceProvider _services;
        private readonly IControlPanelDataClient _client;
        private readonly DispatcherTimer _devicesUpdateTimer;
        private static readonly TimeSpan UpdateInterval = TimeSpan.FromSeconds(1);
        private readonly ObservableCollection<IDeviceControllerViewModel> _controllerViewModels = new ObservableCollection<IDeviceControllerViewModel>();

        public MainWindowViewModel(IServiceProvider services, IControlPanelDataClient client)
        {
            _services = services;
            _client = client;

            _devicesUpdateTimer = new DispatcherTimer();
            _devicesUpdateTimer.Interval = UpdateInterval;
            _devicesUpdateTimer.Tick += OnRequestDeviceUpdate;
        }

        public async Task InitAsync()
        {
            await _client.InitAsync();
            _devicesUpdateTimer.Start();
        }

        private static readonly SemaphoreSlim SemaphoreDeviceUpdate = new SemaphoreSlim(1, 1);
        private async void OnRequestDeviceUpdate(object sender, EventArgs e)
        {
            var devices = (await _client.LoadDevicesAsync()).ToList();

            bool monitorEnter = await SemaphoreDeviceUpdate.WaitAsync(UpdateInterval);
            if (!monitorEnter)
            {
                return;
            }
            try
            {
                foreach (var device in devices)
                {
                    if (_controllerViewModels.Any(vm => vm.IsControllerFor(device)))
                    {
                        continue;
                    }

                    var controllerVm = await CreateControllerViewModelForAsync(device);
                    Add(controllerVm);
                }
                foreach (var controller in _controllerViewModels.ToList())
                {
                    if (devices.Any(d => controller.IsControllerFor(d)))
                    {
                        continue;
                    }
                    Remove(controller);
                    controller.Dispose();
                }
            }
            finally
            {
                SemaphoreDeviceUpdate.Release();
            }
        }

        private async Task<IDeviceControllerViewModel> CreateControllerViewModelForAsync(DeviceBase device)
        {
            var viewModelType = typeof(IDeviceControllerViewModel<>).MakeGenericType(device.GetType());
            var viewModel = (IDeviceControllerViewModel)_services.GetService(viewModelType);
            await viewModel.InitAsync(device);
            return viewModel;
        }

        private void Add(IDeviceControllerViewModel viewModel)
        {
            if (Application.Current.Dispatcher != null)
            {
                Application.Current.Dispatcher.Invoke(() => _controllerViewModels.Add(viewModel));
            }
        }

        private void Remove(IDeviceControllerViewModel viewModel)
        {
            if (Application.Current.Dispatcher != null)
            {
                Application.Current.Dispatcher.Invoke(() => _controllerViewModels.Remove(viewModel));
            }
        }

        public IEnumerable<IDeviceControllerViewModel> DeviceControllers => _controllerViewModels;

        protected override void OnDispose()
        {
            _devicesUpdateTimer.Stop();
            _client.Dispose();
            base.OnDispose();
        }
    }
}
