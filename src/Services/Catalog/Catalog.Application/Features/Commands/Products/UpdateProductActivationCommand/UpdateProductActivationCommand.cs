using System.Text.Json.Serialization;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.UpdateProductActivationCommand
{
    public class UpdateProductActivationCommand : IRequest<IResult>
    {
        [JsonIgnore] public string Id { get; set; }
        public bool IsActive { get; set; }
    }
}