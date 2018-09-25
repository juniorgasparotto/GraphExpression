using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class FieldSerialize : IEntitySerialize
    {
        public bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            return item is FieldEntity;
        }

        public (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            var cast = (FieldEntity)item;
            return (
                cast.Field.FieldType,
                cast.Field.Name
            );
        }
    }
}
