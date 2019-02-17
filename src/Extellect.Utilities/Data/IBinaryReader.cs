#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Data
{
    public interface IBinaryReader<T>
    {
        T Read();
    }
}
