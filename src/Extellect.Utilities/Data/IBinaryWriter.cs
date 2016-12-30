#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Data
{
    public interface IBinaryWriter<T>
    {
        void Write(T item);
    }
}
