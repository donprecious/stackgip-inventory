using System;

namespace StackgipInventory.Shared.Utilities
{
    public static class DateUtility
    {
          public static DateTime GetStartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime GetStartOfMonth(DateTime monthDate)
        {
            
           return new DateTime(monthDate.Year, monthDate.Month, 1);

        }

        public static DateTime GetStartOfDay(DateTime date)
        {
            return date.Date;

        }
        public static DateTime GetEndofDay(DateTime date)
        {
          return  date.Date.AddDays(1).AddTicks(-1);
        }
    }
}
