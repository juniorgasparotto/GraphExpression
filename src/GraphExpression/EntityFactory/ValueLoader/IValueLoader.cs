namespace GraphExpression
{
    public interface IValueLoader
    {
        bool CanLoad(Entity item);
        object GetValue(Entity item);
    }
}