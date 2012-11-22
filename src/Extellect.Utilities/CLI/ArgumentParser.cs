using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Extellect.Utilities.CLI
{
    public class ArgumentParser<T> where T : IArguments, new()
    {
        public T Parse(string[] args)
        {
            T arguments = new T();
            Dictionary<string, Tuple<PropertyInfo, ArgumentAttribute>> lookup = new Dictionary<string, Tuple<PropertyInfo, ArgumentAttribute>>(StringComparer.OrdinalIgnoreCase); 
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty))
            {
                ArgumentAttribute attribute = (ArgumentAttribute)property.GetCustomAttributes(typeof(ArgumentAttribute), false).FirstOrDefault();
                if (!arguments.IgnoreUnmarked || attribute != null)
                {
                    string key = attribute == null || string.IsNullOrEmpty(attribute.Name) ? property.Name : attribute.Name;
                    lookup.Add(key, new Tuple<PropertyInfo, ArgumentAttribute>(property, attribute));

                    // set default values
                    if (attribute != null && attribute.Default != null)
                    {
                        Evaluate(arguments, property, attribute.Default);
                    }

                    // TODO: required
                }
            }
            foreach (var kvp in args.Select(Parse))
            {
                var found = lookup[kvp.Key];
                Evaluate(arguments, found.Item1, kvp.Value);
            }
            return arguments;
        }

        private void Evaluate(T arguments, PropertyInfo property, string value)
        {
            object objectValue = Convert.ChangeType(value, property.PropertyType);
            property.SetValue(arguments, objectValue, null);
        }

        private KeyValuePair<string, string> Parse(string arg)
        {
            if (arg.Length > 0)
            {
                int startIndex = (arg[0] == '/' || arg[0] == '-') ? 1 : 0;
                if (arg.Length > startIndex)
                {
                    int endIndex = arg.IndexOfAny(new char[] { ':', '=' }, startIndex);
                    if (endIndex > startIndex)
                    {
                        string key = arg.Substring(startIndex, endIndex - startIndex);
                        string value = arg.Substring(endIndex + 1);
                        return new KeyValuePair<string, string>(key, value);
                    }
                }
            }
            throw new ArgumentException();
        }
    }
}
