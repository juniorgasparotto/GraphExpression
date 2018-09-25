using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class CollectionItemSerialize : IEntitySerialize
    {
        public bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            return item is CollectionItemEntity;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            var cast = (CollectionItemEntity)item;
            return (
                item.Entity?.GetType(),
                $"[{cast.Key.ToString()}]"
            );
        }
    }
}
