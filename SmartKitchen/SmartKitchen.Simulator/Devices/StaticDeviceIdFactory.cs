using System;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public class StaticDeviceIdFactory 
        : IDeviceIdFactory
    {
        private readonly Guid _fridgeId = Guid.Parse("2365b874-9d3b-49dd-84fb-c41de833385b");

        public Guid CreateFridgeId()
        {
            return _fridgeId;
        }

        private readonly Guid _ovenId = Guid.Parse("87db2d33-90b1-4e2a-8107-94b0cc927000");

        public Guid CreateOvenId()
        {
            return _ovenId;
        }

        private readonly Guid _stoveId = Guid.Parse("bb9fc125-0571-40cb-b3f8-fd2bb887b928");

        public Guid CreateStoveId()
        {
            return _stoveId;
        }
    }
}
