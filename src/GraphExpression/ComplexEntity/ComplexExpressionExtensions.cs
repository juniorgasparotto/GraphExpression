using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

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

            // get all items from collections
            if (entityParent is Array arrayList)
            {
                var list = new List<ArrayItemEntity>();
                ReflectionUtils.IterateArrayMultidimensional(arrayList, indices =>
                {
                    list.Add(new ArrayItemEntity(expression, indices, arrayList.GetValue(indices)));
                });

                foreach (var i in list)
                    yield return i;
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

            // ignore deep in internal classes
            if (IsSystemType(entityParent.GetType()))
                yield break;

            // get all propertis: 
            // 1) ignore indexed (this[...]) with GetIndexParameters > 0
            // 2) ignore properties with only setters
            var properties = entityParent.GetType().GetProperties();
            foreach(var p in properties)
                if (!p.GetIndexParameters().Any() && p.GetGetMethod() != null)
                    yield return new PropertyEntity(expression, entityParent, p);

            // get all fields
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
