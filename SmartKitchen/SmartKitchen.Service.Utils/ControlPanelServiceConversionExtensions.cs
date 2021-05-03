using System;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Utils
{
    public static class ControlPanelServiceConversionExtensions
    {
        public static PeekNotificationRequest ToPeekNotificationRequest(this DeviceKey source)
            => new PeekNotificationRequest
            {
                DeviceKey = source.ToDto()
            };

        public static INotification<T> ToNotification<T>(this PeekNotificationResponse source)
            where T : DeviceBase
            => source.NotificationCase switch
            {
                PeekNotificationResponse.NotificationOneofCase.DeviceNotification => source.DeviceNotification.ToEntity<T>(),
                PeekNotificationResponse.NotificationOneofCase.NullNotification => NullNotification<T>.Empty,
                _ => throw new ArgumentOutOfRangeException()
            };
        
        public static SendCommandRequest ToSendCommandRequest<T>(this ICommand<T> source)
            where T : DeviceBase
            => new SendCommandRequest
            {
                Command = source.ToDto()
            };

    }
}
