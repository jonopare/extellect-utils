﻿namespace Extellect
{
    /// <summary>
    /// Abstracts access to DateTime.Now and DateTime.UtcNow.
    /// Create your own IClock and assign it to Current in order
    /// to allow an application to use a different clock in the
    /// ambient context (but think about the risks first) or 
    /// just inject an IClock dependency into classes that
    /// require one.
    /// </summary>
    public class Clock
    {
        private static IClock _current;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        static Clock()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Default = new DateTimeClock();
            Current = Default;
        }

        /// <summary>
        /// Current clock stored in ambient context (appdomain). 
        /// </summary>
        public static IClock Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _current = value;
            }
        }

        /// <summary>
        /// Gets the default clock which is based on the DateTime class.
        /// </summary>
        public static IClock Default { get; }

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
