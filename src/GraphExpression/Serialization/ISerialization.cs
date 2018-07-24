using System;

namespace GraphExpression.Serialization
{
    public interface ISerialization<T>
    {
        string Serialize();
        Func<EntityItem<T>, string> SerializeItem { get; set; }
    }
}