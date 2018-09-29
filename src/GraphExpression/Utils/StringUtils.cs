using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace GraphExpression.Utils
{
    internal static class StringUtils
    {
        public static string RemoveQuotes(string value, char quote)
        {
            // minimun: '' or ""
            if (value.Length >= 2 && value.StartsWith(quote.ToString()))
                return value.Substring(1, value.Length - 2);
            return value;
        }

        public static bool IsNumber(string value)
        {
            if (value != null)
            {
                var count = 0;
                foreach (var c in value)
                {
                    // negative hashcode start with "-"
                    if (count == 0 && c == '-')
                        count++;
                    else if (char.IsDigit(c))
                        count++;
                }
                return count == value.Length;
            }

            return false;
        }
    }
}