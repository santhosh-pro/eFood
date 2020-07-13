using System;
using System.Collections.Generic;
using System.Text;

namespace eFood.Common.OutboxPattern
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string TypeFullName { get; set; }
        public MessageState State { get; set; }
        public string SerializedMessage { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ProcessingStart { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }

    public enum MessageState
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3
    }
}
