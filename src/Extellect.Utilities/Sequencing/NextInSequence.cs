#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Sequencing
{
    public delegate T NextInSequence<T>(T t) where T : IComparable<T>;
}