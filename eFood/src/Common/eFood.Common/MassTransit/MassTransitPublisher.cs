using System;
using System.Threading.Tasks;
using MassTransit;

namespace eFood.Common.MassTransit
{
    public sealed class MassTransitPublisher : IBusPublisher
    {
        //  should use IBus instead of
        //  private readonly IPublishEndpoint _bus;
        //  https://github.com/MassTransit/MassTransit/issues/1862
        private readonly IBus _bus;

        public MassTransitPublisher(IBus bus)
        {
            _bus = bus;
        }

        public Task Publish<T>(T message) where T : class
        {
            return _bus.Publish(message, typeof(T));
        }

        public Task Publish(object message, Type type)
        {
            return _bus.Publish(message, type);
        }
    }
}
