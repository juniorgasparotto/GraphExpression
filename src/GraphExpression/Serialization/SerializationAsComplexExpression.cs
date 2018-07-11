using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class SerializationAsComplexExpression : SerializationAsExpression<ComplexEntity>
    {
        public IncludePartsEnum IncludeParts { get; set; }
        public string PropertySymbol { get; set; }
        public string FieldSymbol { get; set; }

        public SerializationAsComplexExpression(Expression<ComplexEntity> expression) : base(expression)
        {
            SerializeItemCallback = SerializeItem;
            PropertySymbol = "@";
            FieldSymbol = "!";
        }

        private string SerializeItem(EntityItem<ComplexEntity> item)
        {
            string strEntityType, strType, strContainer;
            if (item.Entity is PropertyEntity prop)
            {
                strEntityType = PropertySymbol;
                strType = prop.Property.PropertyType.FullName;
                strContainer = prop.Property.Name;
            }
            else if (item.Entity is FieldEntity field)
            {
                strEntityType = FieldSymbol;
                strType = field.Field.FieldType.FullName;
                strContainer = field.Field.Name;
            }
            else
            {
                strEntityType = "";
                strType = item.Entity.Entity.GetType().FullName;
                strContainer = null;
            }

            var value = ToLiteral(item.Entity.Entity);
            var output = strEntityType + strType;
            output += strContainer == null ? "" : "." + strContainer;

            if (value != null)
                output += ":" + value;
            else if (item.Entity.Entity != null) // When is not primitive entity use hashcode
                output += "." + item.Entity.Entity.GetHashCode();

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
