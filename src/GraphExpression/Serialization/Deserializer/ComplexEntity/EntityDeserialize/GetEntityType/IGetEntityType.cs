using System;

namespace GraphExpression.Serialization
{
    public interface IGetEntityType : IEntityDeserialize
    {
        bool CanGetEntityType(ItemDeserializer item);
        Type GetEntityType(ItemDeserializer item);
    }
}