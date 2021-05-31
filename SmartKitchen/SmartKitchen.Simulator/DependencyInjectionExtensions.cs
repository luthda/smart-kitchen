using System;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Communication;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Communication.Azure;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Communication.Grpc;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Extensions.DependencyInjection;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection ConfigureSimulator(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSmartKitchenConfiguration();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<IDialogService, DialogService>(
                s => new DialogService(s.GetService<MainWindow>())
            );

            services.ConfigureCommunication();
            services.ConfigureDevices();

            return services;
        }

        private static void ConfigureCommunication(this IServiceCollection services)
        {
            // TODO: Bind here!!!
            // If Azure support is implemented, comment this type registration since it's no longer needed
            //services.AddTransient(typeof(ISimulatorDataClient<>), typeof(GrpcSimulatorDataClient<>));
            //services.AddTransient(typeof(ISimulatorMessageClient<>), typeof(GrpcSimulatorMessageClient<>));

            // TODO: To support Azure, uncomment this type registration

            // Simulator Info Client
            // To support Azure, uncomment this type registration
            //services.AddTransient(typeof(ISimulatorDataClient<>), typeof(BlobSimulatorDataClient<>));
            services.AddTransient(typeof(ISimulatorDataClient<>), typeof(AzureSimulatorDataClient<>));

            // Simulator DeviceState Client
            //services.AddTransient(typeof(ISimulatorMessageClient<>), typeof(AzureSimulatorMessageClient<>));
        }

        private static void ConfigureDevices(this IServiceCollection services)
        {
            services.AddTransient<ISimulatorDeviceCollection, SimulatorDeviceCollection>();
            services.AddTransient<ISimulatorDeviceFactory, SimulatorDeviceFactory>();
            services.AddTransient<IDeviceLoader, SimulatorDeviceLoader>();
            services.AddTransient<IDeviceIdFactory, StaticDeviceIdFactory>();
            //services.AddTransient<IDeviceIdFactory, DynamicDeviceIdeFactory>();
        }
    }
}
