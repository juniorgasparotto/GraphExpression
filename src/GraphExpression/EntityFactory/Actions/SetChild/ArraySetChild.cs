using GraphExpression.Utils;
using System;

namespace GraphExpression
{
    public class ArraySetChild : ISetChild
    {
        public bool CanSetChild(Entity item, Entity child)
        {
            return item.Type.IsArray && child.Name.StartsWith(Constants.INDEXER_START);
        }

        public void SetChild(Entity item, Entity child)
        {
            var array = (Array)item.Value;
            var arrayValue = child.Value;

            // [0] -> simple array
            // [0, 0, 0] -> multidimentional
            long[] indexes = ReflectionUtils.GetArrayIndexesByString(child.Name);
            array.SetValue(arrayValue, indexes);
        }
    }
}