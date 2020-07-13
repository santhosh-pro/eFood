using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eFood.Common.OutboxPattern.EntityFramework
{
    public sealed class EntityFrameworkAccessor<TDbContext> : IOutboxMessageSave where TDbContext : DbContext
    {
        private readonly TDbContext _context;

        public EntityFrameworkAccessor(TDbContext context)
        {
            _context = context;
        }

        public async Task SaveToStorage(OutboxMessage outboxMessage)
        {
            var dbSet = _context.Set<OutboxMessage>();
            dbSet.Add(outboxMessage);
            await _context.SaveChangesAsync();
        }
    }
}
