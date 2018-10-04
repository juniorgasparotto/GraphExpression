using GraphExpression.Serialization;
using System;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public class ComplexEntityFactoryDeserializer
    {
        private Dictionary<string, ItemDeserializer> entities;
        private Dictionary<Type, Type> mapTypes;
        private List<string> errors;

        internal DeserializationTime DeserializationTime { get; set; }

        public Type TypeRoot { get; set; }
        public bool IsTyped { get; private set; }
        public IEnumerable<ItemDeserializer> Entities { get => entities.Values; }
        public IReadOnlyDictionary<Type, Type> MapTypes { get => mapTypes; }

        public IReadOnlyList<string> Errors { get => errors; }
        public bool IgnoreErrors { get; set; }
        public List<IEntityDeserialize> ItemsDeserialize { get; set; }

        public ComplexEntityFactoryDeserializer(Type root)
        {
            this.entities = new Dictionary<string, ItemDeserializer>();
            this.mapTypes = new Dictionary<Type, Type>();
            this.errors = new List<string>();

            this.TypeRoot = root;
            this.IsTyped = TypeRoot != null;
            this.ItemsDeserialize = new List<IEntityDeserialize>
            {
                // Get Entity (order is important - eg. ComplexEntityGetEntity < ArrayGetEntity)
                new PrimitivesGetEntity(),
                new ComplexEntityGetEntity(),
                new ArrayGetEntity(),
                new ExpandoObjectGetEntity(),

                // Member info
                new DefaultGetMemberInfo(),

                // Set child (order is important)
                new MemberInfoSetChild(),
                new DictionarySetChild(),
                new ExpandoObjectSetChild(),
                new ArraySetChild(),
                new ListSetChild(),

                // Get Entity type (order is important)
                new DictionaryItemGetEntityType(),
                new MemberInfoGetEntityType(),
                new ListItemGetEntityType(),
                new ArrayItemGetEntityType()
            };
        }

        public void AddMapType<TFrom, TTo>()
        {
            mapTypes.Add(typeof(TFrom), typeof(TTo));
        }

        internal void AddError(string err)
        {
            errors.Add(err);
        }

        public ItemDeserializer GetEntity(string name, string id)
        {
            return new ItemDeserializer(name);
            //if (DeserializationTime == DeserializationTime.Creation)
            //{
            //    var entity = new ItemDeserializer(name);
            //    entities.Add(id, entity);

            //    if (entity.EntityFactory == null)
            //        entity.EntityFactory = this;
            //    return entity;
            //}
            //else
            //{
            //    return entities[id];
            //}
        }
    }
}