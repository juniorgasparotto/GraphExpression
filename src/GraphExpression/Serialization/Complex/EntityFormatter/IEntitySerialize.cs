using System;

namespace GraphExpression.Serialization
{
    public interface IEntitySerialize
    {
        string Symbol { get; set; }
        bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item);
        (Type Type, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item);
    }
}
