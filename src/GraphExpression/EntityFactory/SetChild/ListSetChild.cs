using GraphExpression.Utils;
using System.Collections;

namespace GraphExpression
{
    /// <summary>
    /// Class default to set value in IList
    /// </summary>
    public class ListSetChild : ISetChild
    {
        /// <summary>
        /// Verify if can set child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        /// <returns>Return TRUE if can set</returns>
        public bool CanSet(Entity item, Entity child)
        {
            return !item.Type.IsArray 
                && item.Value is IList
                && child.Name.StartsWith(Constants.INDEXER_START);
        }

        /// <summary>
        /// Set a child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        public void SetChild(Entity item, Entity child)
        {
            var list = (IList)item.Value;
            list.Add(child.Value);
        }
    }
}
