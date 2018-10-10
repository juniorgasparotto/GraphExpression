using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Typed factory for complex entities
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public class ComplexEntityFactory<T> : ComplexEntityFactory
    {
        /// <summary>
        /// Generic value
        /// </summary>
        public new T Value => base.Value == null ? default(T) : (T)base.Value;

        /// <summary>
        /// Create a instance from complex factory
        /// </summary>
        /// <param name="root">Entity Root to start build</param>
        public ComplexEntityFactory(Entity root = null) : base(typeof(T), root)
        {
        }

        /// <summary>
        /// Build the root entity to generate a real entity
        /// </summary>
        /// <returns>Return this instance to mantain fluent codification</returns>
        public new ComplexEntityFactory<T> Build()
        {
            return (ComplexEntityFactory<T>)base.Build();
        }
    }

    /// <summary>
    /// Non typed factory for complex entities
    /// </summary>
    public class ComplexEntityFactory : IDeserializeFactory<Entity>, IEntityFactory
    {
        private readonly Dictionary<Type, Type> mapTypes;
        private readonly List<string> errors;
        private readonly List<Entity> entities;

        /// <summary>
        /// All entities that was loaded during build process
        /// </summary>
        public IReadOnlyList<Entity> Entities { get => entities; }

        /// <summary>
        /// TRUE when build a Entity with a specify type
        /// </summary>
        public bool IsTyped { get; }

        /// <summary>
        /// Can be used to create a concrete instances from a interfaces or abstract classes
        /// </summary>
        public IReadOnlyDictionary<Type, Type> MapTypes { get => mapTypes; }

        /// <summary>
        /// List of errors if exists
        /// </summary>
        public IReadOnlyList<string> Errors { get => errors; }

        /// <summary>
        /// If TRUE, not throw in some situations
        /// </summary>
        public bool IgnoreErrors { get; set; }

        /// <summary>
        /// List of MemberInfoDiscovery
        /// </summary>
        public List<ITypeDiscovery> TypeDiscovery { get; }

        /// <summary>
        /// List of ValueLoader
        /// </summary>
        public List<IValueLoader> ValueLoader { get; }

        /// <summary>
        /// List of MemberInfoDiscovery
        /// </summary>
        public List<IMemberInfoDiscovery> MemberInfoDiscovery { get; }

        /// <summary>
        /// List of SetChildAction
        /// </summary>
        public List<ISetChild> SetChildAction { get; }

        /// <summary>
        /// Entity Root
        /// </summary>
        public Entity Root { get; set; }

        /// <summary>
        /// Root Type
        /// </summary>
        public Type RootType { get; }

        /// <summary>
        /// Value non typed (object)
        /// </summary>
        public object Value => Root?.Value;

        /// <summary>
        /// Create a non generic factory for complex types
        /// </summary>
        /// <param name="type">Type to create root</param>
        /// <param name="root">Entity Root</param>
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

        /// <summary>
        /// Add a type map: Can be used to create a concrete instances from a interfaces or abstract classes
        /// </summary>
        /// <typeparam name="TFrom">Type FROM</typeparam>
        /// <typeparam name="TTo">Type TO</typeparam>
        public void AddMapType<TFrom, TTo>()
        {
            mapTypes.Add(typeof(TFrom), typeof(TTo));
        }

        /// <summary>
        /// Method to add a error
        /// </summary>
        /// <param name="err">Error description</param>
        public void AddError(string err)
        {
            errors.Add(err);
        }

        /// <summary>
        /// Build the root entity to generate a real entity
        /// </summary>
        /// <returns>Return this instance to mantain fluent codification</returns>
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
         
        /// <summary>
        /// This method is exclusive for deserialization moment and should create a new entity to compose a expression
        /// new Entity("A") + new Entity("B");
        /// </summary>
        /// <param name="name">Name that is used as Raw</param>
        /// <param name="index">Index at expression</param>
        /// <returns>Return a new Entity to compose the expression</returns>
        public Entity GetEntity(string name, int index)
        {
            return new Entity(name);
        }

        #endregion
    }
}