using System.Threading;
using System.Threading.Tasks;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.UpdateProductCommand
{
    public class UpdateProductCommandHandler: IRequestHandler<UpdateProductCommand, IDataResult<Product>>
    {
        public Task<IDataResult<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}