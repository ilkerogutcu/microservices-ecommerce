using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Features.Queries.Catalog.GetProductsByCategoryIdQuery;
using Catalog.Application.Features.Queries.Catalog.ViewModels;
using Catalog.Application.Features.Queries.Products.GetAllProductsQuery;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IProductRepository : IDocumentDbRepository<Product>
    {
        Task<List<ProductDto>> GetAllProductsAsync(GetAllProductsQuery query);
        Task<List<ProductCardViewModel>> GetTopProductsAsync(int count);
        Task<List<ProductCardViewModel>> GetProductsByCategoryIdAsync(GetProductsByCategoryIdQuery query);
        Task<List<ProductDetailsViewModel>> GetProductDetailsByIdAsync(string id);
    }
}