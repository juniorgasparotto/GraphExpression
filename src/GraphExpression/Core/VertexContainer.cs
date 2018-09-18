using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class VertexContainer<T>
    {
        public class EntityId
        {
            public long Id { get; set; }
            public T Entity { get; set; }
        }

        public static ConcurrentBag<EntityId> Vertexes { get; set; } = new ConcurrentBag<EntityId>();

        public static EntityId Add(T entity)
        {
            var entityId = new EntityId { Id = Vertexes.Count, Entity = entity };
            Vertexes.Add(entityId);
            return entityId;
        }

        public static EntityId GetEntityId(T entity)
        {
            return Vertexes.FirstOrDefault(f => f.Entity?.Equals(entity) == true);
        }
    }
}