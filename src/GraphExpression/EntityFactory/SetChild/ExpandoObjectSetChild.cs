using GraphExpression.Utils;
using System.Collections.Generic;
using System.Dynamic;

namespace GraphExpression
{
    /// <summary>
    /// Class default to set value in expandoObject
    /// </summary>
    public class ExpandoObjectSetChild : ISetChild
    {
        /// <summary>
        /// Verify if can set child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        /// <returns>Return TRUE if can set</returns>
        public bool CanSet(Entity item, Entity child)
        {
            return ReflectionUtils.IsAnonymousType(item.Type) || item.Type == typeof(ExpandoObject);
        }

        /// <summary>
        /// Set a child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        public void SetChild(Entity item, Entity child)
        {
            var dic = (IDictionary<string, object>)item.Value;
            dic.Add(child.Name, child.Value);
        }
    }
}