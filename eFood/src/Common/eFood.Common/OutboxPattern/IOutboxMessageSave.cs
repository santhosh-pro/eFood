using System.Threading.Tasks;

namespace eFood.Common.OutboxPattern
{
    public interface IOutboxMessageSave
    {
        /// <summary>
        /// Add message to storage to be published in future
        /// </summary>
        /// <param name="outboxMessage"></param>
        /// <returns></returns>
        Task SaveToStorage(OutboxMessage outboxMessage);
    }

    public interface IOutboxMessageAccessor
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