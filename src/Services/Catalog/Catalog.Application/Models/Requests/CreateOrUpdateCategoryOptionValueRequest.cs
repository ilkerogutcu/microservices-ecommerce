using System.Collections.Generic;

namespace Catalog.Application.Models.Requests
{
    public class CreateOrUpdateCategoryOptionValueRequest
    {
        public string OptionId { get; set; }
        public bool IsRequired { get; set; }
        public bool Varianter { get; set; }
        public List<string> OptionValueIds { get; set; }
    }
}