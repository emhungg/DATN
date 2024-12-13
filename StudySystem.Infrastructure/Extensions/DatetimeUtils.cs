using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Infrastructure.Extensions
{
    public static class DatetimeUtils
    {
        public static DateTime TimeZoneUTC(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.CreateCustomTimeZone("CustomTimeZone", TimeSpan.FromHours(7), "Custom Time Zone", "Custom Time Zone"));
        }

        public static bool CompareDateYear(int date, int date2)
        {
            if (date == date2) return true;
            return false;
        }
    }
}
