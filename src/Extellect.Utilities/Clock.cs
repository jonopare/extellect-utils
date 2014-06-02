using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities
{
    /// <summary>
    /// Abstracts access to DateTime.Now and DateTime.UtcNow.
    /// Create your own IClock and assign it to Current in order
    /// to allow an application to use a different clock.
    /// </summary>
    public class Clock
    {
        /// <summary>
        /// Default type returned by Current
        /// </summary>
        private class DateTimeClock : IClock
        {
            public DateTime Now
            {
                get { return DateTime.Now; }
            }

            public DateTime UtcNow
            {
                get { return DateTime.UtcNow; }
            }
        }

        private static IClock current;

        /// <summary>
        /// Current clock stored in ambient context (appdomain). 
        /// </summary>
        public static IClock Current
        {
            get
            {
                return current;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                current = value;
            }
        }

        static Clock()
        {
            Current = new DateTimeClock();
        }

        /// <summary>
        /// Gets the current local time.
        /// </summary>
        public static DateTime Now
        {
            get
            {
                return Current.Now;
            }
        }

        /// <summary>
        /// Gets the current UTC time.
        /// </summary>
        public static DateTime UtcNow
        {
            get
            {
                return Current.UtcNow;
            }
        }
    }
}
