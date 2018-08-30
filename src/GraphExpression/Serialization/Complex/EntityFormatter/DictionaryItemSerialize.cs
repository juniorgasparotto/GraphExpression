using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class DictionaryItemSerialize : IEntitySerialize
    {
        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is DictionaryItemEntity;
        }

        public (Type Type, string Symbol, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (DictionaryItemEntity)item;
            return (
                item.Entity?.GetType(), 
                null,
                 $"[{cast.Key.GetHashCode()}]"
            );
        }
    }
}
