using Microsoft.EntityFrameworkCore;

namespace eFood.Common.InboxPattern.EntityFramework
{
    public interface IInboxMessage
    {
        DbSet<InboxMessage> InboxMessages { get; set; }
    }
}