using GraphExpression.Utils;
using System;
using System.Collections;

namespace GraphExpression.Serialization
{
    public class ListItemGetEntityType : IGetEntityType
    {
        public bool CanGetEntityType(ItemDeserializer item)
        {
            return item.MemberName.StartsWith(Constants.INDEXER_START) 
                && !item.Parent.EntityType.IsArray 
                && item.Parent.Entity is IList;
        }

        public Type GetEntityType(ItemDeserializer item)
        {
            return item.Parent.EntityType.GenericTypeArguments[0];
        }
    }
}