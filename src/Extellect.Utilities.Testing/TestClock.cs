using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities
{
    public class TestClock : IClock
    {
        private DateTime utcNow;

        public TestClock()
            : this(DateTime.UtcNow)
        {
        }

        public TestClock(DateTime value)
        {
            SetTime(value);
        }

        private void SetTime(DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Unspecified:
                case DateTimeKind.Local:
                    utcNow = value.ToUniversalTime();
                    break;
                case DateTimeKind.Utc:
                    utcNow = value;
                    break;
            }
        }

        public void Tick(TimeSpan duration)
        {
            utcNow = utcNow.Add(duration);
        }

        public void Tick()
        {
            Tick(TimeSpan.FromSeconds(1));
        }

        public DateTime UtcNow
        {
            get
            {
                return utcNow;
            }
            set
            {
                SetTime(value);
            }
        }

        public DateTime Now
        {
            get
            {
                return utcNow.ToLocalTime();
            }
            set
            {
                SetTime(value);
            }
        }
    }
}
