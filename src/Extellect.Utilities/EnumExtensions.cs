using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Extellect.Reflection;

namespace Extellect
{
    /// <summary>
    /// Static class containing helpers and extension methods for Enums. 
    /// Such As; Getting the DescriptionAttribute
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get an Enums System.ComponentModel.DescriptionAttribute
        /// </summary>
        /// <param name="value">The enum value, which has a Description attribute</param>
        /// <returns>The DescriptionAttribute.Description, otherwise if it does not exist, it returns Enum.ToString()</returns>
        public static string Description(this Enum value)
        {
            var memberInfo = value.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault();
            if (memberInfo == null)
            {
                return value.ToString();
            }
            var attribute = memberInfo.GetCustomAttribute<DescriptionAttribute>(false);
            return attribute?.Description ?? value.ToString();
        }
    }
}
