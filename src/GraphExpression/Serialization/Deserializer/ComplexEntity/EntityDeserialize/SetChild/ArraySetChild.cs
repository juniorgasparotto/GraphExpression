using GraphExpression.Utils;
using System;

namespace GraphExpression.Serialization
{
    public class ArraySetChild : ISetChild
    {
        public bool CanSetChild(ItemDeserializer item, ItemDeserializer child)
        {
            return item.EntityType.IsArray && child.MemberName.StartsWith(Constants.INDEXER_START);
        }

        public void SetChild(ItemDeserializer item, ItemDeserializer child)
        {
            var array = (Array)item.Entity;
            var arrayValue = child.Entity;

            // [0] -> simple array
            // [0, 0, 0] -> multidimentional
            long[] indexes = ReflectionUtils.GetArrayIndexesByString(child.MemberName);
            array.SetValue(arrayValue, indexes);
        }
    }
}