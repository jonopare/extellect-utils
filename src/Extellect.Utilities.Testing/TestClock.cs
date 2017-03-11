using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities
{
    public class TestClock : IClock
    {
        public DateTime Now { get { return UtcNow.ToLocalTime(); } }

        public DateTime UtcNow { get; set; }
    }
}
