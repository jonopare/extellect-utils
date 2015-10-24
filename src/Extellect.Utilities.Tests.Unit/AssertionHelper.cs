using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities
{
    public class Assertion
    {
        private bool fail;
        private string message;
        private bool asserted;

        ~Assertion()
        {
            if (!asserted)
                throw new InvalidOperationException("Assert was not called on an Assertion object");
        }

        public void Assert()
        {
            if (fail)
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(message);
            asserted = true;
        }

        public static Assertion AreSequenceEqual<T>(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            using (var actualEnumerator = actual.GetEnumerator())
            using (var expectedEnumerator = expected.GetEnumerator())
            {
                var actualMoveNext = actualEnumerator.MoveNext();
                var expectedMoveNext = expectedEnumerator.MoveNext();

                int i = 0;

                while (actualMoveNext && actualMoveNext == expectedMoveNext)
                {
                    if (!Object.Equals(actualEnumerator.Current, expectedEnumerator.Current))
                    {
                        return new Assertion { fail = true, message = string.Format("Sequences differ at index {0}. Actual: {1}. Expected: {2}", i, actualEnumerator.Current, expectedEnumerator.Current) };
                    }

                    actualMoveNext = actualEnumerator.MoveNext();
                    expectedMoveNext = expectedEnumerator.MoveNext();

                    i++;
                }

                if (actualMoveNext != expectedMoveNext)
                    return new Assertion { fail = true, message = string.Format("Sequences of different length. Unexpected end after {1} element(s). Actual sequence was {0} than expected.", (expectedMoveNext ? "shorter" : "longer"), i) };

                return new Assertion();
            }
        }
    }
}
