using System;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Communication;
using Microsoft.Extensions.DependencyInjection;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Devices
{
    public static class SimulatorDeviceControllerExtensions
    {
        public static async Task RegisterAsync<T>(this ISimDevice<T> device, IServiceProvider services)
            where T : DeviceBase
        {
            if (device == null)
            {
                return;
            }
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var dataClient = services.GetService<ISimulatorDataClient<T>>();
            var messageClient = services.GetService<ISimulatorMessageClient<T>>();
            await device.RegisterAsync(new SimulatorDeviceController<T>(dataClient, messageClient));
        }
    }
}