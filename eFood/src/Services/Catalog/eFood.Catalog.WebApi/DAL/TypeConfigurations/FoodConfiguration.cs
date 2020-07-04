using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eFood.Catalog.WebApi.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eFood.Catalog.WebApi.DAL.TypeConfigurations
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(255);
        }
    }
}
