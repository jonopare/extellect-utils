namespace Extellect.Nuvo
{
    public class OnePageTests
    {
        [Theory]
        [InlineData(1, 100, 0, 10, 1, 10)]
        [InlineData(1, 99, 9, 10, 91, 9)]
        [InlineData(1, 9, 9, 10, -1, 0)]
        public void TakePage_WithOptions_OnlyReturnsRelevantItems(int start, int itemCount, int pageIndex, int pageSize, int firstItem, int numberOfItemsOnPage)
        {
            var page = Enumerable.Range(start, itemCount).TakePage(new OnePageOptions(pageIndex, pageSize));
            Assert.Equal(Enumerable.Range(firstItem, numberOfItemsOnPage), page.Items);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(itemCount, page.ItemCount);
        }

        [Theory]
        [InlineData(1, 100, 0, 10, 1, 10)]
        [InlineData(1, 99, 9, 10, 91, 9)]
        [InlineData(1, 9, 9, 10, -1, 0)]
        public void TakePage_WithPageIndexAndPageSize_OnlyReturnsRelevantItems(int start, int itemCount, int pageIndex, int pageSize, int firstItem, int numberOfItemsOnPage)
        {
            var page = Enumerable.Range(start, itemCount).TakePage(pageIndex, pageSize);
            Assert.Equal(Enumerable.Range(firstItem, numberOfItemsOnPage), page.Items);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(itemCount, page.ItemCount);
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(99, 99)]
        [InlineData(100, 0)]
        public void AsPage_WithoutOptions_ReturnsEveryItemOnOnePage(int start, int itemCount)
        {
            var page = Enumerable.Range(start, itemCount).AsPage();
            Assert.Equal(Enumerable.Range(start, itemCount), page.Items);
            Assert.Equal(0, page.PageIndex);
            Assert.Equal(itemCount, page.PageSize);
            Assert.Equal(itemCount, page.ItemCount);
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(99, 99)]
        [InlineData(100, 0)]
        public void Create_WithoutOptions_ReturnsEveryItemOnOnePage(int start, int itemCount)
        {
            var page = OnePage.Create(Enumerable.Range(start, itemCount));
            Assert.Equal(Enumerable.Range(start, itemCount), page.Items);
            Assert.Equal(0, page.PageIndex);
            Assert.Equal(itemCount, page.PageSize);
            Assert.Equal(itemCount, page.ItemCount);
        }

        [Fact]
        public void Equals_BothPagesHaveSameItems_ReturnsTrue()
        {
            var left = new string[]
            {
                "Item1",
                "Item2",
            }.AsPage();

            var right = new string[]
            {
                "Item1",
                "Item2",
            }.AsPage();

            var actual = left.Equals(right);

            Assert.True(actual);
        }

        [Fact]
        public void Equals_BothPagesHaveSameItemsInDifferentOrder_ReturnsFalse()
        {
            var left = new string[]
            {
                "Item2",
                "Item1",
            }.AsPage();

            var right = new string[]
            {
                "Item1",
                "Item2",
            }.AsPage();

            var actual = left.Equals(right);

            Assert.False(actual);
        }

        [Fact]
        public void Equals_RightPageHasNullItems_ReturnsFalse()
        {
            var left = new OnePage<string>(0, 2, [], 2);

            var right = new string[]
            {
                "Item1",
                "Item2",
            }.AsPage();

            var actual = left.Equals(right);

            Assert.False(actual);
        }

        [Fact]
        public void Equals_LeftPageHasNullItems_ReturnsFalse()
        {
            var left = new string[]
            {
                "Item2",
                "Item1",
            }.AsPage();

            var right = new OnePage<string>(0, 2, [], 2);

            var actual = left.Equals(right);

            Assert.False(actual);
        }
    }
}