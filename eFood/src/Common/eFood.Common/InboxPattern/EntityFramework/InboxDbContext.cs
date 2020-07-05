using Microsoft.EntityFrameworkCore;

namespace eFood.Common.InboxPattern.EntityFramework
{
    public class InboxDbContext : DbContext, IInboxMessage
    {
        public DbSet<InboxMessage> InboxMessages { get; set; }

        public InboxDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InboxMessage>()
                .HasKey(x => x.MessageId);

            modelBuilder.Entity<InboxMessage>()
                .Property(x => x.MessageId)
                .IsRequired()
                .ValueGeneratedNever();

            modelBuilder.Entity<InboxMessage>()
                .Property(x => x.ProcessedAt)
                .IsRequired();
        }
    }
}
