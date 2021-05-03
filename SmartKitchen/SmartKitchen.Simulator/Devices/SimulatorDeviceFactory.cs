using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;
using Hsr.CloudSolutions.SmartKitchen.Simulator.ViewModels;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public class SimulatorDeviceFactory 
        : ISimulatorDeviceFactory
    {
        public ISimulatorDevice CreateViewModelFor<T>(T device) where T : ISimDevice
        {
            return new SimulatorDeviceViewModel<T>(device);
        }
    }
}
