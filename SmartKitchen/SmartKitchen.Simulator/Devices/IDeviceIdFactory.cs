using System;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public interface IDeviceIdFactory
    {
        Guid CreateFridgeId();
        Guid CreateOvenId();
        Guid CreateStoveId();
    }
}
