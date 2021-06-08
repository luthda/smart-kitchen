using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Communication;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public class SimulatorDeviceController<T> 
        : IObserver<ICommand<T>>, IDeviceController<T>
        where T : DeviceBase
    {
        public event EventHandler<ICommand<T>> CommandReceived;

        private readonly ISimulatorDataClient<T> _dataClient;
        private readonly ISimulatorMessageClient<T> _messageClient;

        public SimulatorDeviceController(
            ISimulatorDataClient<T> dataClient, 
            ISimulatorMessageClient<T> messageClient)
        {
            _dataClient = dataClient;
            _messageClient = messageClient;
            
        }

        private T _dto;

        public async Task InitAsync(T device)
        {
            _dto = device;
            await _dataClient.InitAsync();
            await _messageClient.InitAsync(device);

            await _dataClient.RegisterDeviceAsync(_dto);

            _messageClient.Subscribe(this);

            IsInitialized = true;
        }

        public bool IsInitialized { get; private set; } = false;

        public async void Send(INotification<T> notification)
        {
            await _messageClient.SendNotificationAsync(notification);
        }

        public void Dispose()
        {
            if (_dto != null)
            {
                Task.Run(() => _dataClient.UnregisterDeviceAsync(_dto)).Wait();
            }

            _messageClient.Dispose();
            _dataClient.Dispose();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ICommand<T> command)
        {
            if (command is NullCommand<T>)
            {
                return;
            }
            CommandReceived?.Invoke(this, command);
        }
    }
}
