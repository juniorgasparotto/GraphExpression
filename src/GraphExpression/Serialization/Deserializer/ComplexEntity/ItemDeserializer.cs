using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using GraphExpression.Utils;

namespace GraphExpression.Serialization
{
    //[DebuggerDisplay("{Raw}")]
    public class ItemDeserializerRoot<T> : ItemDeserializerRoot
    {
        public new T Entity => (T)base.Entity;

        public ItemDeserializerRoot() : this(0)
        {
        }

        public ItemDeserializerRoot(int id) : base(typeof(T), id)
        {
        }

        public static ItemDeserializerRoot<T> operator +(ItemDeserializerRoot<T> a, ItemDeserializer b)
        {
            a = (ItemDeserializerRoot<T>)((ItemDeserializer)a + b);
            a.Factory = new ComplexEntityFactoryDeserializer(a.Type);

            var factory = a.Factory;

            // Root + (A + B) + C
            // ---
            // A + B
            // Root + A -> Execute all
            // Root + C -> Execute all again, need separate already executed
            // ---

            a.Additions.RemoveAll(f => f.Executed);

            foreach (var e in a.Additions)
            {
                var itemDeserialize = factory
                            .ItemsDeserialize
                            .OfType<ISetChild>()
                            .LastOrDefault(f => f.CanSetChild(e.Source, e.Target));
                itemDeserialize.SetChild(e.Source, e.Target);
                e.Executed = true;
            }

            return a;
        }
    }

    public class ItemDeserializerRoot : ItemDeserializer
    {
        public new ComplexEntityFactoryDeserializer Factory { get; set; }
        public Type Type { get; }

        public ItemDeserializerRoot(Type type) : this(type, 0)
        {
            this.Type = type;
        }

        public ItemDeserializerRoot(Type type, int id) : base(id.ToString())
        {
            this.Type = type;
        }
    }

    [DebuggerDisplay("{Raw}")]
    public class ItemDeserializer
    {
        #region fields
        private readonly Dictionary<string, bool> propertyRead = new Dictionary<string, bool>();

        protected List<AddOperation> Additions;

        private Type entityType;        
        private MemberInfo memberInfo;
        private object entity;
        private ItemDeserializer root;
        private ComplexEntityFactoryDeserializer factory;
        private readonly Dictionary<string, ItemDeserializer> children;
        #endregion

        #region manage properties
        public IReadOnlyCollection<ItemDeserializer> Children => children.Values;
        public ItemDeserializer Parent { get; set; }
        private ItemDeserializerRoot Root => GetRoot();
        public ComplexEntityFactoryDeserializer Factory => GetFactory();
        #endregion

        public string Raw { get; }
        public object Entity => GetEntity();
        public Type EntityType => GetEntityType();
        public MemberInfo MemberInfo => GetMemberInfo();
        public string ValueRaw { get; private set; }
        public string MemberName { get; private set; }
        public bool IsPrimitive { get; private set; }
        public string ComplexEntityId { get; private set; }
        
        public ItemDeserializer(string raw)
        {
            this.children = new Dictionary<string, ItemDeserializer>();
            this.Raw = raw;
            this.ParseRaw();
        }

        public static ItemDeserializer operator +(ItemDeserializer a, ItemDeserializer b)
        {
            // Scenario 1
            // A + B + C + ( D + (E + F))
            // ----
            // A + B -> (1) - Create both context (A and B context)
            // A + C -> (3) - Set A in C
            // E + F -> (1) - Create both context (E and F context - news contexts)
            // D + E -> (2) - Set E -> D
            // A + D -> (4) - Set edges of D context in A to mantain execution order
            // ----

            // 1) Create both
            if (a.Additions == null && b.Additions == null)
                a.Additions = b.Additions = new List<AddOperation>();

            // 2) Set B in A
            else if (a.Additions == null)
                a.Additions = b.Additions;

            // 3) Set A in B
            else if (b.Additions == null)
                b.Additions = a.Additions;

            // 4) Set edges of B context in A to mantain execution order
            else if (a.Additions != b.Additions)
            {
                var childEdges = b.Additions;
                a.Additions.AddRange(childEdges);

                // Instance "edge" is not more necessary
                b.RemoveUselessEdges();

                b.Additions = a.Additions;
            }

            a.Additions.Add(new AddOperation(a, b));

            if (!a.children.ContainsKey(b.MemberName))
                a.children.Add(b.MemberName, b);
            else
                a.children[b.MemberName] = b;

            b.Parent = a;
            return a;
        }

        private void RemoveUselessEdges()
        {
            Additions.Clear();
            Additions = null;
        }

        private void ParseRaw()
        {
            if (string.IsNullOrWhiteSpace(Raw))
                return;

            int index = Raw.IndexOf(":");
            string member, value, hashCode = null;
            bool isPrimitive;

            if (index == -1)
            {
                value = null;

                var parts = Raw.Split('.');
                if (StringUtils.IsNumber(parts.LastOrDefault()))
                {
                    // IS COMPLEX: "Prop.12345"
                    hashCode = parts.LastOrDefault();

                    // ShowType = None
                    if (parts.Length == 1)
                        member = parts[0];
                    // ShowType = [OTHERS]
                    else
                        member = parts[parts.Length - 2];

                    isPrimitive = false;
                }
                else
                {
                    // IS PRIMITIVE AND IS NULL: "Prop"
                    member = Raw;
                    isPrimitive = true;
                }
            }
            else
            {
                // IS PRIMITIVE AND HAS VALUE: "Prop: value"
                member = Raw.Substring(0, index);
                value = Raw.Substring(index + 2); // consider space after colon ": "
                isPrimitive = true;
            }

            // ShowType = FullTypeName || TypeName
            var partsWithType = member.Split('.');
            if (partsWithType.Length > 0)
                member = partsWithType.LastOrDefault();

            this.MemberName = member;
            this.ValueRaw = value;
            this.IsPrimitive = isPrimitive;
            this.ComplexEntityId = hashCode;
        }

        private Type GetEntityType()
        {
            if (!propertyRead.ContainsKey(nameof(GetEntityType)))
            {
                propertyRead[nameof(GetEntityType)] = true;

                if (Factory.IsTyped)
                {
                    // with type associated
                    if (Parent == null)
                    {
                        entityType = Factory.TypeRoot;
                    }
                    else
                    {
                        var itemDeserialize = Factory
                            .ItemsDeserialize
                            .OfType<IGetEntityType>()
                            .LastOrDefault(f => f.CanGetEntityType(this));

                        if (itemDeserialize == null)
                            throw new Exception($"No class of type {nameof(IGetEntityType)} was found to {nameof(GetEntityType)}.");

                        entityType = itemDeserialize.GetEntityType(this);
                    }
                }
                else
                {
                    // without type associated
                    if (IsPrimitive)
                        entityType = typeof(string);
                    else
                        entityType = typeof(ExpandoObject);
                }

                if (Factory.MapTypes.ContainsKey(entityType))
                    entityType = Factory.MapTypes[entityType];
            }

            return entityType;
        }

        private object GetEntity()
        {
            if (!propertyRead.ContainsKey(nameof(GetEntity)))
            {
                propertyRead[nameof(GetEntity)] = true;

                var factory = Factory;
                var entityType = EntityType;
                var complexEntityId = ComplexEntityId;

                // 1) if null is because is not complex entity
                // 2) is necessary use the variable 'entity' 
                //    the ideia here is find the entity processed (entity loaded)
                if (complexEntityId != null)
                {
                    var find = factory.Entities.Where(f => f.ComplexEntityId == complexEntityId && f.entity != null);
                    if (find.Any())
                        return find.First().entity;
                }

                var itemDeserialize = Factory
                    .ItemsDeserialize
                    .OfType<IGetEntity>()
                    .LastOrDefault(f => f.CanGetEntity(this));

                this.entity = itemDeserialize?.GetEntity(this);
            }

            return entity;
        }

        private MemberInfo GetMemberInfo()
        {
            if (!propertyRead.ContainsKey(nameof(GetMemberInfo)))
            {
                propertyRead[nameof(GetMemberInfo)] = true;

                var itemDeserialize = Factory
                    .ItemsDeserialize
                    .OfType<IGetMemberInfo>()
                    .LastOrDefault(f => f.CanGetMemberInfo(this));

                this.memberInfo = itemDeserialize?.GetMemberInfo(this);
            }

            return memberInfo;
        }

        private ItemDeserializerRoot GetRoot()
        {
            if (root == null)
            {
                var p = Parent;

                if (p == null)
                {
                    root = this;
                }
                else
                {
                    while (p != null)
                    {
                        if (p.Parent == null)
                            break;
                        p = p.Parent;
                    }
                    root = p;
                }
            }
            return (ItemDeserializerRoot)root;
        }

        private ComplexEntityFactoryDeserializer GetFactory()
        {
            if (factory == null)
                factory = Root.Factory;
            return factory;
        }

        #region nested classes

        [DebuggerDisplay("{ToString()}")]
        public class AddOperation
        {
            public ItemDeserializer Source { get; }
            public ItemDeserializer Target { get; }
            public bool Executed { get; set;  }

            public AddOperation(ItemDeserializer source, ItemDeserializer target)
            {
                this.Source = source;
                this.Target = target;
            }

            public override string ToString()
            {
                return $"{Source?.MemberName}, {Target?.MemberName}";
            }
        }

        #endregion
    }
}
