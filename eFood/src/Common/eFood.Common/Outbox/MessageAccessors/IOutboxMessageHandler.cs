using System.Threading.Tasks;

namespace eFood.Common.Outbox.MessageAccessors
{
    public interface IOutboxMessageHandler
    {
        /// <summary>
        /// Add message to storage to be published in future
        /// </summary>
        /// <param name="outboxMessage"></param>
        /// <returns></returns>
        Task SaveToStorage(OutboxMessage outboxMessage);
    }
}