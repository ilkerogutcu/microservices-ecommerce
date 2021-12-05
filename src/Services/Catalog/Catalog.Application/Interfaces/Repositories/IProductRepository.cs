using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Queries.Products.GetAllProductsQuery;
using Catalog.Domain.Common;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IProductRepository : IDocumentDbRepository<Product>
    {
        Task<List<ProductDto>> GetAllProducts(GetAllProductsQuery query);
    }
}