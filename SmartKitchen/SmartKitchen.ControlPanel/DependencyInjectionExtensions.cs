﻿using System;
using Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication.Azure;
using Hsr.CloudSolutions.SmartKitchen.ControlPanel.ViewModels;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Extensions.DependencyInjection;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection ConfigureControlPanel(this IServiceCollection services)
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

            services.SetupCommunication();
            services.SetupDeviceController();

            return services;
        }

        private static void SetupCommunication(this IServiceCollection services)
        {
            services.AddTransient(typeof(IControlPanelDataClient), typeof(AzureControlPanelDataClient));
            services.AddTransient(typeof(IControlPanelMessageClient<>), typeof(AzureControlPanelMessageClient<>));
        }

        private static void SetupDeviceController(this IServiceCollection services)
        {
            services.AddTransient(typeof(IDeviceControllerViewModel<>), typeof(UnknownDeviceControllerViewModel<>));
            services.AddTransient<IDeviceControllerViewModel<Fridge>, FridgeControllerViewModel>();
            services.AddTransient<IDeviceControllerViewModel<Oven>, OvenControllerViewModel>();
            services.AddTransient<IDeviceControllerViewModel<Stove>, StoveControllerViewModel>();
        }
    }
}