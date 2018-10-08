using GraphExpression.Utils;
using System;
using System.Collections;

namespace GraphExpression
{
    public class ListItemTypeDiscovery : ITypeDiscovery
    {
        public bool CanGetEntityType(Entity item)
        {
            return item.Name.StartsWith(Constants.INDEXER_START) 
                && !item.Parent.Type.IsArray 
                && item.Parent.Value is IList;
        }

        public Type GetEntityType(Entity item)
        {
            return item.Parent.Type.GenericTypeArguments[0];
        }
    }
}