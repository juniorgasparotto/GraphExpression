using GraphExpression.Utils;
using System;
using System.Collections.Generic;


namespace GraphExpression.Serialization
{
    public class PrimitiveDeserialize : IEntityDeserialize
    {
        public bool CanRead(ItemDeserializer builder, object entity)
        {
            return entity is Array;
        }

        public IEnumerable<ComplexEntity> GetChildren(ComplexBuilder builder, Expression<object> expression, object entity)
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