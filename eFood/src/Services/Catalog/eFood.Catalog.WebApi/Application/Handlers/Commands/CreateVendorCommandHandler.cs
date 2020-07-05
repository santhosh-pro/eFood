using System;
using System.Threading;
using System.Threading.Tasks;
using eFood.Catalog.WebApi.Application.Commands;
using eFood.Catalog.WebApi.Controllers;
using eFood.Catalog.WebApi.DAL;
using eFood.Catalog.WebApi.DAL.Models;
using eFood.Common.MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eFood.Catalog.WebApi.Application.Handlers.Commands
{
    public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, CreateVendorResult>
    {
        private readonly ILogger<CreateVendorCommandHandler> _logger;
        private readonly CatalogDbContext _context;
        private readonly IBusPublisher _publisher;

        public CreateVendorCommandHandler(ILogger<CreateVendorCommandHandler> logger, CatalogDbContext context, IBusPublisher publisher)
        {
            _logger = logger;
            _context = context;
            _publisher = publisher;
        }

        public async Task<CreateVendorResult> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            var newVendor = new Vendor
            {
                Name = request.Name
            };
            _context.Add(newVendor);
            await _context.SaveChangesAsync();

            await _publisher.Publish(new VendorCreateEvent(Guid.NewGuid(), newVendor.Id, newVendor.Name));

            return new CreateVendorResult
            {
                Id = newVendor.Id,
                Name = newVendor.Name
            };
        }
    }
}
