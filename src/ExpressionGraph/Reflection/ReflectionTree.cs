using ExpressionGraph.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpressionGraph.Reflection
{
    public class ReflectionTree
    {
        private Expression<InstanceReflected> _expressions;
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

        public ReflectionTree(object obj)
        {
            this._object = obj;

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

        public InstanceReflected Reflect()
        {
            return this.GetInstance(this._object, "");
        }

        public IEnumerable<InstanceReflected> ReflectTree()
        {
            return this.Query().ToEntities();
        }

        public Expression<InstanceReflected> Query()
        {
            var instanceRoot = new List<InstanceReflected>() { GetInstance(this._object, "") };
            this._expressions = ExpressionBuilder<InstanceReflected>
            .Build
            (
                instanceRoot,
                f => GetChildren(f),
                true,
                false,
                false,
                f => ObjectToString(f.Object, f.ObjectType, f.ContainerName)
            )
            .FirstOrDefault();

            return _expressions;
        }

        private List<InstanceReflected> GetChildren(InstanceReflected instance)
        {
            var list = new List<InstanceReflected>();

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
                            foreach (var param in value.Parameters)
                            {
                                parameterStr += parameterStr == "" ? "" : ", ";
                                parameterStr += ObjectToString(param.Value, param.Value.GetType(), showParameterName ? param.Name : null);
                            }

                            parameterStr = "[" + parameterStr + "]";
                        }

                        list.Add(GetInstance(value.Value, property.Name + parameterStr));
                    }
                }
            }

            return list;
        }

        private string ObjectToString(object obj, Type type, string containerName = null)
        {
            var objString = "";
            var showFullName = _settingsAttributes.HasFlag(SettingsFlags.ShowFullNameOfType);

            if (CanGetAnyMembers(obj, type))
                objString = ReflectionUtils.CSharpName(type, showFullName) + "_" + obj.GetHashCode();
            else
                objString = obj.ToString();
            
            if (string.IsNullOrWhiteSpace(containerName))
                return objString;

            return containerName + ":" + objString;
        }

        private InstanceReflected GetInstance(object obj, string containerName)
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
                    List<MethodInfo> methods = null;
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
            this._propertysReaders[0].Get = (value, type) => type.GetProperties(bindingAttr);
            return this;
        }

        /// <summary>
        /// Set the custom selector to get properties
        /// </summary>
        /// <param name="selector">Selector of the properties</param>
        /// <returns></returns>
        public ReflectionTree SelectProperties(Func<object, Type, IEnumerable<FieldInfo>> selector)
        {
            this._fieldsReaders[0].Get = selector;
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
        public ReflectionTree AddValueReaderForProperties(Func<InstanceReflected, Type, PropertyInfo, bool> filter, Func<object, Type, PropertyInfo, IEnumerable<MethodValue>> valuesGetter)
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
        public ReflectionTree AddValueReaderForMethods(Func<InstanceReflected, Type, MethodInfo, bool> filter, Func<object, Type, MethodInfo, IEnumerable<MethodValue>> valuesGetter)
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
                    typesParents.Add(type);
                    //typesParents.AddRange(ReflectionHelper.GetAllParentTypes(type, true).Distinct());
                    return typesParents;
                });
        }

        private void DefaultSettingsToFields()
        {
            // Disable object from .net system
            // OBS: It's can be overrided ONLY adding other filter, that return "true", after this.
            this.SelectFields(
                (obj, type) =>
                {
                    var valueType = obj.GetType();
                    return valueType.Namespace.Equals("System") || type.Namespace.StartsWith("System.");
                }
                , null
            );

            // By default, disable reading fields to "Array" or "Dictionary". 
            // OBS: It's can be overrided ONLY adding other filter, that return "true", after this.
            this.SelectFields((obj, type) => obj is Array || obj is System.Collections.IDictionary, null);
        }

        private void DefaultSettingsToProperties()
        {
            // Default bindingAttr for all 
            this.SelectProperties(BindingFlags.Public | BindingFlags.Instance);

            // Disable object from .net system
            // OBS: It's can be overrided ONLY adding other filter, that return "true", after this.
            this.SelectProperties(
                (obj, type) =>
                {
                    var valueType = obj.GetType();
                    return valueType.Namespace.Equals("System") || type.Namespace.StartsWith("System.");
                }
                , null
            );

            // Get only property "Item[params int[] indices]" for Arrays
            // OBS: It's can be overrided ONLY adding other filter, that return "true", after this.
            this.SelectProperties(
                (obj, type) =>
                {
                    return obj is Array;
                }
                ,
                (obj, type) =>
                {
                    if (type == typeof(System.Collections.IList))
                        return typeof(System.Collections.IList).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(f => f.GetIndexParameters().Length > 0);
                    return null;
                }
            );

            // Get only property "Item[string key]" for Dictionary
            // OBS: It's can be overrided ONLY adding other filter, that return "true", after this.
            this.SelectProperties(
                (obj, type) =>
                {
                    return obj is System.Collections.IDictionary;
                }
                ,
                (obj, type) =>
                {
                    if (type == typeof(System.Collections.IDictionary))
                        return typeof(System.Collections.IDictionary).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(f => f.GetIndexParameters().Length > 0);
                    return null;
                }
            );

            // set default valuesGetters
            this.AddValueReaderForProperties(new PropertyReaderDefault());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInt32InAnyClass());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInArray());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInDictionary());
        }

        private void DefaultSettingsToMethods()
        {
            
        }

        #endregion

        #endregion

        #region Privates

        private bool CanGetAnyMembers(object obj, Type type)
        {
            var selectorProps = this.GetPropertiesReader(obj, type);
            var selectorFields = this.GetPropertiesReader(obj, type);

            if (selectorProps == null && selectorFields == null)
                return false;

            return true;
        }

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

        #endregion
    }
}
