namespace GraphExpression.Serialization
{
    public interface IGetEntity : IEntityDeserialize
    {
        bool CanGetEntity(ItemDeserializer item);
        object GetEntity(ItemDeserializer item);
    }
}