using System;

namespace Framework.Extensions
{
  public static class DateTimeExtensions
  {

    #region Week of year
    /// <summary>
    /// Weeks the of year.
    /// </summary>
    /// <param name="datetime">The datetime.</param>
    /// <param name="weekRule">The weekRule.</param>
    /// <param name="firstDayOfWeek">The first day of week.</param>
    /// <returns></returns>
    public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekRule, DayOfWeek firstDayOfWeek)
    {
      var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
      return currentCulture.Calendar.GetWeekOfYear(datetime, weekRule, firstDayOfWeek);
    }
    /// <summary>
    /// Weeks the of year.
    /// </summary>
    /// <param name="datetime">The datetime.</param>
    /// <param name="firstDayOfWeek">The first day of week.</param>
    /// <returns></returns>
    public static int WeekOfYear(this DateTime datetime, DayOfWeek firstDayOfWeek)
    {
      var dateTimeFormatInfo = new System.Globalization.DateTimeFormatInfo();
      var weekRule = dateTimeFormatInfo.CalendarWeekRule;
      return WeekOfYear(datetime, weekRule, firstDayOfWeek);
    }
    /// <summary>
    /// Weeks the of year.
    /// </summary>
    /// <param name="datetime">The datetime.</param>
    /// <param name="weekRule">The weekRule.</param>
    /// <returns></returns>
    public static int WeekOfYear(this DateTime datetime, System.Globalization.CalendarWeekRule weekRule)
    {
      var dateTimeFormatInfo = new System.Globalization.DateTimeFormatInfo();
      var firstDayOfWeek = dateTimeFormatInfo.FirstDayOfWeek;
      return WeekOfYear(datetime, weekRule, firstDayOfWeek);
    }
    /// <summary>
    /// Weeks the of year.
    /// </summary>
    /// <param name="datetime">The datetime.</param>
    /// <returns></returns>
    public static int WeekOfYear(this DateTime datetime)
    {
      var dateTimeFormatInfo = new System.Globalization.DateTimeFormatInfo();
      var weekRule = dateTimeFormatInfo.CalendarWeekRule;
      var firstDayOfWeek = dateTimeFormatInfo.FirstDayOfWeek;
      return WeekOfYear(datetime, weekRule, firstDayOfWeek);
    }
    #endregion

    #region Get Datetime for Day of Week
    /// <summary>
    /// Gets the date time for day of week.
    /// </summary>
    /// <param name="datetime">The datetime.</param>
    /// <param name="day">The day.</param>
    /// <param name="firstDayOfWeek">The first day of week.</param>
    /// <returns></returns>
    public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day, DayOfWeek firstDayOfWeek)
    {
      var current = DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek);
      var resultday = DaysFromFirstDayOfWeek(day, firstDayOfWeek);
      return datetime.AddDays(resultday - current);
    }
    public static DateTime GetDateTimeForDayOfWeek(this DateTime datetime, DayOfWeek day)
    {
      var dateTimeFormatInfo = new System.Globalization.DateTimeFormatInfo();
      var firstDayOfWeek = dateTimeFormatInfo.FirstDayOfWeek;
      return GetDateTimeForDayOfWeek(datetime, day, firstDayOfWeek);
    }
    /// <summary>
    /// Firsts the date time of week.
    /// </summary>
    /// <param name="datetime">The datetime.</param>
    /// <returns></returns>
    public static DateTime FirstDateTimeOfWeek(this DateTime datetime)
    {
      var dateTimeFormatInfo = new System.Globalization.DateTimeFormatInfo();
      var firstDayOfWeek = dateTimeFormatInfo.FirstDayOfWeek;
      return FirstDateTimeOfWeek(datetime, firstDayOfWeek);
    }
    /// <summary>
    /// Firsts the date time of week.
    /// </summary>
    /// <param name="datetime">The datetime.</param>
    /// <param name="firstDayOfWeek">The first day of week.</param>
    /// <returns></returns>
    public static DateTime FirstDateTimeOfWeek(this DateTime datetime, DayOfWeek firstDayOfWeek)
    {
      return datetime.AddDays(-DaysFromFirstDayOfWeek(datetime.DayOfWeek, firstDayOfWeek));
    }

    /// <summary>
    /// Dayses from first day of week.
    /// </summary>
    /// <param name="current">The current.</param>
    /// <param name="firstDayOfWeek">The first day of week.</param>
    /// <returns></returns>
    private static int DaysFromFirstDayOfWeek(DayOfWeek current, DayOfWeek firstDayOfWeek)
    {
      //Sunday = 0,Monday = 1,...,Saturday = 6
      var daysbetween = current - firstDayOfWeek;
      if (daysbetween < 0) daysbetween = 7 + daysbetween;
      return daysbetween;
    }
    #endregion

    #region SetTime
    public static DateTime SetTime(this DateTime date, int hour)
    {
      return date.SetTime(hour, 0, 0, 0);
    }
    public static DateTime SetTime(this DateTime date, int hour, int minute)
    {
      return date.SetTime(hour, minute, 0, 0);
    }
    public static DateTime SetTime(this DateTime date, int hour, int minute, int second)
    {
      return date.SetTime(hour, minute, second, 0);
    }
    public static DateTime SetTime(this DateTime date, int hour, int minute, int second, int millisecond)
    {
      return new DateTime(date.Year, date.Month, date.Day, hour, minute, second, millisecond);
    }
    #endregion

    public static DateTime FirstDayOfMonth(this DateTime date)
    {
      return new DateTime(date.Year, date.Month, 1);
    }
    public static DateTime LastDayOfMonth(this DateTime date)
    {
      return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }
    public static DateTime BeginingOfDay(this DateTime date)
    {
      return new DateTime(date.Year, date.Month, date.Day);
    }
    public static DateTime EndOfDay(this DateTime date)
    {
      return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
    }

    public static string ToExtraLongTimeString(this DateTime date)
    {
      return date.ToString("hh:mm:ss.fff");
    }

  }
}