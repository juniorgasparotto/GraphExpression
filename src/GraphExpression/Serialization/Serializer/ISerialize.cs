using System;

namespace GraphExpression.Serialization
{
    public interface ISerialize<T>
    {
        string Serialize();
        string SerializeItem(EntityItem<T> item);
    }
}