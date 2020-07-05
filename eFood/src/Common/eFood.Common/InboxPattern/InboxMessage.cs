using System;
using System.Collections.Generic;
using System.Text;

namespace eFood.Common.InboxPattern
{
    public class InboxMessage
    {
        public Guid MessageId { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
