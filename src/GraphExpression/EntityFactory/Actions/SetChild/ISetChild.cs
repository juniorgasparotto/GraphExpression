namespace GraphExpression
{
    public interface ISetChild 
    {
        bool CanSetChild(Entity item, Entity child);
        void SetChild(Entity item, Entity child);
    }
}