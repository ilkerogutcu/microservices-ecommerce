

using System.Text.Json.Serialization;

namespace Catalog.Domain.Enums
{
 
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortBy
    {
        PriceByDesc,
        PriceByAsc,
        MostRecent,
        BestSeller
    }
}