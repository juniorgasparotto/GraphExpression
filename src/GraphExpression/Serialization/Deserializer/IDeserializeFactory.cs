namespace GraphExpression
{
    public interface IDeserializeFactory<T>
    {
        T GetEntity(string name, int index);
    }
}