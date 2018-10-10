using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Interface that should be used to search for type members 
    /// </summary>
    public interface IMemberInfoDiscovery
    {
        /// <summary>
        /// Verify if can discovery member
        /// </summary>
        /// <param name="item">The item that contains the "MemberName" to do the search</param>
        /// <returns>Return TRUE if can discovery</returns>
        bool CanDiscovery(Entity item);

        /// <summary>
        /// Return the member info from the "item.MemberInfo"
        /// </summary>
        /// <param name="item">The item that contains the "MemberName" to do the search</param>
        /// <returns>Return the MemberInfo or null if not exists</returns>
        MemberInfo GetMemberInfo(Entity item);
    }
}