using GraphExpression.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    /// Class default to discovery dictionary item type
    /// </summary>
    public class DictionaryItemTypeDiscovery : ITypeDiscovery
    {
        /// <summary>
        /// Verify if can discovery type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Return TRUE if can discovery</returns>
        public bool CanDiscovery(Entity item)
        {
            return item.Parent.Value is IDictionary && item.Name.StartsWith(Constants.INDEXER_START);
        }

        /// <summary>
        /// Returns the desired Type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Returns the desired Type</returns>
        public Type GetEntityType(Entity item)
        {
            return typeof(KeyValuePair<,>).MakeGenericType(item.Parent.Type.GetGenericArguments());
        }
    }
}
