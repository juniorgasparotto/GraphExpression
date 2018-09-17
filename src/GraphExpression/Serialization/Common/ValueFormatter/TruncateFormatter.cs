using System;

namespace GraphExpression.Serialization
{
    public class TruncateFormatter : DefaultValueFormatter
    {
        private readonly int maxLength;

        public TruncateFormatter(int maxLenght)
        {
            this.maxLength = maxLenght;
        }

        public override string Format(Type type, object value, bool addQuotesForNonSpacesValues = true)
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
