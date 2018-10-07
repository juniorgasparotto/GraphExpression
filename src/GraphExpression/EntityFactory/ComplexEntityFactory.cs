using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class ComplexEntityFactory<T> : ComplexEntityFactory
    {
        public new T Value => base.Value == null ? default(T) : (T)base.Value;

        public ComplexEntityFactory(Entity root = null) : base(typeof(T), root)
        {
        }

        public new ComplexEntityFactory<T> Build()
        {
            return (ComplexEntityFactory<T>)base.Build();
        }
    }

    public class ComplexEntityFactory : IDeserializeFactory<Entity>
    {
        private Dictionary<Type, Type> mapTypes;
        private List<string> errors;

        public bool IsTyped { get; private set; }        
        public IReadOnlyDictionary<Type, Type> MapTypes { get => mapTypes; }

        public IReadOnlyList<string> Errors { get => errors; }
        public bool IgnoreErrors { get; set; }
        public List<object> ItemsDeserialize { get; set; }

        public Entity Root { get; set; }
        public Type Type { get; }
        public object Value => Root?.Value;

        public ComplexEntityFactory(Type type, Entity root = null)
        {
            this.mapTypes = new Dictionary<Type, Type>();
            this.errors = new List<string>();

            this.Type = type;
            this.Root = root;
            this.IsTyped = Type != null;
            this.ItemsDeserialize = new List<object>
            {
                // Get Entity (order is important - eg. ComplexEntityGetEntity < ArrayGetEntity)
                new PrimitivesGetValue(),
                new ComplexEntityGetValue(),
                new ArrayGetValue(),
                new ExpandoObjectGetValue(),

                // Member info
                new DefaultGetMemberInfo(),

                // Set child (order is important)
                new MemberInfoSetChild(),
                new DictionarySetChild(),
                new ExpandoObjectSetChild(),
                new ArraySetChild(),
                new ListSetChild(),

                // Get Entity type (order is important)
                new DictionaryItemGetType(),
                new MemberInfoGetType(),
                new ListItemGetType(),
                new ArrayItemGetType()
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

        public ComplexEntityFactory Build()
        {
            // 1) Root + null
            if (Root != null)
            {
                // 2) Set references, is important set b.Build here because
                // exists a expression with a one member
                // BuildItem() + Item("true");
                Root.Factory = this;

                // 3) FROM: A + (B + C) + D
                // TO  : Root + (A + (B + C) + D)
                // ---
                // B + C
                // A + B
                // A + D
                // Root + A -> Execute all Additions existing in "A"
                //             Root is not a token, is only to execute expression.
                // ---

                if (Root.Operations != null && Root.Operations.Count > 0)
                {
                    Root.Operations.RemoveAll(f =>
                    {
                        if (f.Executed)
                            return true;

                        // Set root for all
                        f.Source.Factory = this;
                        f.Target.Factory = this;

                        return false;
                    });

                    foreach (var e in Root.Operations)
                    {
                        var itemDeserialize = this
                                    .ItemsDeserialize
                                    .OfType<ISetChild>()
                                    .LastOrDefault(f => f.CanSetChild(e.Source, e.Target));

                        itemDeserialize?.SetChild(e.Source, e.Target);
                        e.Executed = true;
                    }
                }
            }

            return this;
        }

        #region IDeserializeFactory

        private Dictionary<int, Entity> entities;
        public IEnumerable<Entity> Entities { get => entities.Values; }

        public Entity GetEntity(string name, int index)
        {
            if (this.entities == null)
                this.entities = new Dictionary<int, Entity>();

            var item = new Entity(name);
            entities.Add(index, item);
            return item;
        }

        #endregion
    }
}