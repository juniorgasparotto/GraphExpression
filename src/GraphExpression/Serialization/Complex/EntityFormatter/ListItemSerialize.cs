using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class ListItemSerialize : IEntitySerialize
    {
        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is ListItemEntity;
        }

        public (Type Type, string Symbol, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (ListItemEntity)item;
            return (
                item.Entity?.GetType(), 
                null,
                $"[{cast.Key.ToString()}]"
            );
        }
    }
}
