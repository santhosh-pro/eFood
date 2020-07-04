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
    public class FoodsController : ControllerBase
    {
        private readonly ILogger<FoodsController> _logger;
        private readonly CatalogDbContext _context;

        public FoodsController(ILogger<FoodsController> logger, CatalogDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("{vendorId}/foods")]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetFoods()
        {
            return Ok(await _context.Foods
                .Select(x => new FoodDto
                {
                    Id = x.Id,
                    VendorId = x.VendorId,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<FoodDto>> GetById(Guid id)
        {
            return Ok(await _context.Foods
                .Where(x => x.Id == id)
                .Select(x => new FoodDto
                {
                    Id = x.Id,
                    VendorId = x.VendorId,
                    Name = x.Name,
                    Description = x.Description
                })
                .FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<ActionResult<FoodDto>> CreateVendor([FromBody] CreateFoodDto foodDto)
        {
            var newFood = new Food()
            {
                Name = foodDto.Name,
                Description = foodDto.Description,
                VendorId = foodDto.VendorId
            };
            _context.Add(newFood);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetById", new { id = newFood.Id });
        }
    }

    public sealed class FoodDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid VendorId { get; set; }
    }

    public sealed class CreateFoodDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid VendorId { get; set; }
    }
}
