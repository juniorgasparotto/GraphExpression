﻿namespace GraphExpression
{
    public interface ISetChild 
    {
        bool CanSet(Entity item, Entity child);
        void SetChild(Entity item, Entity child);
    }
}