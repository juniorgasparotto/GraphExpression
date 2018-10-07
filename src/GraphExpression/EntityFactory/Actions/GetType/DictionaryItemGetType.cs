using GraphExpression.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphExpression
{
    public class DictionaryItemGetType : IGetType
    {
        public bool CanGetEntityType(Entity item)
        {
            return item.Parent.Value is IDictionary && item.Name.StartsWith(Constants.INDEXER_START);
        }

        public Type GetEntityType(Entity item)
        {
            return typeof(KeyValuePair<,>).MakeGenericType(item.Parent.Type.GetGenericArguments());
        }
    }
}
