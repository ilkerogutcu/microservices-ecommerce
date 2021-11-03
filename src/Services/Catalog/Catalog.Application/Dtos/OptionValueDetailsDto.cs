using System.Collections.Generic;

namespace Catalog.Application.Dtos
{
    public class OptionValueDetailsDto
    {
        public string OptionId { get; set; }
        public string OptionName { get; set; }
        public List<OptionValueDto> OptionValues { get; set; }
    }
}