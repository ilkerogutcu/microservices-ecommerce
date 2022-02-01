using System.Collections.Generic;
using System.Text.Json.Serialization;
using Catalog.Domain.Entities;
using MediatR;
using Olcsan.Boilerplate.Utilities.Results;

namespace Catalog.Application.Features.Commands.Categories.CreateCategoryOptionValueCommand
{
    public class CreateCategoryOptionValueCommand : IRequest<IDataResult<CategoryOptionValue>>
    {
        [JsonIgnore]
        public string CategoryId { get; set; }
        
        public string OptionId { get; set; }
        public bool IsRequired { get; set; }
        public bool Slicer { get; set; }
        public bool Varianter { get; set; }
        public List<string> OptionValueIds { get; set; }
    }
}