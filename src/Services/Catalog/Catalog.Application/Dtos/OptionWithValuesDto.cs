using System.Collections.Generic;

namespace Catalog.Application.Dtos
{
    public class OptionWithValuesDto
    {
        public string OptionId { get; set; }
        public string OptionName { get; set; }
        public bool IsActive { get; set; }
        public List<OptionValueDto> OptionValues { get; set; }
    }
}