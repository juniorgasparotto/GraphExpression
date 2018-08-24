using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public class DictionaryReader : IReader
    {
        public bool CanRead(object entity)
        {
            return entity is System.Collections.IDictionary;
        }

        public IEnumerable<ComplexEntity> GetValues(Expression<object> expression, object entity)
        {
            var dic = (System.Collections.IDictionary)entity;
            foreach (var key in dic.Keys)
                yield return new DictionaryItemEntity(expression, key, dic[key]);
        }
    }
}