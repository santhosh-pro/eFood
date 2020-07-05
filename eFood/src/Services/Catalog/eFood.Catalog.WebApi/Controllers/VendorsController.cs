using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eFood.Catalog.WebApi.Application.Commands;
using eFood.Catalog.WebApi.DAL;
using eFood.Catalog.WebApi.DAL.Models;
using eFood.Common.MassTransit;
using eFood.Common.MassTransit.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace eFood.Catalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly ILogger<VendorsController> _logger;
        private readonly CatalogDbContext _context;
        private readonly IMediator _mediator;
        private readonly IBusPublisher _publisher;

        public VendorsController(ILogger<VendorsController> logger, CatalogDbContext context, IMediator mediator)
        {
            _logger = logger;
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetVendors()
        {
            return Ok(await _context.Vendors
                .Select(x => new VendorDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<VendorDto>> GetById(Guid id)
        {
            return Ok(await _context.Vendors
                .Where(x => x.Id == id)
                .Select(x => new VendorDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<ActionResult<VendorDto>> CreateVendor([FromBody]CreateVendorDto vendor)
        {
            var result = await _mediator.Send(new CreateVendorCommand(vendor.Name));

            return RedirectToAction("GetById", new { id = result.Id });
        }
    }

    public class VendorCreateEvent : IVendorCreateEvent
    {
        public Guid CorrelationId { get; }
        public Guid VendorId { get; }
        public string Name { get; }

        public VendorCreateEvent(Guid? correlationId, Guid vendorId, string name)
        {
            CorrelationId = correlationId ?? Guid.NewGuid();
            VendorId = vendorId;
            Name = name;
        }
    }

    public sealed class VendorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public sealed class CreateVendorDto
    {
        public string Name { get; set; }
    }
}