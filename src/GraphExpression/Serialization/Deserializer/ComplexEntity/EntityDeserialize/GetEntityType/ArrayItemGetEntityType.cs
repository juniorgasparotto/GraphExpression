using GraphExpression.Utils;
using System;

namespace GraphExpression.Serialization
{
    public class ArrayItemGetEntityType : IGetEntityType
    {
        public bool CanGetEntityType(ItemDeserializer item)
        {
            return item.Parent.EntityType.IsArray && item.MemberName.StartsWith(Constants.INDEXER_START);
        }

        public Type GetEntityType(ItemDeserializer item)
        {
            return item.Parent.EntityType.GetElementType();
        }
    }
}