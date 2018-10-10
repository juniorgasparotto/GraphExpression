using System;
using GraphExpression.Utils;

namespace GraphExpression
{
    /// <summary>
    /// Class default to discovery MemberInfo type
    /// </summary>
    public class MemberInfoTypeDiscovery : ITypeDiscovery
    {
        /// <summary>
        /// Verify if can discovery type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Return TRUE if can discovery</returns>
        public bool CanDiscovery(Entity item)
        {
            return item.MemberInfo != null;
        }

        /// <summary>
        /// Returns the desired Type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Returns the desired Type</returns>
        public Type GetEntityType(Entity item)
        {   
            return ReflectionUtils.GetMemberType(item.MemberInfo);
        }
    }
}