using System;
using System.Threading.Tasks;
using eFood.Basket.WebApi.DAL;
using eFood.Basket.WebApi.DAL.Models;
using eFood.Common.InboxPattern;
using eFood.Common.InboxPattern.EntityFramework;
using eFood.Common.MassTransit.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace eFood.Basket.WebApi.Consumers
{
    public class CatalogConsumer : IConsumer<IVendorCreateEvent>
    {
        private readonly ILogger<CatalogConsumer> _logger;
        private readonly BasketDbContext _context;
        private readonly InboxDbContext _inboxDbContext;

        public CatalogConsumer(ILogger<CatalogConsumer> logger, BasketDbContext context, InboxDbContext inboxDbContext)
        {
            _logger = logger;
            _context = context;
            _inboxDbContext = inboxDbContext;
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
            _inboxDbContext.Add(new InboxMessage
            {
                MessageId = message.CorrelationId,
                ProcessedAt = DateTime.Now
            });

            using (var transaction = _context.Database.BeginTransaction())
            {
                await _inboxDbContext.SaveChangesAsync();
                await _context.SaveChangesAsync();
                transaction.Commit();
            }

            

            _logger.LogDebug("End consuming incoming message");
        }
    }
}
