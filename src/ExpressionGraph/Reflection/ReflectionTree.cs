using ExpressionGraph.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ExpressionGraph.Reflection
{
    public class ReflectionTree
    {
        List<Type> noneMemberForTypes = new List<Type>()
            {
                typeof(Boolean),
                typeof(Char),
                typeof(SByte),
                typeof(Byte),
                typeof(Int16),
                typeof(UInt16),
                typeof(Int32),
                typeof(UInt32),
                typeof(Int64),
                typeof(UInt64),
                typeof(Single),
                typeof(Double),
                typeof(Decimal),
                typeof(DateTime),
                typeof(String),
                typeof(System.Text.StringBuilder),
                typeof(TimeSpan),
                typeof(DateTimeOffset),
                typeof(IntPtr),
                typeof(UIntPtr),
                typeof(TimeZone),
                typeof(Uri),
                typeof(Guid),
                typeof(Action),
                typeof(Action<,>),
            };

        List<string> noneMemberForTypesNames = new List<string>()
            {
                "^System.RuntimeType$",
                "^System.Action$",
                "^System.Func$",
                "^System.Action`",
                "^System.Func`"
            };

        private Expression<ReflectedInstance> _expressions;
        private SettingsFlags _settingsAttributes;
        private object _object;

        // Readers for types and member
        private List<DefinitionOfTypeReader> _typesReaders;
        private List<DefinitionOfClassMemberReader<FieldInfo>> _fieldsReaders;
        private List<DefinitionOfClassMemberReader<PropertyInfo>> _propertysReaders;
        private List<DefinitionOfClassMemberReader<MethodInfo>> _methodsReaders;

        // Readers for get values
        private List<DefinitionOfMethodValueReader<PropertyInfo>> _propertyValueReaders;
        private List<DefinitionOfMethodValueReader<MethodInfo>> _methodValueReaders;

        private long maxItems;

        public ReflectionTree(object obj)
        {
            this._object = obj;

            //set default max items 
            this.maxItems = 1000;

            // Readers for types and member AND Add the default selectors
            this._typesReaders = new List<DefinitionOfTypeReader>();
            this._typesReaders.Add(new DefinitionOfTypeReader() { CanRead = (objParam) => true, Get = null });

            this._fieldsReaders = new List<DefinitionOfClassMemberReader<FieldInfo>>();
            this._fieldsReaders.Add(new DefinitionOfClassMemberReader<FieldInfo>() { CanRead = (objParam, type) => true, Get = null });

            this._propertysReaders = new List<DefinitionOfClassMemberReader<PropertyInfo>>();
            this._propertysReaders.Add(new DefinitionOfClassMemberReader<PropertyInfo>() { CanRead = (objParam, type) => true, Get = null });

            this._methodsReaders = new List<DefinitionOfClassMemberReader<MethodInfo>>();
            this._methodsReaders.Add(new DefinitionOfClassMemberReader<MethodInfo>() { CanRead = (objParam, type) => true, Get = null });

            // Readers for get values
            this._propertyValueReaders = new List<DefinitionOfMethodValueReader<PropertyInfo>>();
            this._methodValueReaders = new List<DefinitionOfMethodValueReader<MethodInfo>>();

            this.DefaultSettingsToTypes();
            this.DefaultSettingsToFields();
            this.DefaultSettingsToProperties();
            this.DefaultSettingsToMethods();
        }

        public ReflectionTree Settings(SettingsFlags settingsAttr)
        {
            this._settingsAttributes = settingsAttr;
            return this;
        }

        public ReflectionTree SetMaxItems(long maxItems)
        {
            this.maxItems = maxItems;
            return this;
        }

        public ReflectedInstance Reflect()
        {
            return this.GetInstance(this._object, "");
        }

        public IEnumerable<ReflectedInstance> ReflectTree()
        {
            return this.Query().ToEntities(false);
        }

        public Expression<ReflectedInstance> Query()
        {
            var instanceRoot = new List<ReflectedInstance>() { GetInstance(this._object, "") };
            this._expressions = ExpressionBuilder<ReflectedInstance>
            .Build
            (
                instanceRoot,
                f => GetChildren(f),
                true,
                false,
                false,
                f => 
                {
                    return ObjectToString(f.Entity.Object, f.Entity.ObjectType, f.Entity.ContainerName, f.Id, f.HasChildren());
                },
                this.maxItems
            )
            .FirstOrDefault();

            return _expressions;
        }

        private List<ReflectedInstance> GetChildren(ReflectedInstance instance)
        {
            var list = new List<ReflectedInstance>();

            var fields = instance.GetAllFields().ToList();
            foreach (var field in fields)
            { 
                list.Add(GetInstance(field.Value, field.Name));
            }
  
            var properties = instance.GetAllProperties().ToList();
            foreach (var property in properties)
            {
                if (property.Values != null)
                {
                    foreach (var value in property.Values)
                    {
                        var parameterStr = "";
                        var showParameterName = _settingsAttributes.HasFlag(SettingsFlags.ShowParameterName);
                        if (value.Parameters != null)
                        {
                            if (value.Parameters.Length == 1 && value.Parameters[0].Value is ArrayKey)
                            {
                                parameterStr = value.Parameters[0].Value.ToString();
                            }
                            else
                            { 
                                foreach (var param in value.Parameters)
                                {
                                    parameterStr += parameterStr == "" ? "" : ", ";
                                    parameterStr += ObjectToString(param.Value, param.Value.GetType(), showParameterName ? param.Name : null);
                                }
                            }

                            parameterStr = "[" + parameterStr + "]";
                        }

                        list.Add(GetInstance(value.Value, property.Name + parameterStr));
                    }
                }
            }

            var methods = instance.GetAllMethods().ToList();
            foreach (var method in methods)
            {
                if (method.Values != null)
                {
                    foreach (var value in method.Values)
                    {
                        var parameterStr = "";
                        var showParameterName = _settingsAttributes.HasFlag(SettingsFlags.ShowParameterName);
                        if (value.Parameters != null)
                        {
                            if (value.Parameters.Length == 1 && value.Parameters[0].Value is ArrayKey)
                            {
                                parameterStr = value.Parameters[0].Value.ToString();
                            }
                            else
                            {
                                foreach (var param in value.Parameters)
                                {
                                    parameterStr += parameterStr == "" ? "" : ", ";
                                    parameterStr += ObjectToString(param.Value, param.Value.GetType(), showParameterName ? param.Name : null);
                                }
                            }

                            parameterStr = "[" + parameterStr + "]";
                        }

                        list.Add(GetInstance(value.Value, method.Name + parameterStr));
                    }
                }
            }

            return list;
        }

        private string ObjectToString(object obj, Type type, string containerName = null, long id = -1, bool hasChildren = false)
        {
            string output;
            if (obj != null && hasChildren)
            {
                var showFullName = _settingsAttributes.HasFlag(SettingsFlags.ShowFullNameOfType);
                output = ReflectionUtils.CSharpName(type, showFullName) + "_" + id;
            }
            else
            { 
                output = Utils.ToLiteral(obj);
            }

            if (!string.IsNullOrWhiteSpace(containerName))
                output = containerName + ": " + output;

            return output;
        }

        //private string ObjectToString(object obj, Type type, string containerName = null, long id = -1)
        //{
        //    if (obj != null && id != -1)
        //    {
        //        var showFullName = _settingsAttributes.HasFlag(SettingsFlags.ShowFullNameOfType);
        //        if (CanGetAnyMembers(obj, type))
        //            return ReflectionUtils.CSharpName(type, showFullName) + "_" + id;
        //    }

        //    string output = Utils.ToLiteral(obj);

        //    if (string.IsNullOrWhiteSpace(containerName))
        //        return output;

        //    return containerName + ": " + output;
        //}

        private ReflectedInstance GetInstance(object obj, string containerName)
        {
            var reflectionUnit = new ReflectionUnit();
            reflectionUnit.TypesReader = 
                (objParam) =>
                {
                    IEnumerable<Type> types = null;
                    var selector = this._typesReaders.LastOrDefault(f => f.CanRead != null && f.CanRead(objParam));
                    if (selector != null && selector.Get != null)
                        types = selector.Get(objParam);

                    return types;
                };

            reflectionUnit.FieldsReader = 
                (objParam, type) =>
                {
                    IEnumerable<FieldInfo> fields = null;
                    var selector = this.GetFieldsReader(objParam, type);
                    if (selector != null)
                        fields = selector.Get(objParam, type);

                    return fields;
                };
            
            reflectionUnit.PropertiesReader = 
                (objParam, type) =>
                {
                    IEnumerable<PropertyInfo> properties = null;
                    var selector = this.GetPropertiesReader(objParam, type);
                    if (selector != null)
                        properties = selector.Get(objParam, type);

                    return properties;
                };

            reflectionUnit.MethodsReader = 
                (objParam, type) =>
                {
                    IEnumerable<MethodInfo> methods = null;
                    var selector = this.GetMethodsReader(objParam, type);
                    if (selector != null)
                        methods = selector.Get(objParam, type);

                    return methods;
                };

            reflectionUnit.PropertyValueReaders = _propertyValueReaders;
            reflectionUnit.MethodValueReaders = _methodValueReaders;

            return reflectionUnit.GetInstance(obj, containerName);
        }

        #region Fluent - Get types and members and values

        /// <summary>
        /// Set the custom selector to get fields
        /// </summary>
        /// <param name="selector">Selector of the fields</param>
        /// <returns></returns>
        public ReflectionTree SelectTypes(Func<object, IEnumerable<Type>> selector)
        {
            this._typesReaders[0].Get = selector;
            return this;
        }

        /// <summary>
        /// Set the custom selector to get fields and choose the specific filter to apply the selector 
        /// </summary>
        /// <param name="filter">Specify the filter to apply selector</param>
        /// <param name="selector">Selector of the fields</param>
        /// <returns></returns>
        public ReflectionTree SelectTypes(Func<object, bool> filter, Func<object, IEnumerable<Type>> selector)
        {
            var reader = new DefinitionOfTypeReader();
            reader.CanRead = filter;
            reader.Get = selector;
            this._typesReaders.Add(reader);
            return this;
        }

        /// <summary>
        /// Set bindingAttr to get fields
        /// </summary>
        /// <param name="bindingAttr">Specific binding to return fields</param>
        /// <returns></returns>
        public ReflectionTree SelectFields(BindingFlags bindingAttr)
        {
            this._fieldsReaders[0].Get = (value, type) => type.GetFields(bindingAttr);
            return this;
        }

        /// <summary>
        /// Set the custom selector to get fields
        /// </summary>
        /// <param name="selector">Selector of the fields</param>
        /// <returns></returns>
        public ReflectionTree SelectFields(Func<object, Type, FieldInfo[]> selector)
        {
            this._fieldsReaders[0].Get = selector;
            return this;
        }

        /// <summary>
        /// Set the custom selector to get fields and choose the specific filter to apply the selector 
        /// </summary>
        /// <param name="filter">Specify the filter to apply selector</param>
        /// <param name="selector">Selector of the fields</param>
        /// <returns></returns>
        public ReflectionTree SelectFields(Func<object, Type, bool> filter, Func<object, Type, IEnumerable<FieldInfo>> selector)
        {
            var reader = new DefinitionOfClassMemberReader<FieldInfo>();
            reader.CanRead = filter;
            reader.Get = selector;
            _fieldsReaders.Add(reader);
            return this;
        }

        /// <summary>
        /// Set bindingAttr to get properties
        /// </summary>
        /// <param name="bindingAttr">Specific binding to return properties</param>
        /// <returns></returns>
        public ReflectionTree SelectProperties(BindingFlags bindingAttr)
        {
            this._propertysReaders[0].Get =
                (value, type) =>
                {
                    return type.GetProperties(bindingAttr);
                };
            return this;
        }

        /// <summary>
        /// Set the custom selector to get properties
        /// </summary>
        /// <param name="selector">Selector of the properties</param>
        /// <returns></returns>
        public ReflectionTree SelectProperties(Func<object, Type, IEnumerable<PropertyInfo>> selector)
        {
            this._propertysReaders[0].Get = selector;
            return this;
        }

        /// <summary>
        /// Set the custom selector to get properties and choose the specific filter to apply the selector 
        /// </summary>
        /// <param name="filter">Specify the filter to apply selector</param>
        /// <param name="selector">Selector of the properties</param>
        /// <returns></returns>
        public ReflectionTree SelectProperties(Func<object, Type, bool> filter, Func<object, Type, IEnumerable<PropertyInfo>> selector)
        {
            var reader = new DefinitionOfClassMemberReader<PropertyInfo>();
            reader.CanRead = filter;
            reader.Get = selector;
            this._propertysReaders.Add(reader);
            return this;
        }

        /// <summary>
        /// Set bindingAttr to get properties
        /// </summary>
        /// <param name="bindingAttr">Specific binding to return methods</param>
        /// <returns></returns>
        public ReflectionTree SelectMethods(BindingFlags bindingAttr)
        {
            this._methodsReaders[0].Get = (value, type) => type.GetMethods(bindingAttr);
            return this;
        }

        /// <summary>
        /// Set the custom selector to get methods
        /// </summary>
        /// <param name="selector">Selector of the methods</param>
        /// <returns></returns>
        public ReflectionTree SelectMethods(Func<object, Type, IEnumerable<MethodInfo>> selector)
        {
            this._methodsReaders[0].Get = selector;
            return this;
        }

        /// <summary>
        /// Set the custom selector to get methods and choose the specific filter to apply the selector 
        /// </summary>
        /// <param name="filter">Specify the filter to apply selector</param>
        /// <param name="selector">Selector of the methods</param>
        /// <returns></returns>
        public ReflectionTree SelectMethods(Func<object, Type, bool> filter, Func<object, Type, IEnumerable<MethodInfo>> selector)
        {
            var reader = new DefinitionOfClassMemberReader<MethodInfo>();
            reader.CanRead = filter;
            reader.Get = selector;
            this._methodsReaders.Add(reader);
            return this;
        }

        /// <summary>
        /// Add a IPropertyReader class to reader values to a specific property
        /// </summary>
        /// <param name="reader">Instance of IPropertyReader</param>
        /// <returns></returns>
        public ReflectionTree AddValueReaderForProperties(IPropertyReader reader)
        {
            var reader2 = new DefinitionOfMethodValueReader<PropertyInfo>();
            reader2.CanRead = reader.CanRead;
            reader2.GetValues = reader.GetValues;
            this._propertyValueReaders.Add(reader2);
            return this;
        }

        /// <summary>
        /// Add a custom filter and reader to get values to a specific properties
        /// </summary>
        /// <param name="filter">Specify the filter to apply selector</param>
        /// <param name="valuesGetter">Selector of the values</param>
        /// <returns></returns>
        public ReflectionTree AddValueReaderForProperties(Func<ReflectedInstance, Type, PropertyInfo, bool> filter, Func<ReflectedInstance, Type, PropertyInfo, IEnumerable<MethodValue>> valuesGetter)
        {
            var reader2 = new DefinitionOfMethodValueReader<PropertyInfo>();
            reader2.CanRead = filter;
            reader2.GetValues = valuesGetter;
            this._propertyValueReaders.Add(reader2);
            return this;
        }

        /// <summary>
        /// Add a IMethodReader class to reader values to a specific property
        /// </summary>
        /// <param name="reader">Instance of IMethodReader</param>
        /// <returns></returns>
        public ReflectionTree AddValueReaderForMethods(IMethodReader reader)
        {
            var reader2 = new DefinitionOfMethodValueReader<MethodInfo>();
            reader2.CanRead = reader.CanRead;
            reader2.GetValues = reader.GetValues;
            this._methodValueReaders.Add(reader2);
            return this;
        }

        /// <summary>
        /// Add a custom filter and reader to get values to a specific methods
        /// </summary>
        /// <param name="filter">Specify the filter to apply selector</param>
        /// <param name="valuesGetter">Selector of the values</param>
        /// <returns></returns>
        public ReflectionTree AddValueReaderForMethods(Func<ReflectedInstance, Type, MethodInfo, bool> filter, Func<ReflectedInstance, Type, MethodInfo, IEnumerable<MethodValue>> valuesGetter)
        {
            var reader2 = new DefinitionOfMethodValueReader<MethodInfo>();
            reader2.CanRead = filter;
            reader2.GetValues = valuesGetter;
            this._methodValueReaders.Add(reader2);
            return this;
        }

        #region Default settings

        private void DefaultSettingsToTypes()
        {
            this.SelectTypes(
                (obj) =>
                {
                    var typesParents = new List<Type>();
                    var type = obj.GetType();
                    if (obj is Array)
                    {
                        typesParents.Add(typeof(System.Collections.IList));
                    }
                    else
                    { 
                        typesParents.Add(type);
                    }
                    return typesParents;
                });
        }

        private void DefaultSettingsToFields()
        {
            // Return none fields (null) from object that are specify
            // OBS: It's can be overrided ONLY adding other filter, that return "true", after this.
            this.SelectFields(
                (obj, type) =>
                {
                    var mainType = obj.GetType();

                    if
                    (
                           noneMemberForTypes.Contains(mainType)
                        || noneMemberForTypesNames.Exists(match => Regex.IsMatch(mainType.FullName, match))
                        || typeof(Delegate).IsAssignableFrom(mainType)
                    )
                        return true;

                    return false;
                }
                , null
            );
        }

        private void DefaultSettingsToProperties()
        {
            // Default bindingAttr for all 
            this.SelectProperties(BindingFlags.Public | BindingFlags.Instance);

            // Return none properties (null) from object that are specify
            // OBS: It's can be overrided ONLY adding other filter, that return "true", after this.
            this.SelectProperties(
                (obj, type) =>
                {
                    var mainType = obj.GetType();

                    if 
                    (   
                           noneMemberForTypes.Contains(mainType)
                        || noneMemberForTypesNames.Exists(match => Regex.IsMatch(mainType.FullName, match))
                        || typeof(Delegate).IsAssignableFrom(mainType)
                    )
                        return true;

                    return false;
                }
                , null
            );

            // Get only property "Item[int32 index]", "Item[string key]" for Dictionary, "Item[params int[] indices]" (representative only) for Arrays
            // OBS: It's can be overrided ONLY adding other filter, that return "true", after this.
            this.SelectProperties(
                (obj, type) =>
                {
                    return obj is System.Collections.ArrayList 
                        || obj is System.Collections.BitArray 
                        || obj is System.Collections.IDictionary
                        || obj is Array;
                }
                ,
                (obj, type) =>
                {
                    if 
                    (
                        type == typeof(System.Collections.ArrayList)
                     || type == typeof(System.Collections.BitArray)
                     || type == typeof(System.Collections.IDictionary)
                     || type == typeof(System.Collections.IList)
                    )
                        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(f => f.GetIndexParameters().Length > 0);

                    return null;
                }
            );

            // Set default valuesGetters
            // ** This order is obrigatory **
            this.AddValueReaderForProperties(new PropertyReaderDefault());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInt32InAnyClass());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInArray());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInDictionary());
        }

        private void DefaultSettingsToMethods()
        {
            // Get "GetEnumerator" method for classes that hasn't a indexer (this[...]) to get owner values
            this.SelectMethods(
                (obj, type) =>
                {   
                    return obj is System.Collections.IEnumerable
                        && obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Count(f => f.GetIndexParameters().Length == 0) == 0;
                }
                ,
                (obj, type) =>
                {
                    return type.GetMethods(BindingFlags.NonPublic |  BindingFlags.Public | BindingFlags.Instance).Where(f => f.Name == "GetEnumerator" || f.Name == "System.Collections.IEnumerable.GetEnumerator");
                }
            );

            this.AddValueReaderForMethods(new MethodReaderIEnumerableGetEnumerator());
        }

        #endregion

        #endregion

        #region Privates
        
        private DefinitionOfClassMemberReader<FieldInfo> GetFieldsReader(object obj, Type type)
        {
            var selector = this._fieldsReaders.LastOrDefault(f => f.CanRead != null && f.CanRead(obj, type));
            if (selector != null && selector.Get != null)
                return selector;

            return null;
        }

        private DefinitionOfClassMemberReader<PropertyInfo> GetPropertiesReader(object obj, Type type)
        {
            var selector = this._propertysReaders.LastOrDefault(f => f.CanRead != null && f.CanRead(obj, type));
            if (selector != null && selector.Get != null)
                return selector;

            return null;
        }

        private DefinitionOfClassMemberReader<MethodInfo> GetMethodsReader(object obj, Type type)
        {
            var selector = this._methodsReaders.LastOrDefault(f => f.CanRead != null && f.CanRead(obj, type));
            if (selector != null && selector.Get != null)
                return selector;

            return null;
        }

        #endregion
    }
}
