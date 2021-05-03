using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Repositories
{
    internal class RepositoryBase
    {
        private readonly ILogger _logger;

        public RepositoryBase(ILogger logger)
        {
            _logger = logger;
        }

        protected void Log<T>(string message, T obj)
        {
            var typeName = "???";
            var typeInfo = "???";
            if (obj != null)
            {
                typeName = obj.GetType().Name;
                typeInfo = obj.ToString();
            }
            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}: {message} => {typeName} / {typeInfo}");
        }

        protected void Log(string message)
        {
            _logger.LogInformation($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}: {message}");
        }



        protected ConcurrentQueue<T> GetQueue<T>(
            DeviceDto device, 
            IDictionary<DeviceKeyDto, ConcurrentQueue<T>> queues)
        {
            if (!queues.TryGetValue(device.Key, out var queue))
            {
                queue = new ConcurrentQueue<T>();
                queues.Add(device.Key, queue);
            }
            return queue;
        }

    }
}