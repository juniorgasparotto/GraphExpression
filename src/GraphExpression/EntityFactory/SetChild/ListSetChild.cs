using GraphExpression.Utils;
using System.Collections;

namespace GraphExpression
{
    public class ListSetChild : ISetChild
    {
        public bool CanSet(Entity item, Entity child)
        {
            return !item.Type.IsArray 
                && item.Value is IList
                && child.Name.StartsWith(Constants.INDEXER_START);
        }

        public void SetChild(Entity item, Entity child)
        {
            var list = (IList)item.Value;
            list.Add(child.Value);
        }
    }
}
