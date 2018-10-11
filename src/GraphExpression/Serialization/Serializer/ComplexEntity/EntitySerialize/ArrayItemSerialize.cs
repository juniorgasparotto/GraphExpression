using System;

namespace GraphExpression.Serialization
{
    public class ArrayItemSerialize : IEntitySerialize
    {
        public bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            return item is ArrayItemEntity;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            var cast = (ArrayItemEntity)item;
            return (
                item.Entity?.GetType(),
                $"[{string.Join(",", cast.Indexes)}]"
            );
        }
    }
}
