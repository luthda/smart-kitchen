using System.Collections.Concurrent;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Repositories
{
    public interface INotificationRepository
    {
        Task SendNotificationAsync(NotificationDto notification);
        Task<NotificationDto> PeekNotificationAsync(DeviceKeyDto key);
    }

    internal class NotificationRepository
        : RepositoryBase, INotificationRepository
    {
        private readonly ConcurrentDictionary<DeviceKeyDto, ConcurrentQueue<NotificationDto>> _notificationQueues = new ConcurrentDictionary<DeviceKeyDto, ConcurrentQueue<NotificationDto>>();

        public NotificationRepository(ILogger<NotificationRepository> logger) : base(logger) { }
        

        public async Task SendNotificationAsync(NotificationDto notification)
        {
            if (notification?.DeviceState == null)
            {
                return;
            }
            await Task.Run(() =>
            {
                var queue = GetQueue(notification.DeviceState, _notificationQueues);
                Log("Send notification", notification);
                queue.Enqueue(notification);
            });
        }

        public async Task<NotificationDto> PeekNotificationAsync(DeviceKeyDto key)
        {
            if (key == null)
            {
                return null;
            }
            return await Task.Run(() =>
            {
                if (!_notificationQueues.TryGetValue(key, out var queue))
                {
                    return null;
                }
                Log("Checking notifications", key);

                if (queue.Count > 0)
                {
                    queue.TryDequeue(out NotificationDto result);
                    return result;
                }
                return null;
            });
        }

    }
}
