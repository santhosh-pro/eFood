using Microsoft.EntityFrameworkCore;

namespace eFood.Common.OutboxPattern.EntityFramework
{
    public class OutboxDbContext : DbContext, IOutboxMessage
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public OutboxDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
