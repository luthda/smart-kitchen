using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface ISimulatorDeviceFactory
    {
        ISimulatorDevice CreateViewModelFor<T>(T device) where T : ISimDevice;
    }
}
