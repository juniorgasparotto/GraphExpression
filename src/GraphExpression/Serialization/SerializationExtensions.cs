using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public static class SerializationExtensions
    {
        public static SerializationAsExpression<T> AsSerializer<T>(this Expression<T> expression)
        {
            var serialization = new SerializationAsExpression<T>(expression);
            return serialization;
        }

        public static SerializationAsComplexExpression AsSerializer(this Expression<ComplexEntity> expression)
        {
            var serialization = new SerializationAsComplexExpression(expression);
            return serialization;
        }
    }
}
