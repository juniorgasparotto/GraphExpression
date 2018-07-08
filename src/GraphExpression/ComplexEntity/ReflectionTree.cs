using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphExpression
{
    public static class ExpressionComplexExtensions
    {
        public static Expression<ComplexEntity> AsExpression(this object entityRoot)
        {
            var _root = new ComplexEntity(entityRoot);
            var expression = new Expression<ComplexEntity>(_root, e => GetChildren(e));
            return expression;
        }

        private static IEnumerable<ComplexEntity> GetChildren(ComplexEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            foreach(var p in properties)
                yield return new PropertyEntity(entity, p);

            var fields = entity.GetType().GetFields();
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
