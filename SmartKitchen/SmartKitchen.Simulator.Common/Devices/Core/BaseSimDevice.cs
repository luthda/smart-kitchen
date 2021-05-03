using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices.Core
{
    public abstract class BaseSimDevice<T>
        : ISimDevice<T>
        where T : DeviceBase, new()
    {
        protected BaseSimDevice(Guid id, string label, Point coordinate, Point size)
        {
            Id = id;
            Label = label;
            Coordinate = coordinate;
            Size = size;
        }

        public Guid Id { get; }

        public string Label { get; }

        public Point Coordinate { get; }
        public Point Size { get; }

        public T ToDevice()
        {
            var dto = new T { Id = Id };
            Prepare(dto);
            return dto;
        }

        protected abstract void Prepare(T dto);

        private IDeviceController<T> _controller;

        public async Task RegisterAsync(IDeviceController<T> controller)
        {
            if (_controller == controller)
            {
                return;
            }
            await UnregisterAsync();
            _controller = controller;
            _controller.CommandReceived += OnCommandReceived;
            await _controller.InitAsync(ToDevice());
        }

        public Task UnregisterAsync()
        {
            if (_controller == null)
            {
                return Task.CompletedTask;
            }
            _controller.CommandReceived -= OnCommandReceived;
            _controller.Dispose();

            return Task.CompletedTask;
        }

        private void OnCommandReceived(object sender, ICommand<T> command)
        {
            OnCommandReceived(command);
        }

        protected abstract void OnCommandReceived(ICommand<T> command);

        protected bool Send(INotification<T> notification)
        {
            if (notification == null || _controller?.IsInitialized != true)
            {
                return false;
            }
            _controller.Send(notification);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Send(new DeviceNotification<T>(ToDevice()));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            Task.Run(UnregisterAsync).Wait();
            OnDispose();
        }

        protected virtual void OnDispose() { }
    }
}
