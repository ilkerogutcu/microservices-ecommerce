using System.Collections.Generic;
using Catalog.Application.Dtos;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Products.CreateProductCommand
{
    public class CreateProductCommand : IRequest<IDataResult<Product>>
    {
        public string CategoryId { get; set; }
        public string BrandId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ModelCode { get; set; }
        public List<IFormFile> FileList { get; set; }
        public List<SkuDto> Skus { get; set; }
    }
}