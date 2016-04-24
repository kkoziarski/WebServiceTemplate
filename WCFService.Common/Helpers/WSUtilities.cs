namespace WCFService.Common.Helpers
{
    using System;
    using System.Globalization;

    public class WSUtilities
    {
        public const string DATETIMEFORMAT = "dd-MM-yyyy HH:mm:ss";
        public const string TIMEFORMAT = @"hh\:mm\:ss"; // d\.hh\:mm\:ss

        public static string DateTimeToString(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }
            else
            {
                return date.Value.ToString(DATETIMEFORMAT);
            }
        }

        public static DateTime? StringToDateTime(string strDate)
        {
            DateTime value;
            if (DateTime.TryParseExact(strDate, DATETIMEFORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public static string TimeSpanToString(TimeSpan? time)
        {
            if (!time.HasValue)
            {
                return null;
            }
            else
            {
                return time.Value.ToString(TIMEFORMAT);
            }
        }

        public static TimeSpan? StringToTimeSpan(string strTime)
        {
            TimeSpan value;
            if (TimeSpan.TryParseExact(strTime, TIMEFORMAT, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}