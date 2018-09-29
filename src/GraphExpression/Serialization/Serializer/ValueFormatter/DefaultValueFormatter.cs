using GraphExpression.Utils;
using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class DefaultValueFormatter : IValueFormatter
    {
        public virtual string Format(Type type, object value)
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
                    output = ((DateTimeOffset)input).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
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
                            output = ((DateTime)input).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Boolean:
                            output = ((bool)input) ? "true" : "false";
                            break;
                        case TypeCode.Decimal:
                            output = ((decimal)input).ToString(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Double:
                            output = ((double)input).ToString(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Single:
                            output = ((double)(float)input).ToString(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Object:                            
                            break;
                        default:
                            output = input.ToString();
                            break;
                    }
                }
            }

            return output;
        }

    }
}
