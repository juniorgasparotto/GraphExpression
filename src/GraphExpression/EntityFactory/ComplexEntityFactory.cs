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

    public class ComplexEntityFactory : IDeserializeFactory<Entity>, IEntityFactory
    {
        private readonly Dictionary<Type, Type> mapTypes;
        private readonly List<string> errors;
        private readonly List<Entity> entities;

        public IReadOnlyList<Entity> Entities { get => entities; }
        public bool IsTyped { get; }
        public IReadOnlyDictionary<Type, Type> MapTypes { get => mapTypes; }

        public IReadOnlyList<string> Errors { get => errors; }
        public bool IgnoreErrors { get; set; }

        public List<ITypeDiscovery> TypeDiscovery { get; }
        public List<IValueLoader> ValueLoader { get; }
        public List<IMemberInfoDiscovery> MemberInfoDiscovery { get; }
        public List<ISetChild> SetChildAction { get; }

        public Entity Root { get; set; }
        public Type RootType { get; }
        public object Value => Root?.Value;

        public ComplexEntityFactory(Type type, Entity root = null)
        {
            this.entities = new List<Entity>();
            this.mapTypes = new Dictionary<Type, Type>();
            this.errors = new List<string>();

            this.RootType = type;
            this.Root = root;
            this.IsTyped = RootType != null;
            this.ValueLoader = new List<IValueLoader>
            {
                // Get Entity (order is important - eg. ComplexEntityGetEntity < ArrayGetEntity)
                new PrimitiveValueLoader(),
                new ComplexEntityValueLoader(),
                new ArrayValueLoader(),
                new ExpandoObjectValueLoader()
            };

            this.MemberInfoDiscovery = new List<IMemberInfoDiscovery>
            { 
                // Member info
                new MemberInfoDiscovery(),
            };

            this.SetChildAction = new List<ISetChild>()
            { 
                // Set child (order is important)
                new MemberInfoSetChild(),
                new DictionarySetChild(),
                new ExpandoObjectSetChild(),
                new ArraySetChild(),
                new ListSetChild(),
            };

            this.TypeDiscovery = new List<ITypeDiscovery>
            {
                // Get Entity type (order is important)
                new DictionaryItemTypeDiscovery(),
                new MemberInfoTypeDiscovery(),
                new ListItemTypeDiscovery(),
                new ArrayItemTypeDiscovery()
            };
        }

        public void AddMapType<TFrom, TTo>()
        {
            mapTypes.Add(typeof(TFrom), typeof(TTo));
        }

        public void AddError(string err)
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

                        // Add in list all entities occurences
                        // its is necessary to discovery repeat entities with
                        // same ID
                        this.entities.Add(f.Source);
                        this.entities.Add(f.Target);

                        return false;
                    });

                    foreach (var e in Root.Operations)
                    {
                        var itemDeserialize = this
                                    .SetChildAction
                                    .LastOrDefault(f => f.CanSet(e.Source, e.Target));

                        itemDeserialize?.SetChild(e.Source, e.Target);
                        e.Executed = true;
                    }
                }
            }

            return this;
        }

        #region IDeserializeFactory - used only in deserialize flow
              
        public Entity GetEntity(string name, int index)
        {
            return new Entity(name);
        }

        #endregion
    }
}