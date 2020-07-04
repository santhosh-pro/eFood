using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eFood.Catalog.WebApi.DAL;
using eFood.Catalog.WebApi.DAL.Models;
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

        public VendorsController(ILogger<VendorsController> logger, CatalogDbContext context)
        {
            _logger = logger;
            _context = context;
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
            var newVendor = new Vendor
            {
                Name = vendor.Name
            };
            _context.Add(newVendor);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetById", new { id = newVendor.Id });
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