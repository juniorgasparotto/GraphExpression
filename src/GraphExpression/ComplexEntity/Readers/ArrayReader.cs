using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public class ArrayReader : IReader
    {
        public bool CanRead(object entity)
        {
            return entity is Array;
        }

        public IEnumerable<ComplexEntity> GetValues(Expression<object> expression, object entity)
        {
            var arrayList = (Array)entity;
            var list = new List<ArrayItemEntity>();
            ReflectionUtils.IterateArrayMultidimensional(arrayList, indices =>
            {
                list.Add(new ArrayItemEntity(expression, indices, arrayList.GetValue(indices)));
            });

            foreach (var i in list)
                yield return i;
        }
    }
}