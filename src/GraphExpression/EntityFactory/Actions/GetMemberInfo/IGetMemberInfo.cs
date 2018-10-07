using System.Reflection;

namespace GraphExpression
{
    public interface IGetMemberInfo
    {
        bool CanGetMemberInfo(Entity item);
        MemberInfo GetMemberInfo(Entity item);
    }
}