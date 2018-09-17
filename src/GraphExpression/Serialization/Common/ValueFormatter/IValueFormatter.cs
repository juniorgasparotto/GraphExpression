using System;

namespace GraphExpression.Serialization
{
    public interface IValueFormatter
    {
        string Format(Type type, object value, bool trimQuotesIfNonSpaces = false);
    }
}
