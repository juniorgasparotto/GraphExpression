using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using GraphExpression.Utils;

namespace GraphExpression
{
    /// <summary>
    /// Class that represents a entity in an expression: new Entity("A") + new Entity("B")
    /// </summary>
    [DebuggerDisplay(@"\{{Raw}\}")]
    public class Entity
    {
        #region fields
        private readonly Dictionary<string, bool> propertyRead = new Dictionary<string, bool>();

        private Type type;        
        private MemberInfo memberInfo;
        private object value;
        private readonly Dictionary<string, Entity> children;
        #endregion

        #region manage properties

        /// <summary>
        /// Determines the complex factory
        /// </summary>
        public IEntityFactory Factory { get; set; }

        /// <summary>
        /// Parent entity
        /// </summary>
        public Entity Parent { get; set; }

        /// <summary>
        /// All children
        /// </summary>
        public IReadOnlyCollection<Entity> Children => children.Values;

        /// <summary>
        /// Indexer to find a child by index
        /// </summary>
        /// <param name="index">Child Position</param>
        /// <returns>Return a child</returns>
        public Entity this[int index] => children.Values.ElementAt(index);

        /// <summary>
        /// Indexer to find a child by member name
        /// </summary>
        /// <param name="key">MemberName</param>
        /// <returns>Return a child</returns>
        public Entity this[string key] => children[key];

        /// <summary>
        /// Accumulates the operations that were done in the expression, keeping the same order of execution
        /// </summary>
        public List<Operation> Operations { get; private set; }

        #endregion

        /// <summary>
        /// Content that must contain entity information in the format: Name: Value
        /// </summary>
        public string Raw { get; }

        /// <summary>
        /// Return the type based in your name ou parent type
        /// </summary>
        public Type Type => GetValueType();

        /// <summary>
        /// Return the member info if the name is a exists memberInfo
        /// </summary>
        public MemberInfo MemberInfo => GetMemberInfo();

        /// <summary>
        /// Return the entity name, can be a MemberName, array position "[0]" or anything
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Return the value from this entity
        /// </summary>
        public object Value => GetValue();

        /// <summary>
        /// Return the value as string
        /// </summary>
        public string ValueRaw { get; private set; }

        /// <summary>
        /// Check if value is primitive. Is primitive when exists colon after name: Name: ValuePrimitive
        /// </summary>
        public bool IsPrimitive { get; private set; }

        /// <summary>
        /// Check if value is complex type. Is complex when not exists a colon and exists a numeric ID that represents the instance (Name.ID OR only "ID"): MemberName.1
        /// </summary>
        public string ComplexEntityId { get; private set; }

        /// <summary>
        /// Create a new entity by raw
        /// </summary>
        /// <param name="raw">Raw value</param>
        public Entity(string raw)
        {
            this.children = new Dictionary<string, Entity>();
            this.Raw = raw;
            this.ParseRaw();
        }

        /// <summary>
        /// Create a new entity by name and value, is composed auto to: Name: Value
        /// </summary>
        /// <param name="name">Name of entity</param>
        /// <param name="value">Value of entity</param>
        public Entity(string name, string value)
            : this($"{name}: {value}")
        {
        }

        /// <summary>
        /// Create a new complex entity by name and value, is composed auto to: Name.1
        /// </summary>
        /// <param name="name">Name of entity</param>
        /// <param name="complexEntityId">ID of entity</param>
        public Entity(string name, int complexEntityId)
            : this($"{name}.{complexEntityId}")
        {
        }

        /// <summary>
        /// Create a new complex entity without name: "1"
        /// </summary>
        /// <param name="complexEntityId">ID of entity</param>
        public Entity(int complexEntityId) 
            : this(complexEntityId.ToString())
        {
        }

        /// <summary>
        /// Operator that create a TREE information about the expression
        /// </summary>
        /// <param name="a">Left of Expression</param>
        /// <param name="b">Right of Expression</param>
        /// <returns></returns>
        public static Entity operator +(Entity a, Entity b)
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
            if (a.Operations == null && b.Operations == null)
                a.Operations = b.Operations = new List<Operation>();

            // 2) Set B in A
            else if (a.Operations == null)
                a.Operations = b.Operations;

            // 3) Set A in B
            else if (b.Operations == null)
                b.Operations = a.Operations;

            // 4) Set edges of B context in A to mantain execution order
            else if (a.Operations != b.Operations)
            {
                var childEdges = b.Operations;
                a.Operations.AddRange(childEdges);

                // Instance "edge" is not more necessary
                b.RemoveUselessEdges();

                b.Operations = a.Operations;
            }

            a.Operations.Add(new Operation(a, b));

            if (!a.children.ContainsKey(b.Name))
                a.children.Add(b.Name, b);
            else
                a.children[b.Name] = b;

            b.Parent = a;
            return a;
        }

        private void RemoveUselessEdges()
        {
            Operations.Clear();
            Operations = null;
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

            this.Name = member;
            this.ValueRaw = value;
            this.IsPrimitive = isPrimitive;
            this.ComplexEntityId = hashCode;
        }

        private Type GetValueType()
        {
            if (Factory != null 
                && !propertyRead.ContainsKey(nameof(GetValueType)))
            {
                propertyRead[nameof(GetValueType)] = true;

                if (Factory.IsTyped)
                {
                    // with type associated
                    if (Parent == null)
                    {
                        type = Factory.RootType;
                    }
                    else
                    {
                        var itemDeserialize = Factory
                            .TypeDiscovery
                            .LastOrDefault(f => f.CanDiscovery(this));

                        if (itemDeserialize == null)
                            throw new Exception($"No class of type {nameof(ITypeDiscovery)} was found to {nameof(GetValueType)}.");

                        type = itemDeserialize.GetEntityType(this);
                    }
                }
                else
                {
                    // without type associated
                    if (IsPrimitive)
                        type = typeof(string);
                    else
                        type = typeof(ExpandoObject);
                }

                if (Factory.MapTypes.ContainsKey(type))
                    type = Factory.MapTypes[type];
            }

            return type;
        }

        private object GetValue()
        {
            // While not build yeat, use the valueRaw info
            if (Factory == null)
                return ValueRaw;

            if (!propertyRead.ContainsKey(nameof(GetValue)))
            {
                propertyRead[nameof(GetValue)] = true;

                var factory = Factory;
                var entityType = Type;
                var complexEntityId = ComplexEntityId;

                // 1) if null is because is not complex entity
                // 2) is necessary use the variable 'entity' 
                //    the ideia here is find the entity processed (entity loaded)
                if (complexEntityId != null)
                {
                    var find = factory.Entities.Where(f => f.ComplexEntityId == complexEntityId && f.value != null);
                    if (find.Any())
                        return find.First().value;
                }

                var itemDeserialize = Factory
                    .ValueLoader
                    .LastOrDefault(f => f.CanLoad(this));

                this.value = itemDeserialize?.GetValue(this);
            }

            return value;
        }

        private MemberInfo GetMemberInfo()
        {
            if (Factory != null
                && !propertyRead.ContainsKey(nameof(GetMemberInfo)))
            {
                propertyRead[nameof(GetMemberInfo)] = true;

                var itemDeserialize = Factory
                    .MemberInfoDiscovery
                    .LastOrDefault(f => f.CanDiscovery(this));

                this.memberInfo = itemDeserialize?.GetMemberInfo(this);
            }

            return memberInfo;
        }

        #region nested classes

        /// <summary>
        /// Represent a operation in math expression: A + B
        /// </summary>
        [DebuggerDisplay("{ToString()}")]
        public class Operation
        {
            /// <summary>
            /// Left side
            /// </summary>
            public Entity Source { get; }

            /// <summary>
            /// Right side
            /// </summary>
            public Entity Target { get; }

            /// <summary>
            /// IF TRUE, the operation has executed in any build moment
            /// </summary>
            public bool Executed { get; set;  }

            /// <summary>
            /// Create a operation instance
            /// </summary>
            /// <param name="source">Left side</param>
            /// <param name="target">Right side</param>
            public Operation(Entity source, Entity target)
            {
                this.Source = source;
                this.Target = target;
            }

            /// <summary>
            /// Operation to string
            /// </summary>
            /// <returns>Return operation to string</returns>
            public override string ToString()
            {
                return $"{Source?.Name}, {Target?.Name}";
            }
        }

        #endregion
    }
}
