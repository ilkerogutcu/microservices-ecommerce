using System.Collections.Generic;
using Catalog.Application.Dtos;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.CreateManyProductsCommand
{
    public class CreateManyProductsCommand : IRequest<IDataResult<List<Product>>>
    {
        public List<CreateProductDto> Products { get; set; }
    }
}