using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.CLI
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : Attribute
    {
        public string Name { get; set; }
        public string Default { get; set; }
        public bool Required { get; set; }
    }
}
