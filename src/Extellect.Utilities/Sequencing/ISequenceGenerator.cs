using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Sequencing
{
    public interface ISequenceGenerator<T>
    {
        IEnumerable<T> Generate();
    }

}
