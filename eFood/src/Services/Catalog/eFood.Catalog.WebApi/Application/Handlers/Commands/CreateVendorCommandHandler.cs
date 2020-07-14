using System;
using System.Threading;
using System.Threading.Tasks;
using eFood.Catalog.WebApi.Application.Commands;
using eFood.Catalog.WebApi.Controllers;
using eFood.Catalog.WebApi.DAL;
using eFood.Catalog.WebApi.DAL.Models;
using eFood.Common.MassTransit;
using eFood.Common.OutboxPattern;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace eFood.Catalog.WebApi.Application.Handlers.Commands
{
    public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, CreateVendorResult>
    {
        private readonly ILogger<CreateVendorCommandHandler> _logger;
        private readonly CatalogDbContext _context;
        private readonly IBusPublisher _publisher;
        private readonly IOutboxMessageSave _outboxMessage;

        public CreateVendorCommandHandler(ILogger<CreateVendorCommandHandler> logger, CatalogDbContext context, IBusPublisher publisher, IOutboxMessageSave outboxMessage)
        {
            _logger = logger;
            _context = context;
            _publisher = publisher;
            _outboxMessage = outboxMessage;
        }

        public async Task<CreateVendorResult> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            var newVendor = new Vendor
            {
                Name = request.Name
            };
            _context.Add(newVendor);
            await _context.SaveChangesAsync();

            // await _publisher.Publish(new VendorCreateEvent(Guid.NewGuid(), newVendor.Id, newVendor.Name));
            var message = new VendorCreateEvent(Guid.NewGuid(), newVendor.Id, newVendor.Name);
            var messageOutbox = new OutboxMessage
            {
                CorrelationId = Guid.NewGuid(),
                CreateAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                State = MessageState.NotPublished,
                TypeFullName = message.GetType().FullName + ", " + message.GetType().Assembly.GetName().Name,
                // TypeFullName = message.GetType().FullName,
                SerializedMessage = JsonConvert.SerializeObject(message)
            };

            await _outboxMessage.SaveToStorage(messageOutbox);

            return new CreateVendorResult
            {
                Id = newVendor.Id,
                Name = newVendor.Name
            };
        }
    }
}
