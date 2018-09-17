
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public static class Utils
    {
        public static Expression<object> CreateEmptyExpression()
        {
            var expression = new Expression<object>();
            expression.DefaultSerializer = new ComplexEntityExpressionSerializer(expression);
            return expression;
        }

        public static ComplexEntityExpressionSerializer GetSerialization(Expression<object> expression)
        {
            return (ComplexEntityExpressionSerializer)expression.DefaultSerializer;
        }
    }
}
