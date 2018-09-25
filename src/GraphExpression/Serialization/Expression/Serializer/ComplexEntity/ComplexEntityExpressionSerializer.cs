using GraphExpression.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression.Serialization
{
    public partial class ComplexEntityExpressionSerializer : ExpressionSerializerBase<object>
    {
        public ShowTypeOptions ShowType { get; set; }
        public List<IEntitySerialize> ItemsSerialize { get; private set; }

        public ComplexEntityExpressionSerializer(Expression<object> expression)
            : base(expression)
        {   
            ShowType = ShowTypeOptions.TypeNameOnlyInRoot;
            ValueFormatter = new DefaultValueFormatter();
            ForceQuoteEvenWhenValidIdentified = true;
            ItemsSerialize = new List<IEntitySerialize>()
            {
                new ObjectSerialize(),
                new PropertySerialize(),
                new FieldSerialize(),
                new ArrayItemSerialize(),
                new DynamicItemSerialize(),
                new CollectionItemSerialize(),
            };
        }

        public override string SerializeItem(EntityItem<object> item)
        {
            string parts = null;
            string strType = null;
            string strContainer = null;
            string strValue = null;
            Type type = null;

            var itemSerialize = ItemsSerialize
                .Where(f => f.CanSerialize(this, item))
                .LastOrDefault();

            if (itemSerialize != null)
            {
                var info = itemSerialize.GetSerializeInfo(this, item);
                type = info.Type;
                strContainer = info.ContainerName;
            }

            if (type != null)
            {
                if (ShowType == ShowTypeOptions.TypeNameOnlyInRoot && item.GetType() == typeof(ComplexEntity))
                    strType = type.Name;
                else if (ShowType == ShowTypeOptions.TypeName)
                    strType = type.Name;
                else if (ShowType == ShowTypeOptions.FullTypeName)
                    strType = type.FullName;
            }

            if (!string.IsNullOrWhiteSpace(strType) && string.IsNullOrWhiteSpace(strContainer))
                parts = $"{strType}";
            else if (!string.IsNullOrWhiteSpace(strType) && !string.IsNullOrWhiteSpace(strContainer))
                parts = $"{strType}{Constants.IDENTIFIER_SEPARATOR}{strContainer}";            
            else if (string.IsNullOrWhiteSpace(strType) && !string.IsNullOrWhiteSpace(strContainer))
                parts = $"{strContainer}";

            // Get value
            strValue = ValueFormatter.Format(type, item.Entity);

            // When is not primitive entity use hashcode
            var separatorValue = $"{Constants.KEY_VALUE_SEPARATOR} ";
            if (strValue == null && item.Entity != null)
            {
                strValue = item.Entity.GetHashCode().ToString();
                separatorValue = Constants.IDENTIFIER_SEPARATOR;
            }
            else if (strValue == null)
            {
                strValue = Constants.NULL_VALUE;
            }

            if (string.IsNullOrWhiteSpace(parts))
                separatorValue = null;

            return $"{parts}{separatorValue}{strValue}";
        }
    }
}
