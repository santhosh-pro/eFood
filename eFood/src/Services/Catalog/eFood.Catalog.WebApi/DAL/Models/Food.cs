using System;

namespace eFood.Catalog.WebApi.DAL.Models
{
    public sealed class Food
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid VendorId { get; set; }


        public Vendor Vendor { get; set; }

    }
}
