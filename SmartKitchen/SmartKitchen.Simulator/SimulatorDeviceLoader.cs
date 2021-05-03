using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Devices;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator
{
    public class SimulatorDeviceLoader
        : IDeviceLoader
    {
        private readonly IServiceProvider _services;
        private readonly IDeviceIdFactory _idFactory;

        public SimulatorDeviceLoader(IServiceProvider services, IDeviceIdFactory idFactory)
        {
            _services = services;
            _idFactory = idFactory;
        }

        public async Task<IEnumerable<ISimDevice>> LoadDevicesAsync()
        {
            var devices = new List<ISimDevice>
            {
                await CreateFridgeAsync(),
                await CreateOvenAsync(),
                await CreateStoveAsync()
            };

            return devices;
        }

        private async Task<ISimDevice> CreateFridgeAsync()
        {
            var topLeft = new Point(.096, .281);
            var bottomright = new Point(.226, .658);
            var size = GetSize(topLeft, bottomright);

            var fridge = new SimFridge(_idFactory.CreateFridgeId(), topLeft, size);
            fridge.ConfigureWith(new Fridge { Temperature = 5 });
            await fridge.RegisterAsync(_services);

            return fridge;
        }

        private async Task<ISimDevice> CreateOvenAsync()
        {
            var topLeft = new Point(.470, .615);
            var bottomRight = new Point(.610, .890);
            var size = GetSize(topLeft, bottomRight);

            var oven = new SimOven(_idFactory.CreateOvenId(), topLeft, size);
            await oven.RegisterAsync(_services);

            return oven;
        }

        private async Task<ISimDevice> CreateStoveAsync()
        {
            var topLeft = new Point(.430, .420);
            var bottomRight = new Point(.615, .597);
            var size = GetSize(topLeft, bottomRight);

            var stove = new SimStove(_idFactory.CreateStoveId(), topLeft, size);
            await stove.RegisterAsync(_services);

            return stove;
        }

        private Point GetSize(Point topLeft, Point bottomRight)
        {
            return new Point(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }
    }
}
