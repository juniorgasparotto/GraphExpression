using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class DynamicItemSerialize : IEntitySerialize
    {
        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is DynamicItemEntity;
        }

        public (Type Type, string Symbol, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (DynamicItemEntity)item;
            return (
                item.Entity?.GetType(), 
                serializer.PropertySymbol,
                cast.Property
            );
        }
    }
}
