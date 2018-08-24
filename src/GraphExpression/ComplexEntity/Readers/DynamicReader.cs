using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public class DynamicReader : IReader
    {
        public bool CanRead(object entity)
        {
            return entity is System.Dynamic.ExpandoObject;
        }

        public IEnumerable<ComplexEntity> GetValues(Expression<object> expression, object entity)
        {
            var dyn = (System.Collections.IEnumerable)entity;
            foreach (KeyValuePair<string, object> item in dyn)
                yield return new DynamicItemEntity(expression, item.Key, item.Value);
        }
    }
}