using System;

namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    public static class DateHelper
    {
        /// <summary>
        /// Treat a date/time as being in the UTC time zone without changing its date
        /// </summary>
        public static DateTime AsUtc(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        /// <summary>
        /// Format a DateTime as a standard ISO 8601 UTC string for a date,
        /// ignoring the time component
        /// </summary>
        public static string ToIso8601Date(this DateTime dateTime)
        {
            return dateTime.Date.AsUtc().ToString("o");
        }

        /// <summary>
        /// Format a DateTime as a standard ISO 8601 UTC string for a date and time,        
        /// </summary>
        public static string ToIso8601DateTime(this DateTime dateTime)
        {
            return dateTime.ToString("o");
        }

        /// <summary>
        /// In wire frames a date label shows up like this:
        /// 5:23pm - 24 MAY, 2017
        /// This Date format shows up like this:
        /// 5:23PM - 24 May, 2017
        /// </summary>
        public static string ToLocalDateTimeLabel(this DateTime utcDateTime)
        {
            return utcDateTime.ToLocalTime().ToString("h:mm tt - d MMM, yyyy");
        }

        /// <summary>
        /// In wire frames a date-only label shows up like this:
        /// 24 MAY, 2017
        /// This Date format shows up like this:
        /// 24 May, 2017
        /// </summary>
        public static string ToDateLabel(this DateTime dateTime)
        {
            return dateTime.ToString("d MMM, yyyy");
        }

        /// <summary>
        /// Try parses string value into local DateTime with formatting
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="stringFormat"></param>
        /// <returns></returns>
        public static string TryParseDateTime(this string dateTimeString, string stringFormat)
        {
            DateTime dateTime;
            if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                return dateTime.ToLocalTime().ToString(stringFormat, AppHelpers.GetCultureInfoSafely());
            }

            return dateTimeString;
        }

        /// <summary>
        /// Try parses string value without local time into DateTime with formatting
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="stringFormat"></param>
        /// <returns></returns>
        public static string TryParseDateTimeWithoutLocalTime(this string dateTimeString, string stringFormat)
        {
            DateTime dateTime;
            if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                return dateTime.ToString(stringFormat, AppHelpers.GetCultureInfoSafely());
            }

            return dateTimeString;
        }
    }
}