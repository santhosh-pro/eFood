namespace eFood.Common.OutboxPattern
{
    public class OutboxMessageConfiguration
    {
        public bool Enable { get; set; }
        public double PollingIntervalMilliseconds { get; set; }
    }
}
