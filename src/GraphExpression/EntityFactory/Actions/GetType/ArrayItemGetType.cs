using GraphExpression.Utils;
using System;

namespace GraphExpression
{
    public class ArrayItemGetType : IGetType
    {
        public bool CanGetEntityType(Entity item)
        {
            return item.Parent.Type.IsArray && item.Name.StartsWith(Constants.INDEXER_START);
        }

        public Type GetEntityType(Entity item)
        {
            return item.Parent.Type.GetElementType();
        }
    }
}