using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class CollectionItemSerialize : IEntitySerialize
    {
        public string Symbol { get; set; } = null;

        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is CollectionItemEntity;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (CollectionItemEntity)item;
            return (
                item.Entity?.GetType(),
                $"[{cast.Key.ToString()}]"
            );
        }
    }
}
