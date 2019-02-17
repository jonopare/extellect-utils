#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Sequencing
{
    public interface IIterable<T>
    {
        T Next();
    }
}
