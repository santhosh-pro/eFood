using eFood.Catalog.WebApi.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFood.Catalog.WebApi.DAL.TypeConfigurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(255);
        }
    }
}
