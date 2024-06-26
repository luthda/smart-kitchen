﻿using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hsr.CloudSolutions.SmartKitchen.Util
{
    public class SmartKitchenConfiguration
    {
        public string GrpcHostAddress { get; set; }
        public string StorageConnectionString { get; set; }
        public string ServicesBusConnectionString { get; set; }
        public string CloudTableName { get; set; }
        public string NotificationTopic { get; set; }
        public string CommandTopic { get; set; }

        public string DeviceFunctionsString { get; set; }
    }

    public static class SmartKitchenConfigurationExtensions
    {
        public static void AddSmartKitchenConfiguration(this IServiceCollection services, string settingsFile = "appsettings.json")
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile, optional: false);
            
            IConfigurationRoot config = configBuilder
                .Build();

            services.Configure<SmartKitchenConfiguration>(config);

            services.AddSingleton(
                s => s.GetService<IOptions<SmartKitchenConfiguration>>().Value
            );
        }
    }
}
