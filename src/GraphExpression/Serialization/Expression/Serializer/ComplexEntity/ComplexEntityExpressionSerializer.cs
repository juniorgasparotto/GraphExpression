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
            EncloseItem = true;
            ShowType = ShowTypeOptions.TypeNameOnlyInRoot;
            ValueFormatter = new DefaultValueFormatter();
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
            string strSymbol = null;
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
                strSymbol = itemSerialize.Symbol;
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
                parts = $"{strType}.{strContainer}";            
            else if (string.IsNullOrWhiteSpace(strType) && !string.IsNullOrWhiteSpace(strContainer))
                parts = $"{strContainer}";

            // Get value
            strValue = ValueFormatter.Format(type, item.Entity, true);

            // When is not primitive entity use hashcode
            var separatorValue = ": ";
            if (strValue == null && item.Entity != null)
            {
                strValue = item.Entity.GetHashCode().ToString();
                separatorValue = ".";
            }
            else if (strValue == null)
            {
                strValue = "null";
            }

            if (string.IsNullOrWhiteSpace(strSymbol)
                && string.IsNullOrWhiteSpace(parts))
                separatorValue = null;

            return $"{strSymbol}{parts}{separatorValue}{strValue}";
        }
    }
}
