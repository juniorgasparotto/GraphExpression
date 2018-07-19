using System;

namespace GraphExpression.Serialization
{
    public interface ISerialization<T>
    {
        string Serialize();
        string SerializeItem(EntityItem<T> item);
    }
}