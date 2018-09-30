using GraphExpression.Utils;
using System.Dynamic;

namespace GraphExpression.Serialization
{
    public class ExpandoObjectGetEntity : IGetEntity
    {
        public bool CanGetEntity(ItemDeserializer item)
        {
            return ReflectionUtils.IsAnonymousType(item.EntityType) || item.EntityType == typeof(ExpandoObject);
        }
        
        public object GetEntity(ItemDeserializer item)
        {
            return new ExpandoObject();
        }
    }
}