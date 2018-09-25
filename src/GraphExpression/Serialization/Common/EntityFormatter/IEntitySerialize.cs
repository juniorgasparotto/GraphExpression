using System;

namespace GraphExpression.Serialization
{
    public interface IEntitySerialize
    {
        bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item);
        (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item);
    }
}
