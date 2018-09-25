using System;

namespace GraphExpression.Serialization
{
    public class PropertySerialize : IEntitySerialize
    {
        public bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            return item is PropertyEntity;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            var cast = (PropertyEntity)item;
            return (
                cast.Property.PropertyType,
                cast.Property.Name
            );
        }
    }
}
