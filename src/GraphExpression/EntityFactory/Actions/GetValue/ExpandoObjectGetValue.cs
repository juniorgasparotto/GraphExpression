using GraphExpression.Utils;
using System.Dynamic;

namespace GraphExpression
{
    public class ExpandoObjectGetValue : IGetValue
    {
        public bool CanGetValue(Entity item)
        {
            return ReflectionUtils.IsAnonymousType(item.Type) || item.Type == typeof(ExpandoObject);
        }
        
        public object GetValue(Entity item)
        {
            return new ExpandoObject();
        }
    }
}