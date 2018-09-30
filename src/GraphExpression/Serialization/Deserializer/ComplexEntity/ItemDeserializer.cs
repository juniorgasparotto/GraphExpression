using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using GraphExpression.Utils;

namespace GraphExpression.Serialization
{
    [DebuggerDisplay("{Raw}")]
    public class ItemDeserializer
    {
        #region fields
        private readonly Dictionary<string, bool> propertyRead = new Dictionary<string, bool>();

        private Type entityType;
        private MemberInfo memberInfo;
        private object entity;
        #endregion

        #region manage properties
        public List<ItemDeserializer> Children { get; set; }
        public ItemDeserializer Parent { get; set; }
        public ComplexEntityFactoryDeserializer EntityFactory { get; set; }
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
            this.Raw = raw;
            this.Children = new List<ItemDeserializer>();
            this.ParseRaw();
        }

        public static ItemDeserializer operator +(ItemDeserializer a, ItemDeserializer b)
        {   
            var factory = a.EntityFactory;
            if (factory.DeserializationTime == DeserializationTime.Creation)
            {
                a.Children.Add(b);
                b.Parent = a;
            }
            else if (factory.DeserializationTime == DeserializationTime.AssignChildInParent)
            {
                var itemDeserialize = factory
                                .ItemsDeserialize
                                .OfType<ISetChild>()
                                .LastOrDefault(f => f.CanSetChild(a, b));

                if (itemDeserialize != null)
                    itemDeserialize.SetChild(a, b);
            }

            //Debug.WriteLine(a.Raw);
            //Debug.WriteLine(b.Raw);

            return a;
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

                if (EntityFactory.IsTyped)
                {
                    // with type associated
                    if (Parent == null)
                    {
                        entityType = EntityFactory.TypeRoot;
                    }
                    else
                    {
                        var itemDeserialize = EntityFactory
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

                if (EntityFactory.MapTypes.ContainsKey(entityType))
                    entityType = EntityFactory.MapTypes[entityType];
            }

            return entityType;
        }

        private object GetEntity()
        {
            if (!propertyRead.ContainsKey(nameof(GetEntity)))
            {
                propertyRead[nameof(GetEntity)] = true;

                var factory = EntityFactory;
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

                var itemDeserialize = EntityFactory
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

                var itemDeserialize = EntityFactory
                    .ItemsDeserialize
                    .OfType<IGetMemberInfo>()
                    .LastOrDefault(f => f.CanGetMemberInfo(this));

                this.memberInfo = itemDeserialize?.GetMemberInfo(this);
            }

            return memberInfo;
        }
    }
}
