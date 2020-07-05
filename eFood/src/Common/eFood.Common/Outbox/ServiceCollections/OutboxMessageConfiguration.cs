using System;
using System.Collections.Generic;
using System.Text;

namespace eFood.Common.Outbox.ServiceCollections
{
    public class OutboxMessageConfiguration
    {
        public bool Enable { get; set; }
        public double PollingDbIntervalMilliseconds { get; set; }
    }
}
