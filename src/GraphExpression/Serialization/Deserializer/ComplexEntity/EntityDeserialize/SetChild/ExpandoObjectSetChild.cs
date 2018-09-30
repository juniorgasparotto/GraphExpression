using GraphExpression.Utils;
using System.Collections.Generic;
using System.Dynamic;

namespace GraphExpression.Serialization
{
    public class ExpandoObjectSetChild : ISetChild
    {
        public bool CanSetChild(ItemDeserializer item, ItemDeserializer child)
        {
            return ReflectionUtils.IsAnonymousType(item.EntityType) || item.EntityType == typeof(ExpandoObject);
        }

        public void SetChild(ItemDeserializer item, ItemDeserializer child)
        {
            var dic = (IDictionary<string, object>)item.Entity;
            dic.Add(child.MemberName, child.Entity);
        }
    }
}