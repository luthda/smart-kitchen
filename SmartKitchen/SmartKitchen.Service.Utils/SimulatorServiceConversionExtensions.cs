using System;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Utils
{
    public static class SimulatorServiceConversionExtensions
    {
        public static PeekCommandRequest ToPeekCommandRequest(this DeviceKey source)
            => new PeekCommandRequest
            {
                DeviceKey = source.ToDto()
            };

        public static ICommand<T> ToCommand<T>(this PeekCommandResponse source)
            where T : DeviceBase
            => source.CommandCase switch
            {
                PeekCommandResponse.CommandOneofCase.DeviceCommand => source.DeviceCommand.ToEntity<T>(),
                PeekCommandResponse.CommandOneofCase.NullCommand => NullCommand<T>.Empty,
                _ => throw new ArgumentOutOfRangeException()
            };

        public static SendNotificationRequest ToSendNotificationRequest<T>(this INotification<T> source)
            where T : DeviceBase
            => new SendNotificationRequest
            {
                Notification = source.ToDto()
            };

        public static RegisterDeviceRequest ToRegisterDeviceRequest<T>(this T source)
            where T : DeviceBase
            => new RegisterDeviceRequest
            {
                Device = source.ToDto()
            };

        public static UnregisterDeviceRequest ToUnregisterDeviceRequest<T>(this T source)
            where T : DeviceBase
            => new UnregisterDeviceRequest
            {
                DeviceKey = source.Key.ToDto()
            };

    }
}
