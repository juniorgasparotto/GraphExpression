using GraphExpression.Utils;
using Mono.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace GraphExpression
{
    public class DefaultGetMemberInfo : IGetMemberInfo
    {
        public bool CanGetMemberInfo(Entity item)
        {
            return item.Factory.IsTyped
                  && item.Name != null
                  && !item.Name.StartsWith(Constants.INDEXER_START) // ignore [0] members
                  && item.Parent.Type != null;
        }

        public MemberInfo GetMemberInfo(Entity item)
        {
            var memberInfo = item.Parent.Type
                       .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       .Where(f => f.Name == item.Name)
                       .FirstOrDefault();

            memberInfo = memberInfo ?? throw new Exception($"Member not exists {item.Name} in {item.Parent.Type.FullName}");

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