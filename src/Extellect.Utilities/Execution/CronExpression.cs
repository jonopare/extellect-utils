#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities
{
    public class CronExpression
    {
        internal class FiringIterator
        {
            private CronExpression _cron;

            private readonly int _minuteIndex;
            private readonly int _hourIndex;
            private readonly int _dayIndex;
            private readonly int _monthIndex;

            private readonly bool _isDayOfMonthRestricted;
            private readonly bool _isDayOfWeekRestricted;

            private int[] _expandedDays;

            private DateTime _startOfMonth;

            public FiringIterator(CronExpression cron, int year, int minuteIndex, int hourIndex, int dayIndex, bool isDayOfMonthRestricted, bool isDayOfWeekRestricted, int monthIndex)
            {
                _cron = cron;

                _isDayOfMonthRestricted = isDayOfMonthRestricted;
                _isDayOfWeekRestricted = isDayOfWeekRestricted;

                _startOfMonth = new DateTime(year, _cron._expandedMonths[monthIndex], 1);
                var dayCount = (_startOfMonth.AddMonths(1) - _startOfMonth).Days;

                var firstDayOfWeekOfMonth = (int)_startOfMonth.DayOfWeek;

                var dayOfWeeksOfMonth = Enumerable.Range(1, dayCount)
                    //.Select(x => new { Day = x, DayOfWeek = (x + 7 - firstDayOfWeekOfMonth) % 7 })
                    .Select(x => new { Day = x, DayOfWeek = (x + firstDayOfWeekOfMonth - 1) % 7 })
                    .Where(x => _cron._expandedDayOfWeeks.Contains(x.DayOfWeek))
                    .Select(x => x.Day);

                _minuteIndex = minuteIndex;
                _hourIndex = hourIndex;
                _dayIndex = dayIndex;
                _monthIndex = monthIndex;

                if ((_isDayOfMonthRestricted && _isDayOfWeekRestricted) || (!_isDayOfMonthRestricted && !_isDayOfWeekRestricted))
                {
                    _expandedDays = _cron._expandedDayOfMonths.Where(x => x <= dayCount).Union(dayOfWeeksOfMonth).OrderBy(x => x).ToArray();
                }
                else if (_isDayOfMonthRestricted)
                {
                    _expandedDays = _cron._expandedDayOfMonths.Where(x => x <= dayCount).OrderBy(x => x).ToArray();
                }
                else if (_isDayOfWeekRestricted)
                {
                    _expandedDays = dayOfWeeksOfMonth.OrderBy(x => x).ToArray();
                }
            }

            public DateTime Current
            {
                get
                {
                    return new DateTime(
                        _startOfMonth.Year,
                        _cron._expandedMonths[_monthIndex],
                        _expandedDays[_dayIndex],
                        _cron._expandedHours[_hourIndex],
                        _cron._expandedMinutes[_minuteIndex],
                        0
                    );
                }
            }

            public FiringIterator Next()
            {
                int minuteIndex = _minuteIndex;
                int hourIndex = _hourIndex;
                int dayIndex = _dayIndex;
                int monthIndex = _monthIndex;
                int year = _startOfMonth.Year;
                
                if (++minuteIndex == _cron._expandedMinutes.Length)
                {
                    minuteIndex = 0;
                    if (++hourIndex == _cron._expandedHours.Length)
                    {
                        hourIndex = 0;
                        if (++dayIndex == _expandedDays.Length)
                        {
                            dayIndex = 0;
                            if (++monthIndex == _cron._expandedMonths.Length)
                            {
                                monthIndex = 0;
                                year++;
                            }
                        }
                    }
                }

                return new FiringIterator(_cron, year, minuteIndex, hourIndex, dayIndex, _isDayOfMonthRestricted, _isDayOfWeekRestricted, monthIndex);
            }
        }

        internal struct Range
        {
            public int Start { get; private set; }
            public int End { get; private set; }

            public Range(int once)
                : this()
            {
                Start = End = once;
            }

            public Range(int start, int end)
                : this()
            {
                Start = start;
                End = end;
            }
        }

        private List<Range> _minutes;
        private List<Range> _hours;
        private List<Range> _dayOfMonths;
        private List<Range> _months;
        private List<Range> _dayOfWeeks;

        private Range _minuteRange = new Range(0, 59);
        private Range _hourRange = new Range(0, 23);
        private Range _dayOfMonthRange = new Range(1, 31);
        private Range _monthRange = new Range(1, 12);
        private Range _dayOfWeekRange = new Range(0, 6);

        private int[] _expandedMinutes;
        private int[] _expandedHours;
        private int[] _expandedDayOfMonths;
        private int[] _expandedMonths;
        private int[] _expandedDayOfWeeks;

        public DateTime MaxValue { get; private set; }

        /// <summary>
        /// If both dayOfMonth or dayOfWeek is restricted (i.e. not *) then either will match.
        /// If only one of dayOfMonth or dayOfWeek is restricted then only that field will match.
        /// If neither dayOfMonth nor dayOfWeek is restricted (i.e. both are *) then all days will match.
        /// </summary>
        /// <param name="minute"></param>
        /// <param name="hour"></param>
        /// <param name="dayOfMonth"></param>
        /// <param name="month"></param>
        /// <param name="dayOfWeek"></param>
        public CronExpression(string minute, string hour, string dayOfMonth, string month, string dayOfWeek)
        {
            if (!TryParseMinute(minute))
                throw new ArgumentException("minute", minute);

            if (!TryParseHour(hour))
                throw new ArgumentException("hour", hour);

            if (!TryParseDayOfMonth(dayOfMonth))
                throw new ArgumentException("dayOfMonth", dayOfMonth);

            if (!TryParseMonth(month))
                throw new ArgumentException("month", month);

            if (!TryParseDayOfWeek(dayOfWeek))
                throw new ArgumentException("dayOfWeek", dayOfWeek);

            if (_dayOfMonths.Count > 0 && _months.Count > 0)
            {
                var maxDayCount = Expand(_months, _monthRange).Select(x => MaxDayCount(x)).Min();
                if (!Expand(_dayOfMonths, _dayOfMonthRange).Any(x => x <= maxDayCount))
                {
                    throw new ArgumentException("invalid schedule detected");
                }
            }

            _expandedMinutes = Expand(_minutes, _minuteRange).Distinct().OrderBy(x => x).ToArray();
            _expandedHours = Expand(_hours, _hourRange).Distinct().OrderBy(x => x).ToArray();
            _expandedDayOfMonths = Expand(_dayOfMonths, _dayOfMonthRange).Distinct().OrderBy(x => x).ToArray();
            _expandedMonths = Expand(_months, _monthRange).Distinct().OrderBy(x => x).ToArray();
            _expandedDayOfWeeks = Expand(_dayOfWeeks, _dayOfWeekRange).Distinct().OrderBy(x => x).ToArray();
        }

        internal static int MaxDayCount(int month, int year)
        {
            return month == 2 ? (IsLeapYear(year) ? 29 : 28) : MaxDayCount(month);
        }

        internal static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        internal static int MaxDayCount(int month)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 2:
                    return 29;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    throw new ArgumentException();
            }
        }

        private bool TryConvert<T>(string value, Func<string, T> convert, out T t)
        {
            try
            {
                t = convert(value);
                return true;
            }
            catch (FormatException)
            {
                t = default(T);
                return false;
            }
        }

        private bool TryParse(string value, Range valueRange, Func<string, int> convert, out List<Range> values)
        {
            values = new List<Range>();

            if (string.IsNullOrEmpty(value) || value == "*")
                return true;

            var parts = value.Split(',');
            foreach (var part in parts)
            {
                var rangeParts = part.Split('-');
                switch (rangeParts.Length)
                {
                    case 1:
                        int once;
                        if (!int.TryParse(rangeParts[0], out once))
                            if (!TryConvert(rangeParts[0], convert, out once))
                                return false;
                        values.Add(new Range(once));
                        break;
                    case 2:
                        int start, end;
                        if (!int.TryParse(rangeParts[0], out start))
                            if (!TryConvert(rangeParts[0], convert, out start))
                                return false;
                        if (start < valueRange.Start || start > valueRange.End)
                            return false;
                        if (!int.TryParse(rangeParts[1], out end))
                            if (!TryConvert(rangeParts[1], convert, out end))
                                return false;
                        if (start < valueRange.Start || start > valueRange.End)
                            return false;
                        values.Add(new Range(start, end));
                        break;
                    default:
                        return false;
                }
            }

            return true;
        }

        internal bool TryParseMinute(string minute)
        {
            return TryParse(minute, _minuteRange, x => { throw new FormatException(); }, out _minutes);
        }

        internal bool TryParseHour(string hour)
        {
            return TryParse(hour, _hourRange, x => { throw new FormatException(); }, out _hours);
        }

        internal bool TryParseDayOfMonth(string dayOfMonth)
        {
            return TryParse(dayOfMonth, _dayOfMonthRange, x => { throw new FormatException(); }, out _dayOfMonths);
        }

        internal bool TryParseMonth(string month)
        {
            return TryParse(month, _monthRange, ConvertMonth, out _months);
        }

        internal bool TryParseDayOfWeek(string dayOfWeek)
        {
            return TryParse(dayOfWeek, _dayOfWeekRange, ConvertDayOfWeek, out _dayOfWeeks);
        }

        internal int ConvertMonth(string month)
        {
            switch (month)
            {
                case "Jan":
                    return 1;
                case "Feb":
                    return 2;
                case "Mar":
                    return 3;
                case "Apr":
                    return 4;
                case "May":
                    return 5;
                case "Jun":
                    return 6;
                case "Jul":
                    return 7;
                case "Aug":
                    return 8;
                case "Sep":
                    return 9;
                case "Oct":
                    return 10;
                case "Nov":
                    return 11;
                case "Dec":
                    return 12;
                default:
                    throw new FormatException();
            }
        }

        internal int ConvertDayOfWeek(string dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case "Sun":
                    return 0;
                case "Mon":
                    return 1;
                case "Tue":
                    return 2;
                case "Wed":
                    return 3;
                case "Thu":
                    return 4;
                case "Fri":
                    return 5;
                case "Sat":
                    return 6;
                default:
                    throw new FormatException();
            }
        }

        public IEnumerable<DateTime> FiringSequence(DateTime start, int maxIterations = 100)
        {
            var minuteIndex = Array.BinarySearch(_expandedMinutes, start.Minute);
            if (minuteIndex < 0)
            {
                minuteIndex = System.Math.Min(~minuteIndex, _expandedMinutes.Length - 1);
            }

            var hourIndex = Array.BinarySearch(_expandedHours, start.Hour);
            if (hourIndex < 0)
            {
                hourIndex = System.Math.Min(~hourIndex, _expandedHours.Length - 1); 
            }

            // TODO: some kind of efficiency with days too?

            var monthIndex = Array.BinarySearch(_expandedMonths, start.Month);
            if (monthIndex < 0)
            {
                monthIndex = System.Math.Min(~monthIndex, _expandedHours.Length - 1);
            }

            // start at the first firing of the year
            var iterator = new FiringIterator(this, start.Year, minuteIndex, hourIndex, 0, _dayOfMonths.Any(), _dayOfWeeks.Any(), monthIndex);

            int i = 0, skipped = 0;
            while (i < maxIterations)
            {
                var current = iterator.Current;
                if (current >= start)
                {
                    if (skipped >= 0)
                    {
                        //Console.WriteLine("Skipped {0} premature firings", skipped);
                        skipped = -1;
                    }

                    yield return current;

                    i++;
                }
                else
                {
                    // seeking forward until we're ready to start returning results
                    skipped++;
                }
                iterator = iterator.Next();
            }
        }

        internal bool HasMonthFlag(DateTime when)
        {
            return !_months.Any() || _months.Any(x => x.Start <= when.Month && x.End >= when.Month);
        }

        internal DateTime NextMonth(DateTime when)
        {
            int month;
            if (!TryGetFirst(_months, _monthRange, x => x > when.Month, out month))
                return new DateTime(when.Year + 1, month, 1);
            else
                return new DateTime(when.Year, month, 1);
        }

        internal bool HasDayOfMonthFlag(DateTime when)
        {
            return !_dayOfMonths.Any() || _dayOfMonths.Any(x => x.Start <= when.Day && x.End >= when.Day);
        }

        internal DateTime NextDayOfMonth(DateTime when)
        {
            int dayOfMonth;
            var overflow = !TryGetFirst(_dayOfMonths, _dayOfMonthRange, x => x > when.Day, out dayOfMonth);
            when = new DateTime(when.Year, when.Month, dayOfMonth);
            if (overflow)
            {
                if (when.Month == 12)
                    when = new DateTime(when.Year + 1, 1, dayOfMonth);
                else
                    when = new DateTime(when.Year, when.Month + 1, dayOfMonth);
            }
            return when;
        }

        internal bool HasDayOfWeekFlag(DateTime when)
        {
            return !_dayOfWeeks.Any() || _dayOfWeeks.Any(x => x.Start <= (int)when.DayOfWeek && x.End >= (int)when.DayOfWeek);
        }

        internal DateTime NextDayOfWeek(DateTime when)
        {
            var dayOfWeek = when.DayOfWeek == DayOfWeek.Saturday ? 0 : (int)when.DayOfWeek;
            int day;
            if (!TryGetFirst(_dayOfWeeks, _dayOfWeekRange, x => x > dayOfWeek, out day))
                throw new NotImplementedException();

            var offset = (day - (int)when.DayOfWeek + 7) % 7;

            return when.Date.AddDays(offset);
        }

        internal bool HasHourFlag(DateTime when)
        {
            return !_hours.Any() || _hours.Any(x => x.Start <= when.Hour && x.End >= when.Hour);
        }

        internal DateTime NextHour(DateTime when)
        {
            int hour;
            var overflow = !TryGetFirst(_hours, _hourRange, x => x > when.Hour, out hour);
            when = new DateTime(when.Year, when.Month, when.Day, hour, 0, 0);
            if (overflow)
                when = when.AddDays(1);
            return when;
        }

        internal bool HasMinuteFlag(DateTime when)
        {
            return !_minutes.Any() || _minutes.Any(x => x.Start <= when.Minute && x.End >= when.Minute);
        }

        internal DateTime NextMinute(DateTime when)
        {
            var m = when.Minute == _minuteRange.End ? _minuteRange.Start : when.Minute;
            
            int minute;
            var overflow = !TryGetFirst(_minutes, _minuteRange, x => x > m, out minute);
            when = new DateTime(when.Year, when.Month, when.Day, when.Hour, minute, 0);
            if (overflow)
            {
                when = when.Add(TimeSpan.FromHours(1));
            }
            return when;
        }

        internal static IEnumerable<int> Expand(IEnumerable<Range> ranges, Range valueRange)
        {
            if (!ranges.Any())
            {
                foreach (var value in Enumerable.Range(valueRange.Start, valueRange.End - valueRange.Start + 1))
                    yield return value;
            }
            else
            {
                foreach (var range in ranges)
                    foreach (var value in Enumerable.Range(range.Start, range.End - range.Start + 1))
                        yield return value;
            }
        }

        /// <summary>
        /// Try return the first item that matched the given predicate.
        /// If no items matched, then return false and assign the first item from the list.
        /// </summary>
        /// <param name="ranges"></param>
        /// <param name="valueRange"></param>
        /// <param name="predicate"></param>
        /// <param name="first"></param>
        /// <returns></returns>
        internal static bool TryGetFirst(IEnumerable<Range> ranges, Range valueRange, Func<int, bool> predicate, out int first)
        {
            using (var e = Expand(ranges, valueRange).OrderBy(x => x).GetEnumerator())
            {
                e.MoveNext();
                if (predicate(first = e.Current))
                {
                    return true;
                }

                while (e.MoveNext())
                {
                    if (predicate(e.Current))
                    {
                        first = e.Current;
                        return true;
                    }
                }

                return false;
            }
        }

        public override string ToString()
        {
            var temp = new StringBuilder();
            foreach (var ranges in new[] { _minutes, _hours, _dayOfMonths, _months, _dayOfWeeks })
            {
                if (temp.Length != 0)
                    temp.Append(" ");

                if (ranges.Count == 0)
                    temp.Append("*");

                for (int r = 0; r < ranges.Count; r++)
                {
                    if (r != 0)
                        temp.Append(",");

                    var range = ranges[r];
                    if (range.Start == range.End)
                    {
                        temp.Append(range.Start);
                    }
                    else
                    {
                        temp.Append(range.Start);
                        temp.Append("-");
                        temp.Append(range.End);
                    }
                }

            }
            return temp.ToString();
        }

    }
}
