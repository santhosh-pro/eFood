using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace eFood.Basket.WebApi.DAL
{
    public class BasketContextFactory : IDesignTimeDbContextFactory<BasketDbContext>
    {
        public BasketDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BasketDbContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=Basket;User ID=SA;Password=2wsx2WSX");
            return new BasketDbContext(optionsBuilder.Options);
        }
    }
}
