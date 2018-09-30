using GraphExpression.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public class DictionaryItemGetEntityType : IGetEntityType
    {
        public bool CanGetEntityType(ItemDeserializer item)
        {
            return item.Parent.Entity is IDictionary && item.MemberName.StartsWith(Constants.INDEXER_START);
        }

        public Type GetEntityType(ItemDeserializer item)
        {
            return typeof(KeyValuePair<,>).MakeGenericType(item.Parent.EntityType.GetGenericArguments());
        }
    }
}
