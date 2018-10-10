using GraphExpression.Utils;
using System;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;

namespace GraphExpression
{
    /// <summary>
    /// Class default used to creating any complex type
    /// </summary>
    public class ComplexEntityValueLoader : IValueLoader
    {
        /// <summary>
        /// Verify if can load instance value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return TRUE if can load</returns>
        public bool CanLoad(Entity item)
        {
            return item.ComplexEntityId != null;
        }

        /// <summary>
        /// Return value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return object instance</returns>
        public object GetValue(Entity item)
        {
            var entityType = item.Type;

            if (ReflectionUtils.IsAnonymousType(item.Type))
                return new ExpandoObject();

            if (entityType.GetConstructors().Where(f => f.GetParameters().Length == 0).Any())
                return Activator.CreateInstance(entityType);
            else if (!entityType.IsInterface && !entityType.IsAbstract)
                return FormatterServices.GetUninitializedObject(entityType);

            return null;
        }
    }
}