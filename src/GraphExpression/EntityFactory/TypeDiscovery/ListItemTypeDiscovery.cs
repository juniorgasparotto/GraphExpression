using GraphExpression.Utils;
using System;
using System.Collections;

namespace GraphExpression
{
    /// <summary>
    /// Class default to discovery list item type
    /// </summary>
    public class ListItemTypeDiscovery : ITypeDiscovery
    {
        /// <summary>
        /// Verify if can discovery type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Return TRUE if can discovery</returns>
        public bool CanDiscovery(Entity item)
        {
            return item.Name.StartsWith(Constants.INDEXER_START) 
                && !item.Parent.Type.IsArray 
                && item.Parent.Value is IList;
        }

        /// <summary>
        /// Returns the desired Type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Returns the desired Type</returns>
        public Type GetEntityType(Entity item)
        {
            return item.Parent.Type.GenericTypeArguments[0];
        }
    }
}