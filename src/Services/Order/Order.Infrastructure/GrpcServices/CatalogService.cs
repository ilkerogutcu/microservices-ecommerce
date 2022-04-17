using System.Threading.Tasks;
using AutoMapper;
using Catalog.Grpc.Protos;
using Order.Application.Dtos;
using Order.Application.Interfaces.Services;

namespace Order.Infrastructure.GrpcServices
{
    public class CatalogService : ICatalogService
    {
        private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoService;
        private readonly IMapper _mapper;

        public CatalogService(CatalogProtoService.CatalogProtoServiceClient catalogProtoService, IMapper mapper)
        {
            _catalogProtoService = catalogProtoService;
            _mapper = mapper;
        }

        public async Task<ProductDetailsDto> GetProductDetailsByIdAsync(string id)
        {
            var productDetails = await _catalogProtoService.GetProductByIdAsync(new GetProductByIdRequest {ProductId = id});
            return _mapper.Map<ProductDetailsDto>(productDetails);
        }

        public async Task<bool> UpdateProductStockQuantityById(string id, int quantity)
        {
            var result = await _catalogProtoService.UpdateProductStockQuantityByIdAsync(new UpdateProductStockQuantityByIdRequest
                {ProductId = id, Quantity = quantity});
            return result.Value;
        }
    }
}