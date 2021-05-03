using System.Collections.Concurrent;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Hsr.CloudSolutions.SmartKitchen.Service.Repositories
{
    public interface ICommandRepository
    {
        Task SendCommandAsync(CommandDto command);
        Task<CommandDto> PeekCommandAsync(DeviceKeyDto key);
    }

    internal class CommandRepository
        : RepositoryBase, ICommandRepository
    {
        private readonly ConcurrentDictionary<DeviceKeyDto, ConcurrentQueue<CommandDto>> _commandQueues = new ConcurrentDictionary<DeviceKeyDto, ConcurrentQueue<CommandDto>>();

        public CommandRepository(ILogger<CommandRepository> logger) : base(logger) { }

        public Task SendCommandAsync(CommandDto command)
        {
            if (command?.DeviceState == null)
            {
                return Task.CompletedTask;
            }

            var queue = GetQueue(command.DeviceState, _commandQueues);
            Log("Sending command", command);
            queue.Enqueue(command);

            return Task.CompletedTask;
        }

        public Task<CommandDto> PeekCommandAsync(DeviceKeyDto key)
        {
            if (key == null)
            {
                return null;
            }

            if (!_commandQueues.TryGetValue(key, out var queue))
            {
                return Task.FromResult<CommandDto>(null);
            }
            Log("Checking commands", key);

            CommandDto result = null;
            if (queue.Count > 0)
            {
                queue.TryDequeue(out result);
            }
            return Task.FromResult(result);
        }
    }
}
