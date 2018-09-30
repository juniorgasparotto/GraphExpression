using System;
using GraphExpression.Utils;

namespace GraphExpression.Serialization
{
    public class MemberInfoGetEntityType : IGetEntityType
    {
        public bool CanGetEntityType(ItemDeserializer item)
        {
            return item.MemberInfo != null;
        }

        public Type GetEntityType(ItemDeserializer item)
        {   
            return ReflectionUtils.GetMemberType(item.MemberInfo);
        }
    }
}