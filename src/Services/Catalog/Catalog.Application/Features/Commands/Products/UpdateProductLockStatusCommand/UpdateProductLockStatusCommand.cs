using System.Text.Json.Serialization;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.UpdateProductLockStatusCommand
{
    public class UpdateProductLockStatusCommand : IRequest<IResult>
    {
        [JsonIgnore] public string Id { get; set; }
        public bool LockStatus { get; set; }
    }
}