using System;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Devices;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.ViewModels
{
    public class SimulatorDeviceViewModel<T>
        : ISimulatorDevice
        where T : ISimDevice
    {
        private readonly T _device;

        public SimulatorDeviceViewModel(T device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }
            _device = device;
        }

        public Point Coordinate => _device.Coordinate;
        public Point Size => _device.Size;
        public object Device => _device;
    }
}