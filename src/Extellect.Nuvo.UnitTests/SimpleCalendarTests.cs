namespace Extellect.Nuvo
{
    public class SimpleCalendarTests
    {
        [Theory]
        [InlineData(20151231, 20161231, 12)]
        [InlineData(20151231, 20151130, -1)]
        [InlineData(20151231, 20151231, 0)]
        [InlineData(20151231, 20160131, 1)]
        [InlineData(20151231, 20160229, 2)]
        [InlineData(20151101, 20160301, 4)]
        [InlineData(20151101, 20160331, 5)]
        [InlineData(20151031, 20160401, 5)]
        [InlineData(20151102, 20160402, 5)]
        public void Months_Aligned(int open, int close, int expected)
        {
            var actual = SimpleCalendar.Months(open.ToDate(), close.ToDate());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(20151231, 20161231, 12)]
        [InlineData(20151231, 20151130, -1)]
        [InlineData(20151231, 20151231, 0)]
        [InlineData(20151231, 20160131, 1)]
        [InlineData(20151231, 20160229, 2)]
        [InlineData(20151101, 20160301, 4)]
        [InlineData(20151101, 20160331, 5)]
        [InlineData(20151031, 20160401, 5)]
        [InlineData(20151102, 20160402, 5)]
        public void TryGetMonths_Aligned(int open, int close, int expected)
        {
            Assert.True(SimpleCalendar.TryGetMonths(open.ToDate(), close.ToDate(), out var actual));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(20151230, false)]
        [InlineData(20151231, false)]
        [InlineData(20160101, true)]
        [InlineData(20150102, false)]
        public void IsMonthBegin(int when, bool expected)
        {
            var actual = SimpleCalendar.IsMonthBegin(when.ToDate());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(20151230, false)]
        [InlineData(20151231, true)]
        [InlineData(20160101, false)]
        [InlineData(20160102, false)]
        [InlineData(20160229, true)]
        public void IsMonthEnd(int when, bool expected)
        {
            var actual = SimpleCalendar.IsMonthEnd(when.ToDate(), out DateOnly nextMonthBegin);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1600, true)]
        [InlineData(1700, false)]
        [InlineData(2000, true)]
        [InlineData(2015, false)]
        [InlineData(2016, true)]
        [InlineData(2017, false)]
        public void IsLeapYear(int year, bool expected)
        {
            var actual = SimpleCalendar.IsLeapYear(year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(20200228, -1, 20200128)] // Leap year - not EOM - negative
        [InlineData(20200228, 0, 20200228)] // Leap year - not EOM - zero
        [InlineData(20200228, 1, 20200328)] // Leap year - not EOM - positive
        [InlineData(20200229, -1, 20200131)] // Leap year - EOM - negative
        [InlineData(20200229, 0, 20200229)] // Leap year - EOM - zero
        [InlineData(20200229, 1, 20200331)] // Leap year - EOM - positive
        [InlineData(20210227, -1, 20210127)] // Not leap year - not EOM - negative
        [InlineData(20210227, 0, 20210227)] // Not leap year - not EOM - zero
        [InlineData(20210227, 1, 20210327)] // Not leap year - not EOM - positive
        [InlineData(20210228, -1, 20210131)] // Not leap year - EOM - negative
        [InlineData(20210228, 0, 20210228)] // Not leap year - EOM - zero
        [InlineData(20210228, 1, 20210331)] // Not leap year - EOM - positive
        public void AddMonths(int date, int months, int expected)
        {
            var actual = SimpleCalendar.AddMonths(date.ToDate(), months);
            Assert.Equal(expected.ToDate(), actual);
        }

        [Theory]
        [InlineData(20191230, 20191230)]
        [InlineData(20191231, 20200101)]
        [InlineData(20200101, 20200101)]
        [InlineData(20200102, 20200102)]
        public void MonthEndToNextMonthBegin(int date, int expected)
        {
            var actual = SimpleCalendar.MonthEndToNextMonthBegin(date.ToDate());
            Assert.Equal(expected.ToDate(), actual);
        }

        [Theory]
        [InlineData(20191230, 20191230, true)]
        [InlineData(20191231, 20191230, false)]
        [InlineData(20191231, 20191231, true)]
        [InlineData(20191231, 20200101, true)]
        [InlineData(20200101, 20200101, true)]
        [InlineData(20200102, 20200102, true)]
        [InlineData(20191231, 20200131, false)]
        public void EqualOrShareMonthBoundary(int date1, int date2, bool expected)
        {
            var actual = SimpleCalendar.EqualOrShareMonthBoundary(date1.ToDate(), date2.ToDate());
            Assert.Equal(expected, actual);
            actual = SimpleCalendar.EqualOrShareMonthBoundary(date2.ToDate(), date1.ToDate());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(20181230, 20191231)]
        public void Months_Misaligned_ThrowsMonthBoundaryException(int open, int close)
        {
            Assert.Throws<MonthBoundaryException>(() => SimpleCalendar.Months(open.ToDate(), close.ToDate()));
        }

        [Theory]
        [InlineData(20181230, 20191231, 12)]
        public void Months_MisalignedThumbsuck_ThrowsMonthBoundaryException(int open, int close, int expected)
        {
            var actual = SimpleCalendar.Months(open.ToDate(), close.ToDate(), thumbsuck: true);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(20181230, 20191231, 12)]
        public void TryGetMonths_Misaligned_ReturnsMonthsFromSameDay(int open, int close, int expected)
        {
            Assert.False(SimpleCalendar.TryGetMonths(open.ToDate(), close.ToDate(), out var actual));
            Assert.Equal(expected, actual);
        }
    }
}
