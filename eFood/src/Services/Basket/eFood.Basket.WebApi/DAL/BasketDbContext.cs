using eFood.Basket.WebApi.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace eFood.Basket.WebApi.DAL
{
    public class BasketDbContext : DbContext
    {
        public DbSet<Vendor> Vendors { get; set; }

        public BasketDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketDbContext).Assembly);
        }
    }
}
