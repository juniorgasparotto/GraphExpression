using System;

namespace GraphExpression
{
    public interface ITypeDiscovery
    {
        bool CanGetEntityType(Entity item);
        Type GetEntityType(Entity item);
    }
}