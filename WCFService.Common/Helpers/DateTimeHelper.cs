namespace WCFService.Common.Helpers
{
    using System;

    public static partial class DateTimeHelper
    {

        public static string ToJsonString(this DateTime? datetime)
        {
            return WSUtilities.DateTimeToString(datetime);
        }

        public static string ToJsonString(this DateTime datetime)
        {
            return WSUtilities.DateTimeToString(datetime);
        }

        public static DateTime? ToJsonDateTime(this string strDateTime)
        {
            return WSUtilities.StringToDateTime(strDateTime);
        }

        public static string ToJsonString(this TimeSpan? time)
        {
            return WSUtilities.TimeSpanToString(time);
        }

        public static string ToJsonString(this TimeSpan time)
        {
            return WSUtilities.TimeSpanToString(time);
        }

        public static TimeSpan? ToJsonTimeSpan(this string strTime)
        {
            return WSUtilities.StringToTimeSpan(strTime);  
        }
    }
}