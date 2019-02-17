using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Extellect.CLI
{
    /// <summary>
    /// Command line argument parser.
    /// </summary>
    /// <typeparam name="T">The type of the arguments against which this parser is operating.</typeparam>
    public class ArgumentParser<T> where T : IArguments, new()
    {
        /// <summary>
        /// Prints usage information to the specified TextWriter.
        /// </summary>
        public static void Usage(TextWriter writer)
        {
            Usage(writer, true);
        }

        /// <summary>
        /// Prints usage information to the specified TextWriter.
        /// Unmarked attributes are included by default but can be ignored by setting the flag to false.
        /// </summary>
        public static void Usage(TextWriter writer, bool ignoreUnmarked)
        {
            writer.WriteLine("Usage:");
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty))
            {
                ArgumentAttribute attribute = (ArgumentAttribute)property.GetCustomAttributes(typeof(ArgumentAttribute), false).FirstOrDefault();
                if (!ignoreUnmarked || attribute != null)
                {
                    string key = attribute == null || string.IsNullOrEmpty(attribute.Name) ? property.Name : attribute.Name;

                    writer.Write("\t/");
                    writer.Write(key);

                    // default values
                    if (attribute != null)
                    {
                        if (!string.IsNullOrWhiteSpace(attribute.Default))
                        {
                            writer.Write(":");
                            writer.Write(attribute.Default);
                        }
                        if (attribute.Required)
                        {
                            writer.Write(" ** Required **");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

        /////// <summary>
        /////// Tries to parse the specified command line arguments into a C# object with strongly-typed properties.
        /////// </summary>
        ////public static bool TryParse(IEnumerable<string> args, out T options)
        ////{
        ////    try 
        ////    {	    
        ////        options = Parse(args);
        ////        return true;
        ////    }
        ////    catch (ArgumentException)
        ////    {
        ////        // TODO: it would be better if we didn't have to throw and catch this exception.
        ////        options = default(T);
        ////        return false;
        ////    }
        ////}

        /// <summary>
        /// Throws a new ArgumentValidationException with the specified message.
        /// </summary>
        private static void ThrowArgumentValidationException(string message)
        {
            throw new ArgumentValidationException(message);
        }

        /// <summary>
        /// Parses the specified command line arguments into a C# object with strongly-typed properties.
        /// </summary>
        public static T Parse(IEnumerable<string> args)
        {
            return Parse(args, ThrowArgumentValidationException);
        }
        
        /// <summary>
        /// Parses the specified command line arguments into a C# object with strongly-typed properties.
        /// </summary>
        public static T Parse(IEnumerable<string> args, Action<string> validationErrorCallback)
        {
            T arguments = new T();
            var lookup = new Dictionary<string, Tuple<PropertyInfo, ArgumentAttribute, bool>>(StringComparer.OrdinalIgnoreCase); 
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty))
            {
                ArgumentAttribute attribute = (ArgumentAttribute)property.GetCustomAttributes(typeof(ArgumentAttribute), false).FirstOrDefault();
                if (!arguments.IgnoreUnmarked || attribute != null)
                {
                    var key = attribute == null || string.IsNullOrEmpty(attribute.Name) ? property.Name : attribute.Name;
                    
                    var isRequired = false;

                    if (attribute != null)
                    {
                        if (attribute.Default != null)
                        {
                            ConvertAndSetValue(arguments, property, attribute.Default);
                        }
                        else if (attribute.Required && !arguments.IgnoreMissing)
                        { 
                            // it seems inconsistent to add both required and default to
                            // an argument, so the default wins and we only set the the
                            // isRequired flag to verify that the property is set via the
                            // command line args
                            isRequired = true;
                        }
                    }

                    var value = new Tuple<PropertyInfo, ArgumentAttribute, bool>(property, attribute, isRequired);
                    lookup.Add(key, value);
                }
            }

            var required = lookup.Where(p => p.Value.Item3).Select(p => p.Key).ToDictionary(s => s, StringComparer.OrdinalIgnoreCase);

            foreach (var arg in args.Select(a => Parse(a, validationErrorCallback)).Where(p => p.Key != null))
            {
                Tuple<PropertyInfo, ArgumentAttribute, bool> found;
                if (!lookup.TryGetValue(arg.Key, out found) && !arguments.IgnoreUnknown)
                {
                    Unknown(validationErrorCallback, arg.Key, arg.Value);
                }
                ConvertAndSetValue(arguments, found.Item1, arg.Value);

                if (required.ContainsKey(arg.Key))
                {
                    required.Remove(arg.Key);
                }
            }

            foreach (var r in required)
            {
                Required(validationErrorCallback, r.Key);
            }

            return arguments;
        }

        /// <summary>
        /// Raises the validation error callback when an unknown argument is found and is not ignored
        /// </summary>
        internal static void Unknown(Action<string> validationErrorCallback, string name, string value)
        {
            validationErrorCallback($"Unknown argument found: Name = '{name}', Value = '{value}'");
        }

        /// <summary>
        /// Raises the validation error callback when a required argument is not found and no default is specified
        /// </summary>
        internal static void Required(Action<string> validationErrorCallback, string name)
        {
            validationErrorCallback($"Required argument not found: Name = '{name}'");
        }

        /// <summary>
        /// Evaluates the .NET type for a string value given that value's expected type.
        /// </summary>
        internal static void ConvertAndSetValue(T arguments, PropertyInfo property, string value)
        {
            object objectValue;
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                // add to a list (which we create if necessary)
                var targetType = property.PropertyType.GetGenericArguments()[0];
                objectValue = Convert.ChangeType(value, targetType);
                var list = property.GetValue(arguments, null);
                if (list == null)
                {
                    list = Activator.CreateInstance(property.PropertyType);
                    property.SetValue(arguments, list, null);
                }
                var add = property.PropertyType.GetMethod("Add", new [] { targetType });
                add.Invoke(list, new object[] { objectValue });
            }
            else
            {
                // simple property
                if (property.PropertyType.IsEnum)
                {
                    objectValue = Enum.Parse(property.PropertyType, value);
                }
                else
                {
                    objectValue = Convert.ChangeType(value, property.PropertyType);
                }
                property.SetValue(arguments, objectValue, null);
            }
        }

        /// <summary>
        /// Splits an individual argument into a key/value pair.
        /// </summary>
        internal static KeyValuePair<string, string> Parse(string arg, Action<string> validationErrorCallback)
        {
            if (arg.Length > 0)
            {
                int startIndex = (arg[0] == '/' || arg[0] == '-') ? 1 : 0;
                if (arg.Length > startIndex)
                {
                    int endIndex = arg.IndexOfAny(new char[] { ':', '=' }, startIndex);
                    if (endIndex < 0)
                    {
                        validationErrorCallback($"No separator found: {arg}");
                        return default(KeyValuePair<string, string>);
                    }
                    else if (endIndex > startIndex)
                    {
                        string key = arg.Substring(startIndex, endIndex - startIndex);
                        string value = arg.Substring(endIndex + 1);
                        return new KeyValuePair<string, string>(key, value);
                    }
                    else
                    {
                        validationErrorCallback($"No name found: {arg}");
                        return default(KeyValuePair<string, string>);
                    }
                }
                else
                {
                    validationErrorCallback($"No name or separator found: {arg}");
                    return default(KeyValuePair<string, string>);
                }
            }
            else
            {
                validationErrorCallback("Zero length arg found");
                return default(KeyValuePair<string, string>);
            }
        }
    }
}
