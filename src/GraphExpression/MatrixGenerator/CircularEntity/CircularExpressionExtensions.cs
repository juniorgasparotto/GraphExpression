using GraphExpression.Serialization;
using System;
using System.Collections.Generic;

namespace GraphExpression
{
    public static class CircularExpressionExtensions
    {
        public static Expression<T> AsExpression<T>(this T obj, Func<T, IEnumerable<T>> childrenCallback, bool deep = false)
        {
            var expression = new Expression<T>(obj, childrenCallback, deep);
            expression.DefaultSerializer = new SerializationAsExpression<T>(expression);
            return expression;
        }
    }
}
