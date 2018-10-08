using System.Reflection;

namespace GraphExpression
{
    public interface IMemberInfoDiscovery
    {
        bool CanDiscovery(Entity item);
        MemberInfo GetMemberInfo(Entity item);
    }
}