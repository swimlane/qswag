using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSwagGenerator.Misc
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Converts string to the camel case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var arr = value.ToArray();
            arr[0] = char.ToLowerInvariant(arr[0]);
            return new string(arr);
        }
    }
}
