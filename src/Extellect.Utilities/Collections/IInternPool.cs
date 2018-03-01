using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInternPool
    {
        /// <summary>
        /// 
        /// </summary>
        string Intern(string value);
        /// <summary>
        /// 
        /// </summary>
        bool IsInterned(string value);

        /// <summary>
        /// 
        /// </summary>
        int Count { get; }
    }
}
