using System;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Standard interface for serialize a specify EntityItem
    /// </summary>
    public interface IEntitySerialize
    {
        /// <summary>
        /// Verify if EntityItem can be serialize
        /// </summary>
        /// <param name="serializer">Serializer instance</param>
        /// <param name="item">EntityItem to check</param>
        /// <returns>Return TRUE if can serialize</returns>
        bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item);

        /// <summary>
        /// Return info about the EntityItem serialize
        /// </summary>
        /// <param name="serializer">Serializer instance</param>
        /// <param name="item">EntityItem to discovery info</param>
        /// <returns>Return info about the EntityItem serialize</returns>
        (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item);
    }
}
