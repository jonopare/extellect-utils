using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.CLI
{
    public interface IArguments
    {
        bool IgnoreUnknown { get; }
        bool IgnoreMissing { get; }
        bool IgnoreUnmarked { get; }
    }
}
