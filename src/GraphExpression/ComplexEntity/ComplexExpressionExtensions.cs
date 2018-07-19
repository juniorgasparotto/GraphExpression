using System;
using System.Collections.Generic;

namespace GraphExpression
{
    public static class ComplexExpressionExtensions
    {
        public static Expression<object> AsExpression(this object entityRoot)
        {
            var expression = new Expression<object>(expr => new ComplexEntity(expr) { Entity = entityRoot }, (expr, e) => GetChildren(expr, e));
            return expression;
        }

        private static IEnumerable<EntityItem<object>> GetChildren(Expression<object> expression, EntityItem<object> parent)
        {
            var entityParent = parent.Entity;

            if (IsSystemType(entityParent.GetType()))
                yield break;

            var properties = entityParent.GetType().GetProperties();
            foreach(var p in properties)
                yield return new PropertyEntity(expression, entityParent, p);

            var fields = entityParent.GetType().GetFields();
            foreach (var f in fields)
                yield return new FieldEntity(expression, entityParent, f);
        }

        private static bool IsSystemType(Type type)
        {
            var typeName = type.Namespace ?? "";
            if (typeName == "System" || typeName.StartsWith("System.") ||
                typeName == "Microsoft" || typeName.StartsWith("Microsoft."))
                return true;
            return false;
        }
    }
}
