using System;
using System.Threading.Tasks;

namespace eFood.Common.MassTransit
{
    public interface IBusPublisher
    {
        Task Publish<T>(T message) where T : class;
        Task Publish(object message, Type type);
        Task Publish<T>(object message);
    }
}