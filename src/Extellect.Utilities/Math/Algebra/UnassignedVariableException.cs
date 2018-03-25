#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities.Math.Algebra
{
    public class UnassignedVariableException : Exception
    {
        public UnassignedVariableException(string name)
            : base($"{name} is an unassigned variable and cannot be evaluated")
        {
        }
    }
}
