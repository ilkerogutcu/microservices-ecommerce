﻿using System.Collections.Generic;
using Catalog.Application.Dtos;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Queries.Brands.GetNotActiveBrandsQuery
{
    public class GetNotActiveBrandsQuery : IRequest<IDataResult<List<BrandDto>>>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}