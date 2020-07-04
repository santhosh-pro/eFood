using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace eFood.Catalog.WebApi.DAL
{
    public class CatalogContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=Catalog;User ID=SA;Password=2wsx2WSX");
            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}
