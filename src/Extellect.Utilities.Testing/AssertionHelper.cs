using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extellect.Utilities
{
    public class AssertionHelper
    {
        public static void ArePropertiesEqual<T>(T expected, T actual, PropertyComparison<T> propertyComparison)
        {
            var result = propertyComparison.AreEqual(expected, actual);
            if (!result.Success)
            {
                Assert.Fail($"Property values differ at {result.PropertyName}. Actual: {result.Actual}. Expected: {result.Expected}");
            }
        }

        public static void ArePropertiesNotEqual<T>(T expected, T actual, PropertyComparison<T> propertyComparison)
        {
            var result = propertyComparison.AreEqual(expected, actual);
            if (result.Success)
            {
                Assert.Fail($"Property values differ at {result.PropertyName}. Actual: {result.Actual}. Expected: {result.Expected}");
            }
        }

        public static void AreSequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            using (var expectedEnumerator = expected.GetEnumerator())
            using (var actualEnumerator = actual.GetEnumerator())
            {
                var expectedMoveNext = expectedEnumerator.MoveNext();
                var actualMoveNext = actualEnumerator.MoveNext();

                int i = 0;

                while (actualMoveNext && actualMoveNext == expectedMoveNext)
                {
                    if (!Object.Equals(actualEnumerator.Current, expectedEnumerator.Current))
                    {
                        Assert.Fail(string.Format("Sequences differ at index {0}. Actual: {1}. Expected: {2}", i, actualEnumerator.Current, expectedEnumerator.Current));
                    }

                    expectedMoveNext = expectedEnumerator.MoveNext();
                    actualMoveNext = actualEnumerator.MoveNext();

                    i++;
                }

                if (actualMoveNext != expectedMoveNext)
                    Assert.Fail(string.Format("Sequences of different length. Unexpected end after {1} element(s). Actual sequence was {0} than expected.", (expectedMoveNext ? "shorter" : "longer"), i));
            }
        }
    }
}
