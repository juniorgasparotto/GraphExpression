using System;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Interface that converts an object to string when possible
    /// </summary>
    public interface IValueFormatter
    {
        /// <summary>
        /// Format an object for string
        /// </summary>
        /// <param name="type">Object type</param>
        /// <param name="value">Object value</param>
        /// <returns>Object as string</returns>
        string Format(Type type, object value);
    }
}
