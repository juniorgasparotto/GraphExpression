using System;

namespace GraphExpression.Serialization
{
    public interface IEntitySerialize
    {
        bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item);
        (Type Type, string Symbol, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item);
    }
}
