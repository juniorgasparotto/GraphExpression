using GraphExpression.Serialization;
using System;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public class ComplexEntityFactoryDeserializer
    {
        private Dictionary<string, ComplexEntityDeserializer> entities;
        private Dictionary<Type, Type> mapTypes;

        public Type TypeRoot { get; set; }
        public bool IsTyped { get; private set; }
        public IEnumerable<ComplexEntityDeserializer> Entities { get => entities.Values; }
        public IReadOnlyDictionary<Type, Type> MapTypes { get => mapTypes; }
        internal DeserializationTime DeserializationTime { get; set; }

        public ComplexEntityFactoryDeserializer(Type root)
        {
            this.TypeRoot = root;
            this.IsTyped = TypeRoot != null;
            this.entities = new Dictionary<string, ComplexEntityDeserializer>();
            this.mapTypes = new Dictionary<Type, Type>();
        }

        public void AddMapType<TFrom, TTo>()
        {
            mapTypes.Add(typeof(TFrom), typeof(TTo));
        }

        public ComplexEntityDeserializer GetEntity(string name, string id)
        {
            if (DeserializationTime == DeserializationTime.Creation)
            {
                var entity = new ComplexEntityDeserializer(name);
                entities.Add(id, entity);

                if (entity.EntityFactory == null)
                    entity.EntityFactory = this;
                return entity;
            }
            else
            {
                return entities[id];
            }
        }
    }
}