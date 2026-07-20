using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FinanceMobile
{
    public static class WeekCalculator
    {
        public static DateTime GetMonday(DateTime date)
        {
            // Убираем время (устанавливаем на 00:00:00)
            DateTime dateWithoutTime = date.Date;

            // Вычисляем разницу между текущим днём и понедельником
            int dayDifference = (int)dateWithoutTime.DayOfWeek - (int)DayOfWeek.Monday;

            // Корректируем отрицательную разницу (например, для воскресенья: 0 - 1 = -1 → +7 = 6)
            if (dayDifference < 0)
            {
                dayDifference += 7;
            }

            // Вычитаем смещение, чтобы получить понедельник
            return dateWithoutTime.AddDays(-dayDifference);
        }

            
        public static DateTime GetFirstMonthDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime GetFirstYearDay(DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime GetFirstQuartalDay(DateTime date)
        {
            return new DateTime(date.Year, ((date.Month - 1) / 3 * 3) + 1, 1);
        }
        
        public static int GetDaysCountInMonth(DateTime d) => DateTime.DaysInMonth(d.Year, d.Month);

        public static int GetDaysCountInQuartal(DateTime d)
        {
            var m1 = GetFirstQuartalDay(d);
            var m2 = new DateTime(m1.Year, m1.Month + 1, 1);
            var m3 = new DateTime(m1.Year, m1.Month + 2, 1);
            return DateTime.DaysInMonth(m1.Year, m1.Month)
            + DateTime.DaysInMonth(m2.Year, m2.Month)
            + DateTime.DaysInMonth(m3.Year, m3.Month);
        }

        public static int GetDaysCountInYear(DateTime d) => DateTime.IsLeapYear(d.Year) ? 366 : 365;

        public static string ToRuRangeString(DateTime start, DateTime end)
        {
            var culture = new CultureInfo("ru-RU");

            // Локальная функция для форматирования одной даты
            string FormatDate(DateTime date, string format)
            {
                string result = date.ToString(format, culture);

                // Примечание: в русской локали месяцы по умолчанию с маленькой буквы ("июля").
                // Если вам строго нужны заглавные буквы (как в примере), делаем первую букву заглавной.
                return result.Substring(0, 3) + char.ToUpper(result[3]) + result.Substring(4);
            }

            // Если годы совпадают, не выводим их вообще
            if (start.Year == end.Year)
            {
                return $"{FormatDate(start, "dd MMMM")} - {FormatDate(end, "dd MMMM")}";
            }

            // Если годы разные, добавляем yyyy для обеих дат
            return $"{FormatDate(start, "dd MMMM yyyy")} - {FormatDate(end, "dd MMMM yyyy")}";
        }
    }
}
