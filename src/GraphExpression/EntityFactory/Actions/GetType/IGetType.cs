using System;

namespace GraphExpression
{
    public interface IGetType
    {
        bool CanGetEntityType(Entity item);
        Type GetEntityType(Entity item);
    }
}