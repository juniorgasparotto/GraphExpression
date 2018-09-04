using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class DynamicItemSerialize : IEntitySerialize
    {
        public string Symbol { get; set; } = "@";

        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is DynamicItemEntity;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (DynamicItemEntity)item;
            return (
                item.Entity?.GetType(), 
                cast.Property
            );
        }
    }
}
