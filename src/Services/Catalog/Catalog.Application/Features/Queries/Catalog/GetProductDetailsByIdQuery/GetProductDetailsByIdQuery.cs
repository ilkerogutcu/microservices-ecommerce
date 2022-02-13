using System.Collections.Generic;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Catalog.GetProductDetailsByIdQuery
{
    public class GetProductDetailsByIdQuery : IRequest<IDataResult<List<ProductDetailsViewModel>>>
    {
        public string Id { get; set; }

        public GetProductDetailsByIdQuery(string ıd)
        {
            Id = ıd;
        }
    }
}