using System.Collections.Generic;
using System.Linq;

namespace Catalog.Grpc.Utilities
{
    public static class ColorUtils
    {
        private static readonly IReadOnlyDictionary<string, string> ColorMapping = new Dictionary<string, string>
        {
            ["#000000"] = "Siyah",
            ["#C0C0C0"] = "Gümüş",
            ["#808080"] = "Gri",
            ["#FFFFFF"] = "Beyaz",
            ["#FF0000"] = "Kırmızı",
            ["#800080"] = "Mor",
            ["#008000"] = "Yeşil",
            ["#FFFF00"] = "Sarı",
            ["#FFA500"] = "Turuncu",
            ["#00FF00"] = "Yeşil",
            ["#00FFFF"] = "Açık Mavi",
            ["#0000FF"] = "Mavi",
            ["#000080"] = "Koyu Mavi",
            ["#FFD700"] = "Altın",
        };


        public static string ToColorName(string hexCode)
        {
            ColorMapping.TryGetValue(hexCode, out var name);
            return name;
        }

        public static string ToHexCode(string color)
        {
            return ColorMapping.FirstOrDefault(x => x.Value == color).Key;
        }
    }
}