using System;

namespace Catalog.Application.Helpers
{
    public static class HelperFunctions
    {
        public static DateTime ConvertTimestampToDateTime(long time)
        {
            time /= 100;
            return DateTimeOffset.FromUnixTimeMilliseconds(time).DateTime;
        }
    }
}