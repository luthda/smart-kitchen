using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface ISimulatorDevice
    {
        Point Coordinate { get; }
        Point Size { get; }

        object Device { get; }
    }
}
