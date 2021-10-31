﻿using Catalog.Application.Dtos;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.CreateBrandCommand
{
    public class CreateBrandCommand : IRequest<IDataResult<Brand>>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}