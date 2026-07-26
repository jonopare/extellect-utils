using System.ComponentModel.DataAnnotations;

namespace Extellect.Nuvo
{
    public class ReflectionExtensionsTests
    {
        public ReflectionExtensionsTests? Self { get; set; }
        public string? NullableStringRW { get; set; }
        public string NonNullableStringRW { get; set; } = string.Empty;
        [Required]
        public string? RequiredNullableStringRW { get; set; }
        [Required]
        public string RequiredNonNullableStringRW { get; set; } = string.Empty;

        public string? NullableStringRO { get; }
        public string NonNullableStringRO { get; } = string.Empty;
        [Required]
        public string? RequiredNullableStringRO { get; }
        [Required]
        public string RequiredNonNullableStringRO { get; } = string.Empty;

        //public string? NullableStringWO { set; }
        //public string NonNullableStringWO { set; } = string.Empty;
        //[Required]
        //public string? RequiredNullableStringWO { set; }
        //[Required]
        //public string RequiredNonNullableStringWO { set; } = string.Empty;

        [Fact]
        public void GetNullableUnderlyingType_TypeOfNullableInt32_ReturnsInt32()
        {
            Assert.Equal(typeof(int), typeof(int?).GetNullableUnderlyingType());
        }

        [Fact]
        public void GetNullableUnderlyingType_TypeOfInt32_ReturnsInt32()
        {
            Assert.Equal(typeof(int), typeof(int).GetNullableUnderlyingType());
        }

        [Fact]
        public void IsNullable_TypeOfNullableInt32_ReturnsTrue()
        {
            Assert.True(typeof(int?).IsNullable());
        }

        [Fact]
        public void IsNullable_TypeOfInt32_ReturnsFalse()
        {
            Assert.False(typeof(int).IsNullable());
        }

        [Theory]
        [InlineData(nameof(NullableStringRW), true)]
        [InlineData(nameof(NonNullableStringRW), false)]
        [InlineData(nameof(RequiredNullableStringRW), false)]
        [InlineData(nameof(RequiredNonNullableStringRW), false)]
        [InlineData(nameof(NullableStringRO), true)]
        [InlineData(nameof(NonNullableStringRO), false)]
        [InlineData(nameof(RequiredNullableStringRO), false)]
        [InlineData(nameof(RequiredNonNullableStringRO), false)]
        public void IsNullable_Properties(string propertyName, bool expected)
        {
            Assert.Equal(expected, typeof(ReflectionExtensionsTests).GetProperty(propertyName)!.IsNullable());
        }

        [Fact]
        public void NullableCustomType()
        {
            Assert.True(typeof(ReflectionExtensionsTests).GetProperty(nameof(Self))!.IsNullable());
        }

        [Fact]
        public void FindConcreteImplementations()
        {
            Assert.Equal([typeof(IFunc<bool>)], typeof(Jabberwocky).FindConcreteImplementations(typeof(IFunc<>)));
            Assert.Equal([typeof(IFunc<double>)], typeof(Tweedledum).FindConcreteImplementations(typeof(IFunc<>)));
            Assert.Equal([typeof(IFunc<double>), typeof(IFunc<int>)], typeof(Tweedledee).FindConcreteImplementations(typeof(IFunc<>)));
        }

        private interface IFunc<T>
        {
            T Create();
        }

        private struct Jabberwocky : IFunc<bool>
        {
            public bool Create() => true;
        }

        private class Tweedledum : IFunc<double>
        {
            public double Create() => 1.2d;
        }

        private class Tweedledee : Tweedledum, IFunc<int>
        {
            public new int Create() => 1;
        }
    }
}