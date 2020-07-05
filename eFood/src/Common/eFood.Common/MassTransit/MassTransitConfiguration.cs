namespace eFood.Common.MassTransit
{
    public class MassTransitConfiguration
    {
        public string RabbitMQAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
    }
}
