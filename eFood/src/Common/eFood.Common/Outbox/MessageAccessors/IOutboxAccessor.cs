using System.Threading.Tasks;

namespace eFood.Common.Outbox.MessageAccessors
{
    public interface IOutboxAccessor
    {
        /// <summary>
        /// Get all pending message from outbox message storage
        /// </summary>
        /// <returns></returns>
        // Task<IReadOnlyList<OutboxMessage>> GetPendingMessages();
        Task<OutboxMessage> GetPendingMessage();

        /// <summary>
        /// Mark selected outbox message as published and saved
        /// </summary>
        /// <param name="outboxMessage"></param>
        /// <returns></returns>
        Task MarkAsProcessedAsync(OutboxMessage outboxMessage);
    }
}