using System.Reflection;

namespace GraphExpression.Serialization
{
    public interface IGetMemberInfo : IEntityDeserialize
    {
        bool CanGetMemberInfo(ItemDeserializer item);
        MemberInfo GetMemberInfo(ItemDeserializer item);
    }
}