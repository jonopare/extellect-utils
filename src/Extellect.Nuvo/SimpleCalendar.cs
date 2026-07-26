namespace Extellect.Nuvo
{
    public static class SimpleCalendar
    {
        /// <summary>
        /// Gets the whole number of months between two dates, when one of three conditions is true:
        /// 1) Either both dates fall on the same day of the month, or
        /// 2) Both dates fall on the last day of the month.
        /// 3) The earlier date falls on the first day of the month and the later date falls on the last day of the month.
        /// HACK: If thumbsuck is true then both days are treated as the first day of each month.
        /// NOTE: This method can throw a boundary exception if there isn't a whole number of months
        /// between the two dates.
        /// </summary>
        public static int Months(DateOnly open, DateOnly close, bool thumbsuck = false)
        {
            if (TryGetMonths(open, close, out var months) || thumbsuck)
            {
                return months;
            }
            else
            {
                throw new MonthBoundaryException(open, close);
            }
        }

        public static bool TryGetMonths(DateOnly open, DateOnly close, out int months)
        {
            if (IsMonthBeginOrEnd(open, out DateOnly openNextMonthBegin)
                && IsMonthBeginOrEnd(close, out DateOnly closeNextMonthBegin))
            {
                months = MonthsFromSameDay(openNextMonthBegin, closeNextMonthBegin);
                return true;
            }
            else
            {
                months = MonthsFromSameDay(open, close);
                return open.Day == close.Day;
            }
        }

        public static bool IsAligned(DateOnly open, DateOnly close)
        {
            return TryGetMonths(open, close, out var _);
        }

        internal static int MonthsFromSameDay(DateOnly open, DateOnly end)
        {
            return (end.Year - open.Year) * 12 + (end.Month - open.Month);
        }

        public static bool IsMonthBegin(DateOnly value)
        {
            return value.Day == 1;
        }

        public static bool IsMonthEnd(DateOnly value, out DateOnly nextMonthBegin)
        {
            nextMonthBegin = value.AddDays(1);
            return IsMonthBegin(nextMonthBegin);
        }

        public static bool IsMonthEnd(DateOnly value)
        {
            return IsMonthBegin(value.AddDays(1));
        }

        private static bool IsMonthBeginOrEnd(DateOnly value, out DateOnly nextMonthBegin)
        {
            if (IsMonthBegin(value))
            {
                nextMonthBegin = value;
                return true;
            }
            else if (IsMonthEnd(value, out nextMonthBegin))
            {
                return true;
            }
            else
            {
                nextMonthBegin = DateOnly.MinValue;
                return false;
            }
        }

        public static DateOnly MonthEndToNextMonthBegin(DateOnly value)
        {
            if (IsMonthBeginOrEnd(value, out var nextMonthBegin))
                return nextMonthBegin;
            return value;
        }

        public static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        public static DateOnly AddMonths(DateOnly date, int months)
        {
            if (IsMonthEnd(date, out var nextMonthBegin))
            {
                return nextMonthBegin.AddMonths(months).AddDays(-1);
            }
            else
            {
                return date.AddMonths(months);
            }
        }

        public static bool EqualOrShareMonthBoundary(DateOnly date1, DateOnly date2)
        {
            if (date1 == date2)
            {
                return true;
            }
            else if (!IsMonthBeginOrEnd(date1, out var date1MonthBegin))
            {
                return false;
            }
            else if (!IsMonthBeginOrEnd(date2, out var date2MonthBegin))
            {
                return false;
            }
            else
            {
                return date1MonthBegin == date2MonthBegin;
            }
        }

        /// <summary>
        /// Returns a DateTime of the last day of the month.
        /// </summary>
        public static DateOnly GetMonthEnd(DateOnly date)
        {
            int lastDay;
            switch (date.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    lastDay = 31;
                    break;
                case 2:
                    lastDay = IsLeapYear(date.Year) ? 29 : 28;
                    break;
                default:
                    lastDay = 30;
                    break;
            }
            return new DateOnly(date.Year, date.Month, lastDay);
        }

        /// <summary>
        /// Returns the earlier of 2 dates.
        /// </summary>
        public static DateOnly Min(DateOnly date1, DateOnly date2)
        {
            return (date1 <= date2) ? date1 : date2;
        }
    }
}
