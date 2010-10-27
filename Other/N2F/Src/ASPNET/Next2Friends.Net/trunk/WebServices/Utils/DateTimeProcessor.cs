using System;

namespace Next2Friends.WebServices.Utils
{
    public static class DateTimeProcessor
    {
        public static String ToTicksString(this DateTime dateTime)
        {
            return dateTime.Ticks.ToString();
        }

        public static DateTime? FromTicksString(this String ticksString)
        {
            if (String.IsNullOrEmpty(ticksString))
                return null;
            return new DateTime(Convert.ToInt64(ticksString));
        }
    }
}
