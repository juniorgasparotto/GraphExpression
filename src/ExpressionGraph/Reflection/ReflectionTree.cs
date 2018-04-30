using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpressionGraph.Reflection
{
    public class ReflectionTree
    {
        //private List<Type> noneMemberForMainTypes = new List<Type>()
        //{
        //    typeof(Boolean),
        //    typeof(Char),
        //    typeof(SByte),
        //    typeof(Byte),
        //    typeof(Int16),
        //    typeof(UInt16),
        //    typeof(Int32),
        //    typeof(UInt32),
        //    typeof(Int64),
        //    typeof(UInt64),
        //    typeof(Single),
        //    typeof(Double),
        //    typeof(Decimal),
        //    typeof(DateTime),
        //    typeof(String),
        //    typeof(System.Text.StringBuilder),
        //    typeof(TimeSpan),
        //    typeof(DateTimeOffset),
        //    typeof(IntPtr),
        //    typeof(UIntPtr),
        //    typeof(TimeZone),
        //    typeof(Uri),
        //    typeof(Guid),
        //};

        //private List<Type> noneMemberForSubTypes = new List<Type>()
        //{
        //    typeof(Delegate),
        //    typeof(Array),
        //    typeof(System.Collections.BitArray),
        //    typeof(System.Collections.ArrayList),
        //    typeof(System.Collections.IDictionary),
        //    typeof(System.Collections.IList),            
        //};

        //private List<string> noneMemberForMainTypesFullNames = new List<string>()
        //{
        //    "^System.RuntimeType$",
        //};

        private Expression<ReflectedInstance> _expressions;
        private SettingsFlags _settingsAttributes;
        private object _object;

        // Readers for types and member
        private List<DefinitionOfTypeReader> _typesReaders;
        private List<DefinitionOfClassMemberReader<FieldInfo>> _fieldsReaders;
        private List<DefinitionOfClassMemberReader<PropertyInfo>> _propertysReaders;
        private List<DefinitionOfClassMemberReader<MethodInfo>> _methodsReaders;
        private List<DefinitionOfClassMemberReader<MethodValue>> _enumerableValuesReaders;

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

            // Readers for get IEnumerable values of intances
            this._enumerableValuesReaders = new List<DefinitionOfClassMemberReader<MethodValue>>();

            // Readers for get values
            this._propertyValueReaders = new List<DefinitionOfMethodValueReader<PropertyInfo>>();
            this._methodValueReaders = new List<DefinitionOfMethodValueReader<MethodInfo>>();

            this.DefaultSettingsToTypes();
            this.DefaultSettingsToFields();
            this.DefaultSettingsToProperties();
            this.DefaultSettingsToMethods();
            this.DefaultSettingsToIEnumerable();
        }

        public Expression<ReflectedInstance> AsExpression()
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

        #region Test

        public ReflectionTree SelectParentsType<T>(Func<object, IEnumerable<Type>> selector)
        {
            this.SelectTypes((type) => type == typeof(T), selector);
            return this;
        }

        public ReflectionTree SelectMethods<T>(Func<object, Type, IEnumerable<MethodInfo>> selector)
        {
            this.SelectMethods((value, parentType) => parentType == typeof(T), selector);
            return this;
        }

        #endregion


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
                if (method.Values != null && method.Values.Count > 0)
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
                else
                {
                    list.Add(GetInstance(null, method.Name));
                }
            }

            var enumeratorValues = instance.GetAllEnumeratorValues().ToList();
            foreach (var value in enumeratorValues)
            {
                if (value != null)
                {
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

                        list.Add(GetInstance(value.Value, parameterStr));
                    }
                }
            }

            return list;
        }

        private string ObjectToString(object obj, Type type, string containerName = null, long id = -1, bool hasChildren = false)
        {
            string output;

            if (obj == null)
            {
                output = "null";
            }
            else if (hasChildren)
            {
                output = TypeToString(type, id);
            }
            else
            {
                output = Utils.ToLiteral(obj);

                // if type can't be converted to literal, then print the type
                if (output == null)
                    output = TypeToString(type, id);
            }

            if (!string.IsNullOrWhiteSpace(containerName))
                output = containerName + ": " + output;

            return output;
        }

        private string TypeToString(Type type, long id = -1)
        {
            return "{" + ReflectionUtils.CSharpName(type, true) + "_" + id + "}";
        }

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

            reflectionUnit.EnumeratorReader = 
                (objParam, type) =>
                {
                    IEnumerable<MethodValue> values = null;
                    var selector = this.GetEnumeratorValuesReader(objParam, type);
                    if (selector != null)
                        values = selector.Get(objParam, type);

                    return values;
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
        public ReflectionTree SelectFields(BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
        {
            this._fieldsReaders[0].Get = (value, type) =>
            {
                // ignore properties fields
                // ex: <Prop1>._bracketfield
                var fields = type.GetFields(bindingAttr).Where(f => !f.Name.StartsWith("<"));
                return fields;
            };
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


        /// <summary>
        /// Add a IEnumeratorReader class to reader values to a specific instance of IEnumerable
        /// </summary>
        /// <param name="reader">Instance of IPropertyReader</param>
        /// <returns></returns>
        public ReflectionTree AddIEnumerableReader(IEnumerableReader reader)
        {
            var reader2 = new DefinitionOfClassMemberReader<MethodValue>();
            reader2.CanRead = reader.CanRead;
            reader2.Get = reader.GetValues;
            this._enumerableValuesReaders.Add(reader2);
            return this;
        }

        /// <summary>
        /// Add a custom IEnumeratorReader to reader values to a specific instance of IEnumerable
        /// </summary>
        /// <param name="filter">Specify the filter to apply selector</param>
        /// <param name="valuesGetter">Selector of the values</param>
        /// <returns></returns>
        public ReflectionTree AddIEnumerableReader(Func<object, Type, bool> filter, Func<object, Type, IEnumerable<MethodValue>> valuesGetter)
        {
            var reader2 = new DefinitionOfClassMemberReader<MethodValue>();
            reader2.CanRead = filter;
            reader2.Get = valuesGetter;
            this._enumerableValuesReaders.Add(reader2);
            return this;
        }

        #region Default settings

        private void DefaultSettingsToTypes()
        {
            this.SelectTypes(
                (obj) =>
                {
                    return new List<Type>() { obj.GetType() };
                }
            );
        }

        private void DefaultSettingsToFields()
        {
            /** 
            * Obrigatory to be last reader **
            * Return none fields (null) from object that are specify
            **/
            this.SelectFields((obj, type) => IsSystemType(type), null);
        }

        private void DefaultSettingsToProperties()
        {
            // For all instances, by default, get all public properties of a instance using bindingAttr
            this.SelectProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);

            /** 
            * Obrigatory to be last reader **
            * Return none methods (null) from object that is disabled to deeper
            **/
            this.SelectProperties((obj, type) => IsSystemType(type), null);

            // Add default value reader to single properties
            this.AddValueReaderForProperties(new PropertyReaderDefault());
        }

        private void DefaultSettingsToMethods()
        {
            /** 
            * Obrigatory to be last reader **
            * Return none methods (null) from object that is disabled to deeper
            **/
            this.SelectMethods((obj, type) => IsSystemType(type), null);
        }

        private void DefaultSettingsToIEnumerable()
        {
            // Add default values reader for all instances of IEnumerable
            this.AddIEnumerableReader(new EnumerableReaderDefault());

            // Add default values reader for all Array (overriding IEnumerableReader)
            this.AddIEnumerableReader(new EnumerableReaderArray());

            // Add default values reader for all Dictionary (overriding IEnumerableReader)
            this.AddIEnumerableReader(new EnumerableReaderIDictionary());

            /** 
            * Obrigatory to be last reader **
            * Return none ienumerable values (null) from object that is disabled to deeper 
            **/
            this.AddIEnumerableReader((obj, type) => obj is string, null);
        }

        #endregion

        #endregion

        #region Privates

        private bool IsSystemType(Type type)
        {
            var typeName = type.Namespace ?? "";
            if (typeName == "System" || typeName.StartsWith("System.") ||
                typeName == "Microsoft" || typeName.StartsWith("Microsoft."))
                return true;
            return false;

            //if
            //(
            //       noneMemberForMainTypes.Contains(mainType)
            //    || noneMemberForSubTypes.Exists(type => type.IsAssignableFrom(mainType))
            //    || noneMemberForMainTypesFullNames.Exists(match => Regex.IsMatch(mainType.FullName, match))
            //)
            //    return true;

            //return false;
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

        private DefinitionOfClassMemberReader<MethodInfo> GetMethodsReader(object obj, Type type)
        {
            var selector = this._methodsReaders.LastOrDefault(f => f.CanRead != null && f.CanRead(obj, type));
            if (selector != null && selector.Get != null)
                return selector;

            return null;
        }

        private DefinitionOfClassMemberReader<MethodValue> GetEnumeratorValuesReader(object obj, Type type)
        {
            var selector = this._enumerableValuesReaders.LastOrDefault(f => f.CanRead != null && f.CanRead(obj, type));
            if (selector != null && selector.Get != null)
                return selector;

            return null;
        }

        #endregion
    }
}
