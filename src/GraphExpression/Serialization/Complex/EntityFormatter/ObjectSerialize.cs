using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class ObjectSerialize : IEntitySerialize
    {
        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return true;
        }

        public (Type Type, string Symbol, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return (
                item.Entity?.GetType(),
                null,
                null
            );
        }
    }
}
