using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Hsr.CloudSolutions.SmartKitchen.Service.Repositories;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Services
{
    public class SimulatorService
        : Interface.SimulatorService.SimulatorServiceBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IDeviceRepository _deviceRepository;

        public SimulatorService(
            ICommandRepository commandRepository,
            INotificationRepository notificationRepository,
            IDeviceRepository deviceRepository)
        {
            _commandRepository = commandRepository;
            _notificationRepository = notificationRepository;
            _deviceRepository = deviceRepository;
        }

        public override async Task<Empty> SendNotification(SendNotificationRequest request, ServerCallContext context)
        {
            await _notificationRepository.SendNotificationAsync(
                request.Notification
            );
            return new Empty();
        }

        public override async Task<PeekCommandResponse> PeekCommand(PeekCommandRequest request, ServerCallContext context)
        {
            CommandDto command = await _commandRepository.PeekCommandAsync(request.DeviceKey);
            PeekCommandResponse result = new PeekCommandResponse();

            if (command != null)
            {
                result.DeviceCommand = command;
            }
            else
            {
                result.NullCommand = new NullCommandDto();
            }

            return result;
        }

        public override async Task<Empty> RegisterDevice(RegisterDeviceRequest request, ServerCallContext context)
        {
            await _deviceRepository.Add(
                request.Device
            );
            return new Empty();
        }

        public override async Task<Empty> UnregisterDevice(UnregisterDeviceRequest request, ServerCallContext context)
        {
            await _deviceRepository.Remove(
                request.DeviceKey
            );
            return new Empty();
        }
    }
}
