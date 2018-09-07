#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math
{
    /// <summary>
    /// I've yet to figure out if this is helpful or not.
    /// Surely it's better to simply use syntax like this in the caller:
    ///     Expression&lt;Func&lt;double, double&gt;&gt; expr = x => System.Math.Cos(x);
    /// </summary>
    public static class MathExpression
    {
        public static MethodCallExpression Cos(Expression value)
        {
            return Expression.Call(typeof(System.Math).GetMethod(nameof(System.Math.Cos)), value);
        }

        public static MethodCallExpression Sin(Expression value)
        {
            return Expression.Call(typeof(System.Math).GetMethod(nameof(System.Math.Sin)), value);
        }
    }
}
