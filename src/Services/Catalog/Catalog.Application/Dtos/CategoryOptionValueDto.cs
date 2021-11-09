using System.Collections.Generic;

namespace Catalog.Application.Dtos
{
    public class CategoryOptionValueDto
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public OptionDto Option { get; set; }
        public bool Varianter { get; set; }
        public bool IsRequired { get; set; }
        public List<OptionValueDto> OptionValues { get; set; }
    }
}