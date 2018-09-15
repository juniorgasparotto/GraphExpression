using System;

namespace GraphExpression.Serialization
{
    public class ObjectSerialize : IEntitySerialize
    {
        public string Symbol { get; set; } = null;

        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return true;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return (
                item.Entity?.GetType(),
                null
            );
        }
    }
}
