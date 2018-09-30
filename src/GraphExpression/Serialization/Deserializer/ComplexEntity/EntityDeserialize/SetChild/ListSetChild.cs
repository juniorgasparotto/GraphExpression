using GraphExpression.Utils;
using System.Collections;

namespace GraphExpression.Serialization
{
    public class ListSetChild : ISetChild
    {
        public bool CanSetChild(ItemDeserializer item, ItemDeserializer child)
        {
            return !item.EntityType.IsArray 
                && item.Entity is IList
                && child.MemberName.StartsWith(Constants.INDEXER_START);
        }

        public void SetChild(ItemDeserializer item, ItemDeserializer child)
        {
            var list = (IList)item.Entity;
            list.Add(child.Entity);
        }
    }
}
