namespace GraphExpression.Serialization
{
    public interface ISetChild : IEntityDeserialize
    {
        bool CanSetChild(ItemDeserializer item, ItemDeserializer child);
        void SetChild(ItemDeserializer item, ItemDeserializer child);
    }
}