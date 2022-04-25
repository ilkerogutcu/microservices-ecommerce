using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Grpc.Interfaces;
using Catalog.Grpc.Interfaces.Repositories;
using Catalog.Grpc.Protos;
using Catalog.Grpc.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Catalog.Grpc.Services
{
    public class CatalogService : CatalogProtoService.CatalogProtoServiceBase

    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CatalogService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public override async Task<ProductResponse> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
            try
            {
                var productDetails = await _productRepository.GetProductDetailsByIdAsync(request.ProductId);
                if (string.IsNullOrEmpty(productDetails.Size)) productDetails.Size = "Tek Ebat";
                var result = _mapper.Map<ProductResponse>(productDetails);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return null;
            }
        }

        public override async Task<BoolValue> UpdateProductStockQuantityById(UpdateProductStockQuantityByIdRequest request, ServerCallContext context)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(request.ProductId);
                if (product is null) return new BoolValue {Value = false};
                product.StockQuantity = request.Quantity;
                await _productRepository.UpdateAsync(request.ProductId, product);
                return new BoolValue {Value = true};
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new BoolValue {Value = false};
            }
        }
    }
}