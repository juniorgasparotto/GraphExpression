using GraphExpression.Utils;
using System.Collections;

namespace GraphExpression.Serialization
{
    public class DictionarySetChild : ISetChild
    {
        public bool CanSetChild(ItemDeserializer item, ItemDeserializer child)
        {
            return item.Entity is IDictionary && (child.MemberName.StartsWith(Constants.INDEXER_START) || IsIgnoredProperty(child));
        }

        public void SetChild(ItemDeserializer item, ItemDeserializer child)
        {
            if (IsIgnoredProperty(child))
                return;

            var dicKey = child.EntityType.GetProperty("Key").GetValue(child.Entity);
            var dicValue = child.EntityType.GetProperty("Value").GetValue(child.Entity);

            var add = item.EntityType.GetMethod("Add", new[] { dicKey.GetType(), dicValue.GetType() });
            add.Invoke(item.Entity, new object[] { dicKey, dicValue });
        }

        private bool IsIgnoredProperty(ItemDeserializer child)
        {
            return child.MemberName == "Comparer" || child.MemberName == "Count";
        }

    }
}
