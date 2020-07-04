using System;
using System.Collections;
using System.Collections.Generic;

namespace eFood.Catalog.WebApi.DAL.Models
{
    public sealed class Vendor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Food> Foods { get; set; }
    }
}
