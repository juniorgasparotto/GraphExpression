using System;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Default commom object item serialize
    /// </summary>
    public class ObjectSerialize : IEntitySerialize
    {
        /// <summary>
        /// Verify if EntityItem can be serialize
        /// </summary>
        /// <param name="serializer">Serializer instance</param>
        /// <param name="item">EntityItem to check</param>
        /// <returns>Return TRUE if can serialize</returns>
        public bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            return true;
        }

        /// <summary>
        /// Return info about the EntityItem serialize
        /// </summary>
        /// <param name="serializer">Serializer instance</param>
        /// <param name="item">EntityItem to discovery info</param>
        /// <returns>Return info about the EntityItem serialize</returns>
        public (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            return (
                item.Entity?.GetType(),
                null
            );
        }
    }
}
