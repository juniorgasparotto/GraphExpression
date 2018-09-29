using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections;
using Mono.Reflection;
using GraphExpression.Utils;
using System.ComponentModel;

namespace GraphExpression.Serialization
{
    [DebuggerDisplay("{Raw}")]
    public class ItemDeserializer
    {
        #region fields
        private readonly Dictionary<string, bool> propertyRead = new Dictionary<string, bool>();
        private bool isInitiated = false;

        private Type entityType;
        private MemberInfo memberInfo;
        private MemberInfo memberSetter;
        private string membeName;
        private string valueRaw;
        private bool isPrimitive;
        private string complexEntityId;
        private object entity;
        #endregion

        #region manage properties
        public List<ItemDeserializer> Children { get; set; }
        public ItemDeserializer Parent { get; set; }
        public ComplexEntityFactoryDeserializer EntityFactory { get; set; }
        #endregion

        public string Raw { get; }

        public object Entity
        {
            get
            {
                if (!propertyRead.ContainsKey(nameof(Entity)))
                {
                    propertyRead[nameof(Entity)] = true;
                    SetEntity();
                }

                return entity;
            }
        }

        public Type EntityType
        {
            get
            {
                if (!propertyRead.ContainsKey(nameof(EntityType)))
                {
                    propertyRead[nameof(EntityType)] = true;

                    if (EntityFactory.IsTyped)
                    {
                        // with type associated
                        if (Parent == null)
                        {
                            entityType = EntityFactory.TypeRoot;
                        }
                        else if (MemberName.StartsWith("[") && Parent.Entity is Array array)
                        {
                            entityType = array.GetType().GetElementType();
                        }
                        else if (MemberName.StartsWith("[") && Parent.Entity is IList list)
                        {
                            entityType = list.GetType().GenericTypeArguments[0];
                        }
                        else if (MemberName.StartsWith("[") && Parent.Entity is IDictionary dic)
                        {
                            var genericTypes =  dic.GetType().GetGenericArguments();
                            entityType = typeof(KeyValuePair<,>).MakeGenericType(genericTypes);
                        }
                        else if (MemberInfoSetter != null)
                        {
                            entityType = ReflectionUtils.GetMemberType(MemberInfoSetter);
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
        }

        public MemberInfo MemberInfo
        {
            get
            {
                if (!propertyRead.ContainsKey(nameof(MemberInfo))
                    && EntityFactory.IsTyped
                    && MemberName != null
                    && !MemberName.StartsWith("[") // ignore [0] members
                    && Parent.EntityType != null)
                {
                    propertyRead[nameof(MemberInfo)] = true;

                    memberInfo = Parent.EntityType
                           .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                           .Where(f => f.Name == MemberName)
                           .FirstOrDefault();

                    memberInfo = memberInfo ?? throw new Exception($"Member not exists {MemberName} in {Parent.EntityType.FullName}");
                }

                return memberInfo;
            }
        }

        public MemberInfo MemberInfoSetter
        {
            get
            {
                if (!propertyRead.ContainsKey(nameof(MemberInfoSetter)))
                {
                    propertyRead[nameof(MemberInfoSetter)] = true;

                    if (MemberInfo is PropertyInfo prop)
                    {
                        if (prop.GetSetMethod(true) != null)
                            memberSetter = prop;
                        else
                            memberSetter = prop.GetBackingField();
                    }
                    else if (MemberInfo is FieldInfo field)
                    {
                        memberSetter = field;
                    }
                }

                return memberSetter;
            }
        }

        public string ValueRaw
        {
            get
            {
                if (!propertyRead.ContainsKey(nameof(ValueRaw)))
                {
                    propertyRead[nameof(ValueRaw)] = true;
                    ParseRaw();
                }

                return valueRaw;
            }
        }

        public string MemberName
        {
            get
            {
                if (!propertyRead.ContainsKey(nameof(MemberName)))
                {
                    propertyRead[nameof(MemberName)] = true;
                    ParseRaw();
                }
                return membeName;
            }
        }

        public bool IsPrimitive
        {
            get
            {
                if (!propertyRead.ContainsKey(nameof(IsPrimitive)))
                {
                    propertyRead[nameof(IsPrimitive)] = true;
                    ParseRaw();
                }

                return isPrimitive;
            }
        }

        public string ComplexEntityId
        {
            get
            {
                if (!propertyRead.ContainsKey(nameof(ComplexEntityId)))
                {
                    propertyRead[nameof(ComplexEntityId)] = true;
                    ParseRaw();
                }
                return complexEntityId;
            }
        }
                
        public ItemDeserializer(string raw)
        {
            this.Raw = raw;
            this.Children = new List<ItemDeserializer>();
        }

        public static ItemDeserializer operator +(ItemDeserializer a, ItemDeserializer b)
        {
            //Debug.WriteLine(a.Raw);
            //Debug.WriteLine(b.Raw);

            var factory = a.EntityFactory;
            if (factory.DeserializationTime == DeserializationTime.Creation)
            {
                a.Children.Add(b);
                b.Parent = a;
            }
            else if (factory.DeserializationTime == DeserializationTime.AssignChildInParent)
            {
                if (a.Entity is ExpandoObject && a.Entity is IDictionary<string, object> expando)
                {
                    expando.Add(b.MemberName, b.Entity);
                }
                else if (a.Entity is IDictionary dic)
                {
                    if (b.MemberName.StartsWith("["))
                    {
                        var key = b.EntityType.GetProperty("Key").GetValue(b.Entity);
                        var value = b.EntityType.GetProperty("Value").GetValue(b.Entity);

                        var add = a.EntityType.GetMethod("Add", new[] { key.GetType(), value.GetType() });
                        add.Invoke(a.Entity, new object[] { key, value });
                    }
                    else if (b.MemberName != "Comparer" && b.MemberName != "Count")
                    {
                        SetChildMemberInfo(a, b);
                    }
                }
                else if (a.Entity is Array array && b.MemberName.StartsWith("["))
                {
                    // [0] -> simple array
                    // [0, 0, 0] -> multidimentional
                    long[] indexes = GetArrayIndexesByString(b.MemberName);
                    array.SetValue(b.Entity, indexes);
                }
                else if (a.Entity is IList list && b.MemberName.StartsWith("["))
                {
                    // ignore other properties from the IList (Capacity, Count)
                    list.Add(b.Entity);
                }
                else
                {
                    SetChildMemberInfo(a, b);
                }
            }

            return a;
        }

        private static long[] GetArrayIndexesByString(string strIndexes)
        {
            var indexesSplit = strIndexes.Substring(1, strIndexes.Length - 2);
            var indexes = indexesSplit
                            .Split(',')
                            .Select(f => Convert.ToInt64(f))
                            .ToArray();
            return indexes;
        }

        private static void SetChildMemberInfo(ItemDeserializer a, ItemDeserializer b)
        {
            if (a.Entity == null && b.Entity != null)
            {
                var error = $"An instance of type '{a.EntityType.FullName}' contains value, but not created. Make sure it is an interface or an abstract class, if so, set up a corresponding concrete class in the '{nameof(ComplexEntityFactoryDeserializer)}.{nameof(ComplexEntityFactoryDeserializer.MapTypes)}' configuration.";
                if (!a.EntityFactory.IgnoreErrors)
                    throw new Exception(error);
                else 
                    a.EntityFactory.AddError(error);
                return;
            }

            if (b.MemberInfoSetter is PropertyInfo prop)
                prop.SetValue(a.Entity, b.Entity);
            else if (b.MemberInfoSetter is FieldInfo field)
                field.SetValue(a.Entity, b.Entity);
        }

        private void ParseRaw()
        {
            if (isInitiated)
                return;

            isInitiated = true;

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

            this.membeName = member;
            this.valueRaw = value;
            this.isPrimitive = isPrimitive;
            this.complexEntityId = hashCode;
        }

        public void SetEntity()
        {
            if (ReflectionUtils.IsAnonymousType(EntityType))
            {
                // anonymous type with type associated
                entity = new ExpandoObject();
            }
            else
            {
                if (IsPrimitive && ValueRaw != null)
                {
                    entity = TypeDescriptor.GetConverter(EntityType).ConvertFromInvariantString(ValueRaw);
                }
                else if (ComplexEntityId != null)
                {
                    // 1) if null is because is not a complex entity
                    // 2) not use property because is not necessary.
                    //    the ideia here is find the entity processed
                    var find = EntityFactory.Entities.Where(f => f.ComplexEntityId == ComplexEntityId && f.entity != null);
                    if (find.Any())
                    {
                        entity = find.First().entity;
                    }
                    else if (EntityType == typeof(ExpandoObject))
                    {
                        entity = new ExpandoObject();
                    }
                    else if (EntityType.IsArray)
                    {
                        var lastItem = Children.LastOrDefault();
                        object[] indexes;

                        if (lastItem != null)
                        {
                            var longs = GetArrayIndexesByString(lastItem.MemberName);
                            indexes = new object[longs.Length];
                            // 1) array needs a INT index, long throw exceptions
                            // 2) Need add + 1 because the seralize contain the position and
                            //    the array constructors needs the lenght from each dimentions
                            //    [0] -> pos = 0 | and | lenght = 1
                            for (var i = 0; i < longs.Length; i++)
                                indexes[i] = (int)longs[i] + 1;
                        }
                        else
                        {
                            indexes = new object[] { 0 };
                        }

                        entity = Activator.CreateInstance(EntityType, indexes);
                    }
                    else
                    {
                        if (EntityType.GetConstructors().Where(f => f.GetParameters().Length == 0).Any())
                        {
                            entity = Activator.CreateInstance(EntityType);
                        }
                        else
                        {
                            if (!EntityType.IsInterface && !EntityType.IsAbstract)
                                entity = FormatterServices.GetUninitializedObject(EntityType);
                        }
                    }
                }
            }
        }
    }
}
