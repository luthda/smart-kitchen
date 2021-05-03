using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Repositories
{

    public interface IDeviceRepository
    {
        Task Add(DeviceDto device);
        Task Remove(DeviceKeyDto key);
        Task<IEnumerable<DeviceDto>> GetAll();
        Task<bool> Exists(DeviceKeyDto key);
    }

    internal class DeviceRepository
        : RepositoryBase
        , IDeviceRepository
    {
        private readonly ConcurrentDictionary<DeviceKeyDto, DeviceDto> _devices = new ConcurrentDictionary<DeviceKeyDto, DeviceDto>();

        public DeviceRepository(ILogger<DeviceRepository> logger) : base(logger) { }

        public async Task Add(DeviceDto device)
        {
            if (device == null)
            {
                return;
            }
            await Task.Run(() =>
            {
                Log("Register", device);
                if (_devices.Any(d => d.Key.Equals(device.Key)))
                {
                    return;
                }
                _devices.TryAdd(device.Key, device);
            });
        }

        public async Task Remove(DeviceKeyDto key)
        {
            if (key == null)
            {
                return;
            }
            await Task.Run(() =>
            {
                Log("Unregister", key);
                _devices.TryRemove(key, out _);
            });
        }

        public async Task<IEnumerable<DeviceDto>> GetAll()
        {
            return await Task.Run(() =>
            {
                Log("Reading devices");
                return _devices
                    .ToArray() // Thread-safe
                    .Select(d => d.Value)
                    .ToList();
            });
        }

        public async Task<bool> Exists(DeviceKeyDto key) 
            => await Task.Run(
                () => _devices.Any(d => d.Key.Equals(key))
            );
    }
}
