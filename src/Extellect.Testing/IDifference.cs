﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Testing
{
    public interface IDifference
    {
        string Expected { get; }
        string Actual { get; }
    }
}
