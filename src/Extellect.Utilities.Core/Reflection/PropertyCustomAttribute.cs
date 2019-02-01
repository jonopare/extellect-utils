using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Reflection
{
    /// <summary>
    /// A data structure that hold a property and its 
    /// associated custom attribute of the specified type.
    /// </summary>
    public class PropertyCustomAttribute<TAttribute>
    {
        /// <summary>
        /// Gets the property
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Gets the custom attribute
        /// </summary>
        public TAttribute CustomAttribute { get; private set; }

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public PropertyCustomAttribute(PropertyInfo property, TAttribute customAttribute)
        {
            Property = property;
            CustomAttribute = customAttribute;
        }
    }
}
