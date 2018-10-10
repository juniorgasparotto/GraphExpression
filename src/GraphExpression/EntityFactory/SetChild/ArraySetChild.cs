using GraphExpression.Utils;
using System;

namespace GraphExpression
{
    /// <summary>
    /// Class default to set value in array
    /// </summary>
    public class ArraySetChild : ISetChild
    {
        /// <summary>
        /// Verify if can set child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        /// <returns>Return TRUE if can set</returns>
        public bool CanSet(Entity item, Entity child)
        {
            return item.Type.IsArray && child.Name.StartsWith(Constants.INDEXER_START);
        }

        /// <summary>
        /// Set a child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        public void SetChild(Entity item, Entity child)
        {
            var array = (Array)item.Value;
            var arrayValue = child.Value;

            // [0] -> simple array
            // [0, 0, 0] -> multidimentional
            long[] indexes = ReflectionUtils.GetArrayIndexesByString(child.Name);
            array.SetValue(arrayValue, indexes);
        }
    }
}