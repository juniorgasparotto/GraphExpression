using GraphExpression.Utils;
using System.Dynamic;

namespace GraphExpression
{
    /// <summary>
    /// Class default used to creating a ExpandoObject
    /// </summary>
    public class ExpandoObjectValueLoader : IValueLoader
    {
        /// <summary>
        /// Verify if can load instance value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return TRUE if can load</returns>
        public bool CanLoad(Entity item)
        {
            return ReflectionUtils.IsAnonymousType(item.Type) || item.Type == typeof(ExpandoObject);
        }

        /// <summary>
        /// Return value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return object instance</returns>
        public object GetValue(Entity item)
        {
            return new ExpandoObject();
        }
    }
}