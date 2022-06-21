using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipStation.NetSdk.CodeGen
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };

        /// <summary>
        /// Get substring until first occurrence of given character has been found. Returns the whole string if character has not been found.
        /// </summary>
        public static string GetUntil(this string that, char @char)
        {
            return that[..(IndexOf() == -1 ? that.Length : IndexOf())];
            int IndexOf() => that.IndexOf(@char);
        }
    }
}
