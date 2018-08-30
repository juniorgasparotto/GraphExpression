using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class ComplexExpressionExtensions
    {
        public static Expression<object> AsExpression(this object entityRoot, ComplexBuilder builder = null, bool deep = false)
        {
            builder = builder ?? new ComplexBuilder();
            var expression = new Expression<object>(expr => new ComplexEntity(expr, entityRoot), (expr, e) => GetChildren(builder, expr, e), deep);
            expression.DefaultSerializer = new SerializationAsComplexExpression(expression);
            return expression;
        }

        private static IEnumerable<EntityItem<object>> GetChildren(ComplexBuilder builder, Expression<object> expression, EntityItem<object> parent)
        {
            var entityParent = parent?.Entity;

            if (entityParent == null)
                yield break;

            // Find the reader, the last "reader" is the most important
            var instanceReader = builder
                .Readers
                .Where(f => f.CanRead(builder, entityParent))
                .LastOrDefault();

            if (instanceReader != null)
                foreach (var item in instanceReader.GetChildren(builder, expression, entityParent))
                    yield return item;
        }
    }
}
