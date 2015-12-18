using ExpressionGraph.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Type> GetAllParentTypes(Type type, bool includeInterfaces)
        {
            if (includeInterfaces)
            { 
                foreach (Type i in type.GetInterfaces())
                {
                    yield return i;
                    foreach (Type t in GetAllParentTypes(i, includeInterfaces))
                    {
                        yield return t;
                    }
                }
            }

            if (type.BaseType != null)
            {
                yield return type.BaseType;
                foreach (Type b in GetAllParentTypes(type.BaseType, includeInterfaces))
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
            if (!type.IsGenericType) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                                            .Select(t => CSharpName(t, showFullName))));
            sb.Append(">");
            return sb.ToString();
        }

        public static Dictionary<ArrayKey, object> ArrayToDictionary(Array inObject)
        {
            var outObject = new Dictionary<ArrayKey, object>();
            ArrayToDictionary(inObject, outObject, new int[0]);
            return outObject;
        }

        private static void ArrayToDictionary(Array inObject, Dictionary<ArrayKey, object> outObject, int[] indices)
        {
            int dimension = indices.Length;
            int[] newIndices = new int[dimension + 1];

            for (int i = 0; i < dimension; i++)
            {
                newIndices[i] = indices[i];
            }

            for (int i = inObject.GetLowerBound(dimension); i <= inObject.GetUpperBound(dimension); i++)
            {
                newIndices[dimension] = i;
                bool isTopLevel = (newIndices.Length == inObject.Rank);

                if (isTopLevel)
                {
                    outObject.Add(new ArrayKey(newIndices), inObject.GetValue(newIndices));
                }
                else
                {
                    ArrayToDictionary(inObject, outObject, newIndices);
                }
            }
        }
    }
}