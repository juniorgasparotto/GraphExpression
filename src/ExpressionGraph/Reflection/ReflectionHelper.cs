using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public static class ReflectionHelper
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

        public static string CSharpName(this Type type)
        {
            var sb = new StringBuilder();
            var name = type.Name;
            if (!type.IsGenericType) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                                            .Select(t => t.CSharpName())));
            sb.Append(">");
            return sb.ToString();
        }
    }
}