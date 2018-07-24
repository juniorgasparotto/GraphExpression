using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class SerializationAsComplexExpression : SerializationAsExpression<object>
    {
        public IncludePartsEnum IncludeParts { get; set; }
        public string PropertySymbol { get; set; }
        public string FieldSymbol { get; set; }

        public SerializationAsComplexExpression(Expression<object> expression)
            : base(expression)
        {
            PropertySymbol = "@";
            FieldSymbol = "!";
            SerializeItem = SerializeItemInternal;
        }

        private string SerializeItemInternal(EntityItem<object> item)
        {
            string strEntityType = null;
            string strType = null;
            string strContainer = null;
            string strValue = null;
            Type type = null;

            if (item is PropertyEntity prop)
            {
                strEntityType = PropertySymbol;
                type = prop.Property.PropertyType;
                strContainer = prop.Property.Name;
            }
            else if (item is FieldEntity field)
            {
                strEntityType = FieldSymbol;
                type = field.Field.FieldType;
                strContainer = field.Field.Name;
            }
            else
            {
                strEntityType = "";
                type = item.Entity.GetType();
                strContainer = null;
            }

            if (IncludeParts.HasFlag(IncludePartsEnum.TypeName))
                strType = type.Name;
            else if (IncludeParts.HasFlag(IncludePartsEnum.FullTypeName))
                strType = type.FullName;

            if (IncludeParts.HasFlag(IncludePartsEnum.Value))
                strValue = ToLiteral(item.Entity);

            var output = strEntityType + strType;
            output += strContainer == null ? "" : "." + strContainer;

            if (strValue != null)
                output += ":" + strValue;
            else if (item.Entity != null) // When is not primitive entity use hashcode
                output += "." + item.Entity.GetHashCode();
            else
                output += ": null";

            return output;
        }

        private string ToLiteral(object input)
        {
            string output = null;

            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(DateTimeOffset))
                {
                    output = ToLiteral(((DateTimeOffset)input).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture));
                }
                else if (type == typeof(IntPtr) || type == typeof(UIntPtr))
                {
                    output = input.ToString();
                }
                else
                {
                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Byte:
                        case TypeCode.SByte:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                            output = input.ToString();
                            break;
                        case TypeCode.DateTime:
                            output = ToLiteral(((DateTime)input).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture));
                            break;
                        case TypeCode.Boolean:
                            output = ((bool)input) ? "true" : "false";
                            break;
                        case TypeCode.Decimal:
                            output = ((Decimal)input).ToString(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Double:
                            output = ((Double)input).ToString(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Single:
                            output = ((Double)((Single)input)).ToString(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Object:
                            //if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                            //{
                            //    output = ToLiteral(Nullable.GetUnderlyingType(type));
                            //}
                            //else
                            //{
                            //    output = ToLiteral(input.ToString());
                            //}
                            break;
                        default:
                            output = StringToLiteral(input.ToString());
                            break;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// To Verbatim
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string StringToLiteral(string input)
        {
            using (var writer = new System.IO.StringWriter())
            {
                using (var provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new System.CodeDom.CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        [Flags]
        public enum IncludePartsEnum
        {
            TypeName = 0,
            FullTypeName = 2,
            PropertyOrFieldName = 4,
            Value = 8
        }
    }
}
