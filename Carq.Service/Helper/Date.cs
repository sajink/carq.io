namespace Carq.Data
{
    using System;
    using System.Globalization;

    public static class Date
    {
        public const string MonthFormat = "yyyyMM";
        public const string DayFormat = "yyyyMMdd";
        public const string HourFormat = "yyyyMMddHH";
        public const string MinuteFormat = "yyyyMMddHHmm";
        public const string SecondFormat = "yyyyMMddHHmmss";

        public static string Month(int add = 0) => DateTime.UtcNow.AddMinutes(330).AddMonths(add).ToString(MonthFormat);
        public static string Day(int add = 0) => DateTime.UtcNow.AddMinutes(330).AddDays(add).ToString(DayFormat);
        public static string Hour(int add = 0) => DateTime.UtcNow.AddMinutes(330).AddHours(add).ToString(HourFormat);
        public static string Minute(int add = 0) => DateTime.UtcNow.AddMinutes(330 + add).ToString(MinuteFormat);
        public static string Second(int add = 0) => DateTime.UtcNow.AddMinutes(330).AddSeconds(add).ToString(SecondFormat);

        public static DateTime ToDate(this string date, string format) => DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
        public static double ToDateJs(this string date, string format) => (DateTime.ParseExact(date, format, CultureInfo.InvariantCulture) - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }
}
