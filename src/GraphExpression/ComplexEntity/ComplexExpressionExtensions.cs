using GraphExpression.Serialization;
using System;
using System.Collections.Generic;

namespace GraphExpression
{
    public static class ComplexExpressionExtensions
    {
        public static Expression<object> AsExpression(this object entityRoot)
        {
            var expression = new Expression<object>(expr => new ComplexEntity(expr, entityRoot), (expr, e) => GetChildren(expr, e));
            expression.DefaultSerializer = new SerializationAsComplexExpression(expression);
            return expression;
        }

        private static IEnumerable<EntityItem<object>> GetChildren(Expression<object> expression, EntityItem<object> parent)
        {
            var entityParent = parent?.Entity;

            if (entityParent == null)
                yield break;

            if (entityParent is Array arrayList)
            {
                for(var i = 0; i < arrayList.Length; i++)
                    yield return new ArrayItemEntity(expression, i, arrayList.GetValue(i));
            }
            else if (entityParent is System.Dynamic.ExpandoObject)
            {
                var dyn = (System.Collections.IEnumerable)entityParent;
                foreach (KeyValuePair<string, object> item in dyn)
                    yield return new DynamicItemEntity(expression, item.Key, item.Value);
            }
            else if (entityParent is System.Collections.IDictionary dic)
            {
                foreach (var key in dic.Keys)
                    yield return new DictionaryItemEntity(expression, key, dic[key]);
            }
            else if (entityParent is System.Collections.IList list)
            {
                for (var i = 0; i < list.Count; i++)
                    yield return new ListItemEntity(expression, i, list[i]);
            }

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
