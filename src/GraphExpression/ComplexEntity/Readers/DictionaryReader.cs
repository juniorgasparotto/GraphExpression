using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public class DictionaryReader : IComplexItemReader
    {
        public bool CanRead(ComplexBuilder builder, object entity)
        {
            return entity is System.Collections.IDictionary;
        }

        public IEnumerable<ComplexEntity> GetItems(ComplexBuilder builder, Expression<object> expression, object entity)
        {
            var dic = (System.Collections.IDictionary)entity;
            foreach (var key in dic.Keys)
                yield return new DictionaryItemEntity(expression, key, dic[key]);
        }
    }
}