using System;
using System.Linq;

namespace LukeApps.Common.Helpers
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateISO(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        public static string ToShortDateLocal(this DateTime datetime)
        {
            return datetime.ToString("dd/MM/yyyy");
        }

        public static string ToShortTime(this DateTime datetime)
        {
            return datetime.ToString("HH:mm");
        }

        public static string FormatUTCDate(this DateTime datetime)
        {
            return datetime.AddHours(4).ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        public static DateTime AddWorkDays(this DateTime date, int workingDays)
        {
            int direction = workingDays < 0 ? -1 : 1;
            DateTime newDate = date;
            while (workingDays != 0)
            {
                newDate = newDate.AddDays(direction);
                if (newDate.DayOfWeek != DayOfWeek.Friday &&
                    newDate.DayOfWeek != DayOfWeek.Saturday &&
                    !newDate.IsHoliday())
                {
                    workingDays -= direction;
                }
            }
            return newDate;
        }

        public static bool IsHoliday(this DateTime date)
        {
            // TODO: load from a DB
            DateTime[] holidays =
            new DateTime[] {
                new DateTime(2019,01,01)
            };

            return holidays.Contains(date.Date);
        }
    }
}