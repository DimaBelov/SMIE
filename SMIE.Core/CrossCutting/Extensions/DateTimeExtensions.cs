using System;

namespace SMIE.Core.CrossCutting.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FromUnix(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
