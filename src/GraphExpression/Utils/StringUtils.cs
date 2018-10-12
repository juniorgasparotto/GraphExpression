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

        public static string TrimAll(string str)
        {
            return str.Trim().TrimStart('\r', '\n').TrimEnd('\r', '\n');
        }

        public static string Indent(string str, int count)
        {
            var builder = new StringBuilder();
            foreach (var s in str)
            {
                builder.AppendLine("".PadLeft(count) + s);
            }

            return builder.ToString();
        }
    }
}