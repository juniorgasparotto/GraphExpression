using GraphExpression.Utils;
using System.Collections;

namespace GraphExpression
{
    public class DictionarySetChild : ISetChild
    {
        public bool CanSetChild(Entity item, Entity child)
        {
            return item.Value is IDictionary && (child.Name.StartsWith(Constants.INDEXER_START) || IsIgnoredProperty(child));
        }

        public void SetChild(Entity item, Entity child)
        {
            if (IsIgnoredProperty(child))
                return;

            var dicKey = child.Type.GetProperty("Key").GetValue(child.Value);
            var dicValue = child.Type.GetProperty("Value").GetValue(child.Value);

            var add = item.Type.GetMethod("Add", new[] { dicKey.GetType(), dicValue.GetType() });
            add.Invoke(item.Value, new object[] { dicKey, dicValue });
        }

        private bool IsIgnoredProperty(Entity child)
        {
            return child.Name == "Comparer" || child.Name == "Count";
        }

    }
}
