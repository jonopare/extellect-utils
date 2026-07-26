namespace Extellect.Nuvo
{
    public class ListInternPoolTests
    {
        [Fact]
        public void Everything()
        {
#pragma warning disable CS0612, CS0618
            var sut = new ListInternPool<string>(128);
#pragma warning restore CS0612, CS0618

            Assert.Equal(0, sut.Count);

            var one = sut.Intern(Copy("One"));
            var two = sut.Intern(Copy("Two"));

            Assert.Equal(2, sut.Count);

            var oneAgain = sut.Intern(Copy("One"));
            var twoAgain = sut.Intern(Copy("Two"));

            Assert.Equal(2, sut.Count);

            Assert.True(ReferenceEquals(one, oneAgain));
            Assert.True(ReferenceEquals(two, twoAgain));

            Assert.False(sut.IsInterned("One"));
            Assert.False(sut.IsInterned("Two"));
            Assert.True(sut.IsInterned(one));
            Assert.True(sut.IsInterned(two));
        }

        [Fact]
        public void Everything_NonDefaultComparer()
        {
#pragma warning disable CS0612, CS0618
            var sut = new ListInternPool<string>(128, StringComparer.OrdinalIgnoreCase);
#pragma warning restore CS0612, CS0618

            Assert.Equal(0, sut.Count);

            var one = sut.Intern(Copy("One"));
            var two = sut.Intern(Copy("Two"));

            Assert.Equal(2, sut.Count);

            var oneAgain = sut.Intern(Copy("one"));
            var twoAgain = sut.Intern(Copy("two"));

            Assert.Equal(2, sut.Count);

            Assert.True(ReferenceEquals(one, oneAgain));
            Assert.True(ReferenceEquals(two, twoAgain));

            Assert.False(sut.IsInterned("One"));
            Assert.False(sut.IsInterned("Two"));
            Assert.False(sut.IsInterned("one"));
            Assert.False(sut.IsInterned("two"));
            Assert.True(sut.IsInterned(one));
            Assert.True(sut.IsInterned(two));
        }

        private static string Copy(string value)
        {
            return new string(value);
        }
    }
}
