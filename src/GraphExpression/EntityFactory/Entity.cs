using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using GraphExpression.Utils;

namespace GraphExpression
{
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
        public IEntityFactory Factory { get; set; }
        public Entity Parent { get; set; }
        public IReadOnlyCollection<Entity> Children => children.Values;
        public Entity this[int index] => children.Values.ElementAt(index);
        public Entity this[string key] => children[key];
        public List<Operation> Operations { get; private set; }
        #endregion

        public string Raw { get; }
        public Type Type => GetValueType();
        public MemberInfo MemberInfo => GetMemberInfo();
        public string Name { get; private set; }
        public object Value => GetValue();
        public string ValueRaw { get; private set; }
        public bool IsPrimitive { get; private set; }
        public string ComplexEntityId { get; private set; }
        
        public Entity(string raw)
        {
            this.children = new Dictionary<string, Entity>();
            this.Raw = raw;
            this.ParseRaw();
        }

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
                            .ItemsDeserialize
                            .OfType<IGetType>()
                            .LastOrDefault(f => f.CanGetEntityType(this));

                        if (itemDeserialize == null)
                            throw new Exception($"No class of type {nameof(IGetType)} was found to {nameof(GetValueType)}.");

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
                    .ItemsDeserialize
                    .OfType<IGetValue>()
                    .LastOrDefault(f => f.CanGetValue(this));

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
                    .ItemsDeserialize
                    .OfType<IGetMemberInfo>()
                    .LastOrDefault(f => f.CanGetMemberInfo(this));

                this.memberInfo = itemDeserialize?.GetMemberInfo(this);
            }

            return memberInfo;
        }

        #region nested classes

        [DebuggerDisplay("{ToString()}")]
        public class Operation
        {
            public Entity Source { get; }
            public Entity Target { get; }
            public bool Executed { get; set;  }

            public Operation(Entity source, Entity target)
            {
                this.Source = source;
                this.Target = target;
            }

            public override string ToString()
            {
                return $"{Source?.Name}, {Target?.Name}";
            }
        }

        #endregion
    }
}
