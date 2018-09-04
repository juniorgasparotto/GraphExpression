using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class PropertySerialize : IEntitySerialize
    {
        public string Symbol { get; set; } = "@";

        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is PropertyEntity;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (PropertyEntity)item;
            return (
                cast.Property.PropertyType,
                cast.Property.Name
            );
        }
    }
}
