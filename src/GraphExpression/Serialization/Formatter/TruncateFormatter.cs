using System;

namespace GraphExpression.Serialization
{
    public class TruncateFormatter : IValueFormatter
    {
        private readonly int maxLenght;

        public TruncateFormatter(int maxLenght)
        {
            this.maxLenght = maxLenght;
        }

        public string Format(Type type, object value)
        {
            if (value is string str)
                return Truncate(str, maxLenght);
            return null;
        }

        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
