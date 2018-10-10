using GraphExpression.Utils;
using Mono.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Class default to search for type members
    /// </summary>
    public class MemberInfoDiscovery : IMemberInfoDiscovery
    {
        /// <summary>
        /// Verify if can discovery member
        /// </summary>
        /// <param name="item">The item that contains the "MemberName" to do the search</param>
        /// <returns>Return TRUE if can discovery</returns>
        public bool CanDiscovery(Entity item)
        {
            return item.Factory.IsTyped
                  && item.Name != null
                  && !item.Name.StartsWith(Constants.INDEXER_START) // ignore [0] members
                  && item.Parent.Type != null;
        }

        /// <summary>
        /// Return the member info from the "item.MemberInfo"
        /// </summary>
        /// <param name="item">The item that contains the "MemberName" to do the search</param>
        /// <returns>Return the MemberInfo or null if not exists</returns>
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