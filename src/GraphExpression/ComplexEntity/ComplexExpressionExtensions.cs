using System;
using System.Collections.Generic;

namespace GraphExpression
{
    public static class ComplexExpressionExtensions
    {
        public static Expression<object> AsExpression(this object entityRoot)
        {
            Expression<object> expression = null;
            expression = new Expression<object>(entityRoot, e => GetChildren(expression, e));
            return expression;
        }

        private static IEnumerable<object> GetChildren(Expression<object> expression, object parent)
        {
            if (IsSystemType(parent.GetType()))
                yield break;

            var properties = parent.GetType().GetProperties();
            foreach(var p in properties)
                yield return new PropertyEntity(expression, parent, p);

            var fields = parent.GetType().GetFields();
            foreach (var f in fields)
                yield return new FieldEntity(expression, parent, f);
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
