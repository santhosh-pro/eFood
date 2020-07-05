using System.Threading.Tasks;
using eFood.Basket.WebApi.DAL;
using eFood.Basket.WebApi.DAL.Models;
using eFood.Common.MassTransit.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eFood.Basket.WebApi.Consumers
{
    public class CatalogConsumer : IConsumer<IVendorCreateEvent>
    {
        private readonly ILogger<CatalogConsumer> _logger;
        private readonly BasketDbContext _context;

        public CatalogConsumer(ILogger<CatalogConsumer> logger, BasketDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Consume(ConsumeContext<IVendorCreateEvent> context)
        {
            _logger.LogDebug($"Start consuming incoming message with correlationId: {context.Message.CorrelationId}");

            var message = context.Message;

            _context.Add(new Vendor
            {
                Id = message.VendorId,
                Name = message.Name
            });
            await _context.SaveChangesAsync();

            _logger.LogDebug("End consuming incoming message");
        }
    }
}
