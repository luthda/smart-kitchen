using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Hsr.CloudSolutions.SmartKitchen.Service.Repositories;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Services
{
    public class ControlPanelService
        : Interface.ControlPanelService.ControlPanelServiceBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IDeviceRepository _deviceRepository;

        public ControlPanelService(
            ICommandRepository commandRepository,
            INotificationRepository notificationRepository,
            IDeviceRepository deviceRepository)
        {
            _commandRepository = commandRepository;
            _notificationRepository = notificationRepository;
            _deviceRepository = deviceRepository;
        }

        public override async Task<Empty> SendCommand(SendCommandRequest request, ServerCallContext context)
        {
            await _commandRepository.SendCommandAsync(
                request.Command
            );
            return new Empty();
        }

        public override async Task<PeekNotificationResponse> PeekNotification(PeekNotificationRequest request, ServerCallContext context)
        {
            NotificationDto notification = await _notificationRepository.PeekNotificationAsync(request.DeviceKey);
            PeekNotificationResponse result = new PeekNotificationResponse();

            if (notification != null)
            {
                result.DeviceNotification = notification;
            }
            else
            {
                result.NullNotification = new NullNotificationDto();
            }

            return result;
        }

        public override async Task<RegisteredDevicesResponse> GetRegisteredDevices(Empty request, ServerCallContext context)
        {
            IEnumerable<DeviceDto> devices = await _deviceRepository.GetAll();
            var result = new RegisteredDevicesResponse();
            result.Devices.AddRange(devices);

            return result;
        }
    }
}
