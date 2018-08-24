using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public class ListReader : IReader
    {
        public bool CanRead(object entity)
        {
            return entity is System.Collections.IList;
        }

        public IEnumerable<ComplexEntity> GetValues(Expression<object> expression, object entity)
        {
            var list = (System.Collections.IList)entity;
            for (var i = 0; i < list.Count; i++)
                yield return new ListItemEntity(expression, i, list[i]);
        }
    }
}