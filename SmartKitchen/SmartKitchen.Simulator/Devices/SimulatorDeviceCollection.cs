using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public class SimulatorDeviceCollection 
        : ISimulatorDeviceCollection
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly ISimulatorDeviceFactory _factory;
        private readonly List<ISimulatorDevice> _devices = new List<ISimulatorDevice>();

        public SimulatorDeviceCollection(ISimulatorDeviceFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IEnumerable<ISimDevice> Devices => _devices.Select(d => d.Device).OfType<ISimDevice>();

        public void Add(ISimDevice device)
        {
            if (device == null)
            {
                return;
            }
            if (_devices.Any(d => d.Device == device))
            {
                return;
            }
            var vm = _factory.CreateViewModelFor(device);
            _devices.Add(vm);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<ISimulatorDevice> { vm }));
        }

        public void Remove(ISimDevice device)
        {
            if (device == null)
            {
                return;
            }
            var vm = _devices.SingleOrDefault(d => d.Device == device);
            if (vm == null)
            {
                return;
            }
            _devices.Remove(vm);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<ISimulatorDevice> { vm }));
        }

        public IEnumerator<ISimulatorDevice> GetEnumerator()
        {
            return _devices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
