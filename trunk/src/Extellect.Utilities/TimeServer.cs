using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities
{
    public class TimeServer
    {
        public static TimeServer Current { get; set; }

        static TimeServer()
        {
            Current = new TimeServer();
        }

        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        public DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public DateTime Today
        {
            get
            {
                return DateTime.Today;
            }
        }
    }
}
