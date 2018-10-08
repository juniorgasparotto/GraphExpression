using System;
using GraphExpression.Utils;

namespace GraphExpression
{
    public class MemberInfoTypeDiscovery : ITypeDiscovery
    {
        public bool CanGetEntityType(Entity item)
        {
            return item.MemberInfo != null;
        }

        public Type GetEntityType(Entity item)
        {   
            return ReflectionUtils.GetMemberType(item.MemberInfo);
        }
    }
}