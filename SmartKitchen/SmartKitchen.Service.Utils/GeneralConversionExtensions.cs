using System;
using System.Collections.Generic;
using System.Linq;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Utils
{
    public static class GeneralConversionExtensions
    {
        public static DeviceKeyDto ToDto(this DeviceKey source)
            => new DeviceKeyDto
            {
                Id = source.Id.ToString(),
                Type = source.Type
            };

        public static DeviceKey ToEntity(this DeviceKeyDto source)
            => new DeviceKey(Type.GetType(source.Type), Guid.Parse(source.Id));

        public static DoorStateDto ToDto(this DoorState source)
            => source switch
            {
                DoorState.Closed => DoorStateDto.Closed,
                DoorState.Open => DoorStateDto.Open,
                _ => throw new ArgumentOutOfRangeException(nameof(source))
            };

        public static DoorState ToEntity(this DoorStateDto source)
            => source switch
            {
                DoorStateDto.Closed => DoorState.Closed,
                DoorStateDto.Open => DoorState.Open,
                _ => throw new ArgumentOutOfRangeException(nameof(source))
            };

        public static DeviceDto ToDto(this DeviceBase source)
        {
            DeviceDto result = new DeviceDto
            {
                Id = source.Id.ToString(),
                Key = source.Key.ToDto()
            };

            switch (source)
            {
                case Fridge f:
                    result.Fridge = new FridgeDetailDto
                    {
                        Temperature = f.Temperature,
                        Door = f.Door.ToDto()
                    };
                    break;
                case Oven o:
                    result.Oven = new OvenDetailDto
                    {
                        Temperature = o.Temperature,
                        Door = o.Door.ToDto()
                    };
                    break;
                case Stove s:
                    result.Stove = new StoveDetailDto
                    {
                        Temperature = s.Temperature,
                        HasPan = s.HasPan
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }

            return result;
        }

        public static IEnumerable<DeviceBase> ToEntities(this IEnumerable<DeviceDto> source)
            => source.Select(s => s.ToEntity<DeviceBase>());


        public static T ToEntity<T>(this DeviceDto source)
            where T : DeviceBase
        {
            var result = source.DetailCase switch
            {
                DeviceDto.DetailOneofCase.Fridge => (DeviceBase)new Fridge
                {
                    Temperature = source.Fridge.Temperature, 
                    Door = source.Fridge.Door.ToEntity()
                },
                DeviceDto.DetailOneofCase.Oven => new Oven
                {
                    Temperature = source.Oven.Temperature, 
                    Door = source.Oven.Door.ToEntity()
                },
                DeviceDto.DetailOneofCase.Stove => new Stove
                {
                    Temperature = source.Stove.Temperature,
                    HasPan = source.Stove.HasPan
                },
                _ => throw new ArgumentOutOfRangeException(nameof(source.DetailCase))
            };

            result.Id = Guid.Parse(source.Id);

            return (T)result;
        }

        public static CommandDto ToDto<T>(this ICommand<T> source)
            where T : DeviceBase
            => new CommandDto
            {
                DeviceState = source.DeviceState.ToDto(),
                Type = source.Type
            };

        public static ICommand<T> ToEntity<T>(this CommandDto source)
            where T : DeviceBase
            => new DeviceCommand<T>(
                source.DeviceState.ToEntity<T>(),
                source.Type
            );


        public static NotificationDto ToDto<T>(this INotification<T> source)
            where T : DeviceBase
            => new NotificationDto
            {
                DeviceState = source.DeviceState.ToDto(),
                Type = source.Type
            };

        public static INotification<T> ToEntity<T>(this NotificationDto source)
            where T : DeviceBase
            => new DeviceNotification<T>(
                source.DeviceState.ToEntity<T>(),
                source.Type
            );
    }
}
