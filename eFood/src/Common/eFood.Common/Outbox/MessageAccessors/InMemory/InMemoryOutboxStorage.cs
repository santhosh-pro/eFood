using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eFood.Common.Outbox.MessageAccessors.InMemory
{
    public class InMemoryOutboxStorage : IOutboxAccessor, IOutboxMessageHandler
    {
        private static readonly ConcurrentDictionary<Guid, OutboxMessage> OutboxMessages = new ConcurrentDictionary<Guid, OutboxMessage>();

        public Task<IReadOnlyList<OutboxMessage>> GetPendingMessages()
        {
            return Task.FromResult<IReadOnlyList<OutboxMessage>>(OutboxMessages.Values
                .Where(m => m.ProcessedAt is null)
                .ToList());
        }

        public Task<OutboxMessage> GetPendingMessage()
        {
            return Task.FromResult(OutboxMessages.Values.FirstOrDefault(m => m.ProcessedAt is null));
        }

        public Task MarkAsProcessedAsync(OutboxMessage outboxMessage)
        {
            outboxMessage.ProcessedAt = DateTime.UtcNow;
            outboxMessage.State = MessageState.Published;

            return Task.CompletedTask;
        }

        public Task SaveToStorage(OutboxMessage outboxMessage)
        {
            OutboxMessages.TryAdd(outboxMessage.Id, outboxMessage);
            return Task.CompletedTask;
        }
    }
}
