using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class ArrayItemSerialize : IEntitySerialize
    {
        public string Symbol { get; set; } = null;

        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is ArrayItemEntity;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (ArrayItemEntity)item;
            return (
                item.Entity?.GetType(),
                $"[{string.Join(",", cast.Key)}]"
            );
        }
    }
}
