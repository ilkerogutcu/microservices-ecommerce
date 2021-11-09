﻿using System.Collections.Generic;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.CreateOrUpdateCategoryOptionValueCommand
{
    public class CreateOrUpdateCategoryOptionValueCommand: IRequest<IDataResult<CategoryOptionValue>>
    {
        public string CategoryId { get; set; }
        public string OptionId { get; set; }
        public bool IsRequired { get; set; }
        public bool Varianter { get; set; }
        public List<string>  OptionValueIds { get; set; }
    }
}