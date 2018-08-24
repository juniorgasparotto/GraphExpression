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

            // find instance reader
            var instanceReader = builder.InstanceReaders.Where(f => f.CanRead(builder, entityParent)).FirstOrDefault();
            if (instanceReader != null)
                foreach (var item in instanceReader.GetItems(builder, expression, entityParent))
                    yield return item;

            // get members
            foreach(var memberReader in builder.MemberReaders)
                if (memberReader.CanRead(builder, entityParent))
                    foreach (var item in memberReader.GetItems(builder, expression, entityParent))
                        yield return item;
        }
    }
}
