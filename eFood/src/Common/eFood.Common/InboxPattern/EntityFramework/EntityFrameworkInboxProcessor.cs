using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eFood.Common.InboxPattern.EntityFramework
{
    public class EntityFrameworkInboxProcessor<TDbContext> : IInboxMessageProcessor where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public EntityFrameworkInboxProcessor(TDbContext context)
        {
            _context = context;
        }

        public async Task ProcessMessage(Guid messageId)
        {
            var dbSet = _context.Set<InboxMessage>();

            dbSet.Add(new InboxMessage
            {
                MessageId = messageId,
                ProcessedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

        }
    }
}
