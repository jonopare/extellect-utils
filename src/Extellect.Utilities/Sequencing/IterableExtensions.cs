#pragma warning disable 1591
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Sequencing
{
    public static class IterableExtensions
    {
        public static IIterable<T> Create<T>(Func<T> source) => new FuncIterable<T>(source);

        public static IIterable<double> ToIterable(this Random random)
        {
            return Create(random.NextDouble);
        }
    }

}
