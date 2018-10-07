namespace GraphExpression
{
    public interface IGetValue
    {
        bool CanGetValue(Entity item);
        object GetValue(Entity item);
    }
}