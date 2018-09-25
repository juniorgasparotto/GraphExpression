using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GraphExpression.Utils
{
    internal static class ReflectionUtils
    {
        public static IEnumerable<Type> GetAllParentTypes(Type type, bool includeSelf = true, bool includeInterfaces = true)
        {
            if (includeSelf)
                yield return type;

            if (includeInterfaces)
            {
                foreach (Type i in type.GetInterfaces())
                {
                    yield return i;
                    foreach (Type t in GetAllParentTypes(i, false, includeInterfaces))
                    {
                        yield return t;
                    }
                }
            }

            if (type.BaseType != null)
            {
                yield return type.BaseType;
                foreach (Type b in GetAllParentTypes(type.BaseType, false, includeInterfaces))
                {
                    yield return b;
                }
            }
        }

        public static bool TryGetPropertyValue<T>(object source, string property, out T value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            value = default(T);

            var sourceType = source.GetType();
            var sourceProperties = sourceType.GetProperties();
            var properties = sourceProperties
                .Where(s => s.Name.Equals(property));

            if (properties.Count() == 0)
            {
                sourceProperties = sourceType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
                properties = sourceProperties.Where(s => s.Name.Equals(property));
            }

            if (properties.Count() > 0)
            {
                var propertyValue = properties
                    .Select(s => s.GetValue(source, null))
                    .FirstOrDefault();

                if (propertyValue != null)
                {
                    value = (T)propertyValue;
                    return true;
                }
            }

            return false;
        }

        public static string CSharpName(Type type, bool showFullName = false)
        {
            var sb = new StringBuilder();
            var name = showFullName ? type.FullName : type.Name;
            //return name;
            if (!type.IsGenericType || name.IndexOf('`') == -1) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                                            .Select(t => CSharpName(t, showFullName))));
            sb.Append(">");
            return sb.ToString();
        }

        public static Type GetDeclaredType<TSelf>(TSelf self)
        {
            return typeof(TSelf);
        }

        public static void IterateArrayMultidimensional(Array inObject, Action<int[]> callback, int[] indices = null)
        {
            int dimension = indices == null ? 0 : indices.Length;
            int[] newIndices = new int[dimension + 1];

            for (int i = 0; i < dimension; i++)
                newIndices[i] = indices[i];

            for (int i = inObject.GetLowerBound(dimension); i <= inObject.GetUpperBound(dimension); i++)
            {
                newIndices[dimension] = i;
                bool isTopLevel = (newIndices.Length == inObject.Rank);

                if (isTopLevel)
                    callback((int[])newIndices.Clone());
                else
                    IterateArrayMultidimensional(inObject, callback, newIndices);
            }
        }

        public static bool IsSystemType(Type type)
        {
            var typeName = type.Namespace ?? "";
            if (typeName.StartsWith("System") || typeName.StartsWith("Microsoft"))
                return true;
            return false;
        }

        public static bool IsValidIdentifier(string name)
        {
            return SyntaxFacts.IsValidIdentifier(name);
        }

        public static bool IsCSharpKeyword(string name)
        {
            switch (name)
            {
                case "bool":
                case "byte":
                case "sbyte":
                case "short":
                case "ushort":
                case "int":
                case "uint":
                case "long":
                case "ulong":
                case "double":
                case "float":
                case "decimal":
                case "string":
                case "char":
                case "object":
                case "typeof":
                case "sizeof":
                case "null":
                case "true":
                case "false":
                case "if":
                case "else":
                case "while":
                case "for":
                case "foreach":
                case "do":
                case "switch":
                case "case":
                case "default":
                case "lock":
                case "try":
                case "throw":
                case "catch":
                case "finally":
                case "goto":
                case "break":
                case "continue":
                case "return":
                case "public":
                case "private":
                case "internal":
                case "protected":
                case "static":
                case "readonly":
                case "sealed":
                case "const":
                case "new":
                case "override":
                case "abstract":
                case "virtual":
                case "partial":
                case "ref":
                case "out":
                case "in":
                case "where":
                case "params":
                case "this":
                case "base":
                case "namespace":
                case "using":
                case "class":
                case "struct":
                case "interface":
                case "delegate":
                case "checked":
                case "get":
                case "set":
                case "add":
                case "remove":
                case "operator":
                case "implicit":
                case "explicit":
                case "fixed":
                case "extern":
                case "event":
                case "enum":
                case "unsafe":
                    return true;
                default:
                    return false;
            }
        }

        public static string RemoveQuotes(string value, char quote)
        {
            // minimun: '' or ""
            if (value.Length >= 2 && value.StartsWith(quote.ToString()))
                return value.Substring(1, value.Length - 2);
            return value;
        }

        /// <summary>
        /// To Verbatim
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToVerbatim(string input)
        {
            return SyntaxFactory.Literal(input).ToString();

            //using (var writer = new System.IO.StringWriter())
            //{
            //    using (var provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp"))
            //    {
            //        provider.GenerateCodeFromExpression(new System.CodeDom.CodePrimitiveExpression(input), writer, null);
            //        return writer.ToString();
            //    }
            //}
        }
    }
}