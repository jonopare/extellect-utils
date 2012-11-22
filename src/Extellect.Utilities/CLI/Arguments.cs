using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.CLI
{
    public class Arguments : IArguments
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IgnoreUnknown
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IgnoreMissing
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Allow setting of properties that haven't been marked up with an ArgumentAttribute.
        /// </summary>
        public bool IgnoreUnmarked
        {
            get { return false; }
        }
    }
}
