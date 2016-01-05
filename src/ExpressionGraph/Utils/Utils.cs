using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    internal static class Utils
    {
        public static string TrimAll(string str)
        {
            return str.Trim().TrimStart('\r', '\n').TrimEnd('\r', '\n');
        }

        public static Dictionary<TKey, TValue> InvertDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            var reverse = new Dictionary<TKey, TValue>();

            for (var i = dictionary.Count - 1; i >= 0; i--)
                reverse.Add(dictionary.Keys.ElementAt(i), dictionary.Values.ElementAt(i));

            return reverse;
        }

        public static string Indent(string str, int count)
        {
            var builder = new StringBuilder();
            foreach (var s in str)
            {
                builder.AppendLine("".PadLeft(count) + s);
            }

            return builder.ToString();
        }

        public static string ToLiteral(object input, bool addNullLiteral = true)
        {
            string output = addNullLiteral ? "null" : "";

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
                            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                output = ToLiteral(Nullable.GetUnderlyingType(type));
                            }
                            else
                            {
                                output = ToLiteral(input.ToString());
                            }
                            break;
                        default:
                            output = ToLiteral(input.ToString());
                            break;
                    }
                }
            }

            return output;
        }

        public static string ToLiteral(string input)
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
