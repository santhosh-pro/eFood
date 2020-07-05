using System;
using MassTransit;

namespace eFood.Common.MassTransit.Messages
{
    public interface IVendorCreateEvent : CorrelatedBy<Guid>
    {
        public Guid VendorId { get; }
        public string Name { get; }
    }
}