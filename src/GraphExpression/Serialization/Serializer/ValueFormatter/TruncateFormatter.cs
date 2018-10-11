using System;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Truncate class that converts an object to string and truncate strings with custom "maxLenght"
    /// </summary>
    public class TruncateFormatter : DefaultValueFormatter
    {
        private readonly int maxLength;

        /// <summary>
        /// Create a truncate formatter
        /// </summary>
        /// <param name="maxLenght"></param>
        public TruncateFormatter(int maxLenght)
        {
            this.maxLength = maxLenght;
        }

        /// <summary>
        /// Format an object for string
        /// </summary>
        /// <param name="type">Object type</param>
        /// <param name="value">Object value</param>
        /// <returns>Object as string</returns>
        public override string Format(Type type, object value)
        {
            if (value is string str)
                return base.Format(type, Truncate(str));
            return base.Format(type, value);
        }

        private string Truncate(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
