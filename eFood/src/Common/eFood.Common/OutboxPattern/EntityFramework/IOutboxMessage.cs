using Microsoft.EntityFrameworkCore;

namespace eFood.Common.OutboxPattern.EntityFramework
{
    public interface IOutboxMessage
    {
        DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}