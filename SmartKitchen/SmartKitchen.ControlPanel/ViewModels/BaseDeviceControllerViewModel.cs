using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.UI.ViewModels;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public abstract class BaseDeviceControllerViewModel<T> : BaseViewModel, IDeviceControllerViewModel<T>, IObserver<INotification<T>>
        where T : DeviceBase, new()
    {
        private readonly IControlPanelMessageClient<T> _client;
        private readonly Func<DeviceBase, T> _cast;

        protected BaseDeviceControllerViewModel(IControlPanelMessageClient<T> client, Func<DeviceBase, T> cast)
        {
            _client = client;
            _cast = cast;

            _client.Subscribe(this);
        }

        protected T Cast(DeviceBase device)
        {
            return _cast(device);
        }

        protected T Device { get; private set; }

        private DeviceKey _key;
        protected DeviceKey Key
        {
            get => _key;
            set
            {
                _key = value;
                Id = _key.Id;
            }
        }

        private Guid _id;
        public Guid Id
        {
            get => _id;
            private set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public bool IsControllerFor(DeviceBase device)
        {
            if (device == null || Key == null)
            {
                return false;
            }
            return Key.Equals(device.Key);
        }

        Task IDeviceControllerViewModel.InitAsync(DeviceBase config)
        {
            return InitAsync(Cast(config));
        }

        public async Task InitAsync(T config)
        {
            Device = config;
            Key = config?.Key;
            Configure(config);
            await _client.InitAsync(Device);
        }

        protected abstract void Configure(T config);

        protected async void SendCommand(string command)
        {
            if (!_client.IsInitialized)
            {
                return;
            }
            var deviceCommand = new DeviceCommand<T>(ToDevice(), command);
            await _client.SendCommandAsync(deviceCommand);
        }


        public void Update(T update)
        {
            if (!IsControllerFor(update))
            {
                throw new InvalidOperationException("This controller is not responsible for the update.");
            }
            OnUpdate(update);
        }

        protected abstract void OnUpdate(T update);

        protected T ToDevice()
        {
            var dto = new T();
            dto.Id = Id;
            Prepare(dto);
            return dto;
        }

        protected abstract void Prepare(T dto);

        protected override void OnDispose()
        {
            _client.Dispose();
            base.OnDispose();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(INotification<T> notification)
        {
            if (notification is NullNotification<T> || !notification.HasDeviceInfo)
            {
                return;
            }
            Update(notification.DeviceState);
        }
    }
}
