using GraphExpression.Utils;
using Mono.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace GraphExpression.Serialization
{
    public class DefaultGetMemberInfo : IGetMemberInfo
    {
        public bool CanGetMemberInfo(ItemDeserializer item)
        {
            return item.EntityFactory.IsTyped
                  && item.MemberName != null
                  && !item.MemberName.StartsWith(Constants.INDEXER_START) // ignore [0] members
                  && item.Parent.EntityType != null;
        }

        public MemberInfo GetMemberInfo(ItemDeserializer item)
        {
            var memberInfo = item.Parent.EntityType
                       .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       .Where(f => f.Name == item.MemberName)
                       .FirstOrDefault();

            memberInfo = memberInfo ?? throw new Exception($"Member not exists {item.MemberName} in {item.Parent.EntityType.FullName}");

            if (memberInfo is PropertyInfo prop)
            {
                if (prop.GetSetMethod(true) != null)
                    memberInfo = prop;
                else
                    memberInfo = prop.GetBackingField();
            }

            return memberInfo;
        }
    }
}