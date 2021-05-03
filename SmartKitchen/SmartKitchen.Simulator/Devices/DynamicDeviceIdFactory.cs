using System;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public class DynamicDeviceIdFactory 
        : IDeviceIdFactory
    {
        public Guid CreateFridgeId()
        {
            return Guid.NewGuid();
        }

        public Guid CreateOvenId()
        {
            return Guid.NewGuid();
        }

        public Guid CreateStoveId()
        {
            return Guid.NewGuid();
        }
    }
}
