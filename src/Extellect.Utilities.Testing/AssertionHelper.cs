using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Extellect.Utilities.Testing
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

        public static void AreSequencesEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            AreSequencesEqual(expected, actual, CompareValue);
        }

        public static void AreSequencesEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, Func<object, object, IEnumerable<IDifference>> equals)
        {
            using (var actualEnumerator = actual.GetEnumerator())
            using (var expectedEnumerator = expected.GetEnumerator())
            {
                var actualMoveNext = actualEnumerator.MoveNext();
                var expectedMoveNext = expectedEnumerator.MoveNext();

                var i = 0;

                while (actualMoveNext && actualMoveNext == expectedMoveNext)
                {
                    var firstDifference = equals(actualEnumerator.Current, expectedEnumerator.Current).FirstOrDefault();
                    if (firstDifference != null)
                    {
                        Assert.Fail($"Sequences differ at index {i}. Actual: {firstDifference.Actual}. Expected: {firstDifference.Expected}");
                    }

                    actualMoveNext = actualEnumerator.MoveNext();
                    expectedMoveNext = expectedEnumerator.MoveNext();

                    i++;
                }

                if (actualMoveNext != expectedMoveNext)
                {
                    Assert.Fail($"Sequences of different length. Unexpected end after {i} element(s). Actual sequence was {(expectedMoveNext ? "shorter" : "longer")} than expected.");
                }
            }
        }
        public static void AssertAreSetsEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            IEnumerable<T> differences;
            if ((differences = expected.Except(actual)).Any())
            {
                Assert.Fail($"Sets differ. Expected {differences.First()} but didn't find it.");
            }
            else if ((differences = actual.Except(expected)).Any())
            {
                Assert.Fail($"Sets differ. Found {differences.First()} but wasn't expecting it.");
            }
        }

        public static void AssertAreAllEqual<T>(T expected, IEnumerable<T> actuals)
        {
            foreach (var actual in actuals)
            {
                Assert.AreEqual(expected, actual);
            }
        }

        public static void AppendAssertionFailure(StringBuilder assertionFailures, string typeName, string propertyName, object identifier, object expected, object actual)
        {
            if (!Equals(expected, actual))
            {
                if (assertionFailures.Length > 0)
                    assertionFailures.AppendLine();
                assertionFailures.AppendFormat("{0}.{1}[{2}] : Expected:<{3}>. Actual<{4}>.", typeName, propertyName, identifier, ToString(expected), ToString(actual));
            }
        }

        internal static object ToString(object value)
        {
            return value ?? "(null)";
        }

        public static IEnumerable<IDifference> ComparePropertyValues(object expected, object actual)
        {
            var type = expected.GetType();
            if (type != actual.GetType())
            {
                yield return new TypeDifference(type, actual.GetType());
            }
            else
            {
                foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty))
                {
                    var e = propertyInfo.GetValue(expected);
                    var a = propertyInfo.GetValue(actual);
                    if (!Equals(e, a))
                    {
                        yield return new PropertyDifference<object>(propertyInfo.Name, e, a);
                    }
                }
            }
        }

        public static IEnumerable<IDifference> CompareValue(object expected, object actual)
        {
            if (!Equals(expected, actual))
            {
                yield return new ValueDifference(expected, actual);
            }
        }
    }
}
