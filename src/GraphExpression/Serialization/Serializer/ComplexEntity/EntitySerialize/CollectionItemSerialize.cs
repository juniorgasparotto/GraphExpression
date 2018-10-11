using System;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Default collection item serialize
    /// </summary>
    public class CollectionItemSerialize : IEntitySerialize
    {
        /// <summary>
        /// Verify if EntityItem can be serialize
        /// </summary>
        /// <param name="serializer">Serializer instance</param>
        /// <param name="item">EntityItem to check</param>
        /// <returns>Return TRUE if can serialize</returns>
        public bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            return item is CollectionItemEntity;
        }

        /// <summary>
        /// Return info about the EntityItem serialize
        /// </summary>
        /// <param name="serializer">Serializer instance</param>
        /// <param name="item">EntityItem to discovery info</param>
        /// <returns>Return info about the EntityItem serialize</returns>
        public (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
        {
            var cast = (CollectionItemEntity)item;
            return (
                item.Entity?.GetType(),
                $"[{cast.Key.ToString()}]"
            );
        }
    }
}
