using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// This class is used as a vertex cache when in the expression assembly the graph information is linked.
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public class VertexContainer<T>
    {
        /// <summary>
        /// Represents the ID of a given entity
        /// </summary>
        public class EntityId
        {
            /// <summary>
            /// ID of the entity
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Real entity
            /// </summary>
            public T Entity { get; set; }
        }

        /// <summary>
        /// Static property where the vertices that have been read are.
        /// It is necessary to be static because some features need to keep the same ID for the same memory entity. Only in this way is it possible to know if one entity is equal to another within two different instances of "Expression"
        /// </summary>
        public static ConcurrentBag<EntityId> Vertexes { get; set; } = new ConcurrentBag<EntityId>();

        /// <summary>
        /// Add a new entity to list and assign the auto ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static EntityId Add(T entity)
        {
            var entityId = new EntityId { Id = Vertexes.Count, Entity = entity };
            Vertexes.Add(entityId);
            return entityId;
        }

        /// <summary>
        /// Retriave ID using the entity in memory
        /// </summary>
        /// <param name="entity">Real entity</param>
        /// <returns>Return the EntityId instance</returns>
        public static EntityId GetEntityId(T entity)
        {
            return Vertexes.FirstOrDefault(f => f.Entity?.Equals(entity) == true);
        }
    }
}