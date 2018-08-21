﻿using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public partial class SerializationAsComplexExpression : SerializationAsExpression<object>
    {
        public ShowTypeOptions ShowType { get; set; }
        public IValueFormatter ValueFormatter { get; set; }
        public string PropertySymbol { get; set; }
        public string FieldSymbol { get; set; }
        public bool EncloseItem { get; set; }

        public SerializationAsComplexExpression(Expression<object> expression)
            : base(expression)
        {
            PropertySymbol = "@";
            FieldSymbol = "!";
            EncloseItem = true;
            SerializeItem = SerializeItemInternal;
            ValueFormatter = new DefaultValueFormatter();
        }

        private string SerializeItemInternal(EntityItem<object> item)
        {
            string parts = null;
            string strSymbol = null;
            string strType = null;
            string strContainer = null;
            string strValue = null;
            Type type = null;

            if (item is PropertyEntity prop)
            {
                strSymbol = PropertySymbol;
                type = prop.Property.PropertyType;
                strContainer = prop.Property.Name;
            }
            else if (item is FieldEntity field)
            {
                strSymbol = FieldSymbol;
                type = field.Field.FieldType;
                strContainer = field.Field.Name;
            }
            else if (item is DynamicItemEntity dynItem)
            {
                strSymbol = PropertySymbol;
                type = item.Entity?.GetType();
                strContainer = dynItem.Property;
            }
            else if (item is ArrayItemEntity arrayItem)
            {
                type = item.Entity?.GetType();
                strContainer = $"[{arrayItem.Key.ToString()}]";
            }
            else if (item is DictionaryItemEntity dicItem)
            {
                type = item.Entity?.GetType();
                strContainer = $"[{dicItem.Key.GetHashCode()}]";
            }
            else if (item is ListItemEntity listItem)
            {
                type = item.Entity?.GetType();
                strContainer = $"[{listItem.Key.ToString()}]";
            }
            else
            {
                type = item.Entity?.GetType();
            }

            if (type != null)
            {
                if (ShowType == ShowTypeOptions.TypeName)
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
            strValue = ValueFormatter.Format(type, item.Entity);

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

            var encloseStart = EncloseItem ? "{" : null;
            var encloseEnd = EncloseItem ? "}" : null;

            return $"{encloseStart}{strSymbol}{parts}{separatorValue}{strValue}{encloseEnd}";
        }

        public enum ShowTypeOptions
        {
            None,
            TypeName,
            FullTypeName = 2,
        }
    }
}
