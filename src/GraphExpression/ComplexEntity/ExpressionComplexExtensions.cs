using System;
using System.Collections.Generic;

namespace GraphExpression
{
    public static class ExpressionComplexExtensions
    {
        public static Expression<object, ComplexEntity> AsExpression(this object entityRoot)
        {
            var expression = new Expression<object, ComplexEntity>(entityRoot, e => GetChildren(e));
            return expression;
        }

        private static IEnumerable<object> GetChildren(object parent)
        {
            if (IsSystemType(parent.GetType()))
                yield break;

            var properties = parent.GetType().GetProperties();
            foreach(var p in properties)
                yield return new PropertyEntity(entity, p);

            var fields = parent.GetType().GetFields();
            foreach (var f in fields)
                yield return new FieldEntity(entity, f);
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
