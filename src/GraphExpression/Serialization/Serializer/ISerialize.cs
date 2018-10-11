using System;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Standard interface for creating expression serializers
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public interface ISerialize<T>
    {
        /// <summary>
        /// Serialize a expression to string
        /// </summary>
        /// <returns>Expression as string</returns>
        string Serialize();

        /// <summary>
        /// Serialize a unique EntityItem
        /// </summary>
        /// <param name="item">EntityItem to serialize</param>
        /// <returns>Entity item as string</returns>
        string SerializeItem(EntityItem<T> item);
    }
}