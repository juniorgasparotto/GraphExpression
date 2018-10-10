using GraphExpression.Utils;
using System;

namespace GraphExpression
{
    /// <summary>
    /// Class default to discovery item array type
    /// </summary>
    public class ArrayItemTypeDiscovery : ITypeDiscovery
    {
        /// <summary>
        /// Verify if can discovery type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Return TRUE if can discovery</returns>
        public bool CanDiscovery(Entity item)
        {
            return item.Parent.Type.IsArray && item.Name.StartsWith(Constants.INDEXER_START);
        }

        /// <summary>
        /// Returns the desired Type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Returns the desired Type</returns>
        public Type GetEntityType(Entity item)
        {
            return item.Parent.Type.GetElementType();
        }
    }
}