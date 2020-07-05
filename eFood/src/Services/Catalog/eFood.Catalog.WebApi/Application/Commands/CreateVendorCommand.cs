using System;
using MediatR;

namespace eFood.Catalog.WebApi.Application.Commands
{
    public class CreateVendorCommand : IRequest<CreateVendorResult>
    {
        public string Name { get; }

        public CreateVendorCommand(string name)
        {
            Name = name;
        }
    }

    public sealed class CreateVendorResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
