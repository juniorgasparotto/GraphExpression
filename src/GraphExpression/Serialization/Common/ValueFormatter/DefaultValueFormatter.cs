using GraphExpression.Utils;
using System;
using System.Globalization;

namespace GraphExpression.Serialization
{
    public class DefaultValueFormatter : IValueFormatter
    {
        public virtual string Format(Type type, object value, bool trimQuotes)
        {
            return ToLiteral(value, trimQuotes);
        }

        private string ToLiteral(object input, bool trimQuotes)
        {
            string output = null;

            if (input != null)
            {
                var type = input.GetType();
                if (type == typeof(DateTimeOffset))
                {
                    output = ToLiteral(((DateTimeOffset)input).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture), trimQuotes);
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
                            output = ToLiteral(((DateTime)input).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture), trimQuotes);
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
                            output = ReflectionUtils.StringToLiteral(input.ToString());
                            if (trimQuotes)
                                output = ReflectionUtils.RemoveQuotes(output, Constants.DEFAULT_QUOTE);
                            break;
                    }
                }
            }

            return output;
        }

    }
}
