using GraphExpression.Utils;
using System.Collections.Generic;
using System.Dynamic;

namespace GraphExpression
{
    public class ExpandoObjectSetChild : ISetChild
    {
        public bool CanSet(Entity item, Entity child)
        {
            return ReflectionUtils.IsAnonymousType(item.Type) || item.Type == typeof(ExpandoObject);
        }

        public void SetChild(Entity item, Entity child)
        {
            var dic = (IDictionary<string, object>)item.Value;
            dic.Add(child.Name, child.Value);
        }
    }
}