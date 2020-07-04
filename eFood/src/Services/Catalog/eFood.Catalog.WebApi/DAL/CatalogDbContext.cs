using eFood.Catalog.WebApi.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace eFood.Catalog.WebApi.DAL
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Food> Foods { get; set; }

        public CatalogDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        }
    }
}
