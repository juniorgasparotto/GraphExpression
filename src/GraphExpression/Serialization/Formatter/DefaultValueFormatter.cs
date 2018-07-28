using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class DefaultValueFormatter : IValueFormatter
    {
        public string Format(Type type, object value)
        {
            return ToLiteral(value);
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
    }
}
