using GraphExpression.Utils;
using System.Collections;

namespace GraphExpression
{
    /// <summary>
    /// Class default to set value in dictionary
    /// </summary>
    public class DictionarySetChild : ISetChild
    {
        /// <summary>
        /// Verify if can set child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        /// <returns>Return TRUE if can set</returns>
        public bool CanSet(Entity item, Entity child)
        {
            return item.Value is IDictionary && (child.Name.StartsWith(Constants.INDEXER_START) || IsIgnoredProperty(child));
        }

        /// <summary>
        /// Set a child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
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
