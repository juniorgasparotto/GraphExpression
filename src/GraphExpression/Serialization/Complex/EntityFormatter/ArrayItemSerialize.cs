using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class ArrayItemSerialize : IEntitySerialize
    {
        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is ArrayItemEntity;
        }

        public (Type Type, string Symbol, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (ArrayItemEntity)item;
            return (
                item.Entity?.GetType(), 
                null,
                $"[{string.Join(",", cast.Key)}]"
            );
        }
    }
}
