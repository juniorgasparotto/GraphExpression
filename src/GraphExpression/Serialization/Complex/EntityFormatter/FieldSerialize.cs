using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class FieldSerialize : IEntitySerialize
    {
        public bool CanSerialize(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            return item is FieldEntity;
        }

        public (Type Type, string Symbol, string ContainerName) GetSerializeInfo(SerializationAsComplexExpression serializer, EntityItem<object> item)
        {
            var cast = (FieldEntity)item;
            return (
                cast.Field.FieldType, 
                serializer.FieldSymbol,
                cast.Field.Name
            );
        }
    }
}
