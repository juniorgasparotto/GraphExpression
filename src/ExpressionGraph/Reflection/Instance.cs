using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class UnitReflaction
    {
        private Func<object, IEnumerable<Type>> _typesReader;
        private Func<object, Type, IEnumerable<FieldInfo>> _fieldsReader;
        private Func<object, Type, IEnumerable<PropertyInfo>> _propertiesReader;
        private Func<object, Type, IEnumerable<MethodInfo>> _methodsReader;
        private List<DefinitionOfMethodValueReader<PropertyInfo>> _propertyValueReaders;
        private List<DefinitionOfMethodValueReader<MethodInfo>> _methodValueReaders;

        public string Name { get; private set; }
        public object Object { get; private set; }
        public Type ObjectType { get; private set; }
        public string ContainerName { get; private set; }
        public List<InstanceType> InstanceTypes { get; private set; }

        public UnitReflaction(object obj, string name = null, string containerName = null)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            this.ObjectType = obj.GetType();
            this.Name = name ?? this.ObjectType.Name + "_" + obj.GetHashCode();
            this.ContainerName = containerName;
            this.Object = obj;
            this.InstanceTypes = new List<InstanceType>();

            // set defaults to reader members
            this._typesReader = this.GetAllTypesDefault;
            this._fieldsReader = this.GetAllFieldInfoDefault;
            this._propertiesReader = this.GetAllPropertiesDefault;
            this._methodsReader = this.GetAllMethodsDefault;

            // set defaults to reader values of methods. Empty for methods
            this._methodValueReaders = new List<DefinitionOfMethodValueReader<MethodInfo>>();

            // set defaults to reader values of properties.
            this._propertyValueReaders = new List<DefinitionOfMethodValueReader<PropertyInfo>>();
            this.AddValueReaderForProperties(new PropertyReaderDefault());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInt32InAnyClass());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInArray());
            this.AddValueReaderForProperties(new PropertyReaderIndexerInDictionary());
        }

        public IEnumerable<Property> GetAllProperties()
        {
            return this.InstanceTypes.SelectMany(f => f.Properties);
        }

        public IEnumerable<Method> GetAllMethods()
        {
            return this.InstanceTypes.SelectMany(f => f.Methods);
        }

        public IEnumerable<Field> GetAllFields()
        {
            return this.InstanceTypes.SelectMany(f => f.Fields);
        }

        #region Privates

        private void ParseProperties(IEnumerable<PropertyInfo> properties, InstanceType instanceType, List<Property> propertiesAddeds)
        {
            if (properties == null) return;

            foreach (var property in properties)
            {
                List<MethodValue> values = null;

                if (property.GetMethod != null)
                { 
                    var read = this._propertyValueReaders.LastOrDefault(f => f.CanRead(this, instanceType.Type, property));
                    if (read != null)
                        values = read.GetValues(this, instanceType.Type, property).ToList();
                    else 
                        throw new Exception("No reader has been found for the Property '" + property.Name + "'");
                }

                var name = property.Name;
                
                // Add fullName for property when it exists in other type with the same name.
                if (propertiesAddeds != null)
                {
                    var propDuplicated = propertiesAddeds.FirstOrDefault(f => f.PropertyInfo.Name == property.Name && f.ParentType != instanceType.Type);

                    if (propDuplicated != null)
                    {
                        var typeStr = ReflectionHelper.CSharpName(instanceType.Type);
                        name = typeStr + "." + name;
                    }
                }

                var propertyConverted = new Property(name, instanceType.Type, property, values);

                // check if is override
                var method = property.GetMethod != null ? property.GetMethod : property.SetMethod;

                if (method.GetBaseDefinition() != method)
                    propertyConverted.IsOverride = true;

                propertyConverted.IsStatic = method.IsStatic;
                propertyConverted.IsVirtual = method.IsVirtual;
                propertyConverted.IsAbstract = method.IsAbstract;
                propertyConverted.IsExplicitlyImpl = property.Name.Contains(".");
                propertyConverted.IsNew = method.IsHideBySig;
                propertyConverted.IsSealed = method.IsFinal;
                propertyConverted.IsIndexedProperty = property.GetIndexParameters().Length > 0;

                if (property.GetMethod != null)
                {
                    propertyConverted.IsGetPublic = property.GetMethod.IsPublic;
                    propertyConverted.IsGetPrivate = property.GetMethod.IsPrivate;
                    propertyConverted.IsGetProtected = property.GetMethod.IsFamily;
                    propertyConverted.IsGetInternal = property.GetMethod.IsAssembly;
                }

                if (property.SetMethod != null)
                {
                    propertyConverted.IsSetPublic = property.SetMethod.IsPublic;
                    propertyConverted.IsSetPrivate = property.SetMethod.IsPrivate;
                    propertyConverted.IsSetProtected = property.SetMethod.IsFamily;
                    propertyConverted.IsSetInternal = property.SetMethod.IsAssembly;
                }

                instanceType.Properties.Add(propertyConverted);
            }
        }

        private void ParseMethods(IEnumerable<MethodInfo> methods, InstanceType instanceType, List<Method> methodsAddeds)
        {
            if (methods == null) return;

            var objType = instanceType.Type;
            foreach (var method in methods)
            {
                List<MethodValue> values = null;

                if (method.ReturnType != typeof(void))
                {
                    var read = this._methodValueReaders.LastOrDefault(f => f.CanRead(this, instanceType.Type, method));
                    if (read != null)
                        values = read.GetValues(this, instanceType.Type, method).ToList();
                }

                var name = method.Name;

                // Prevent duplicate indexer (Item) "this[int32 index]" or "this[int32 index, int32 index2]"
                var parameters = method.GetParameters();
                if (parameters.Length > 0)
                {
                    var paramsStr = "";
                    foreach (var param in parameters)
                    {
                        paramsStr += paramsStr == "" ? "" : ", ";
                        paramsStr += string.Format("{0} {1}", param.ParameterType.Name, param.Name);
                    }
                    name += " [" + paramsStr + "]";
                }

                if (methodsAddeds != null)
                {
                    var propDuplicated = methodsAddeds.FirstOrDefault(f => f.Name == name);

                    if (propDuplicated != null)
                    {
                        if (propDuplicated.ParentType != objType)
                            name = objType.FullName + "." + name;
                    }
                }

                var methodAux = new Method(name, objType, method, values);

                if (method.GetBaseDefinition() != method)
                    methodAux.IsOverride = true;

                methodAux.IsPublic = method.IsPublic;
                methodAux.IsPrivate = method.IsPrivate;
                methodAux.IsProtected = method.IsFamily;
                methodAux.IsInternal = method.IsAssembly;

                methodAux.IsStatic = method.IsStatic;
                methodAux.IsVirtual = method.IsVirtual;
                methodAux.IsAbstract = method.IsAbstract;
                methodAux.IsExplicitlyImpl = method.Name.Contains(".");
                methodAux.IsNew = method.IsHideBySig;
                methodAux.IsSealed = method.IsFinal;

                instanceType.Methods.Add(methodAux);
            }
        }

        private void ParseFields(IEnumerable<FieldInfo> fields, InstanceType instanceType, List<Field> fieldsAddeds)
        {
            if (fields == null) return;

            var objType = instanceType.Type;
            foreach (var field in fields)
            {
                var name = field.Name;
                var value = field.GetValue(this.Object);

                if (fieldsAddeds != null)
                {
                    var propDuplicated = fieldsAddeds.FirstOrDefault(f => f.Name == name);

                    if (propDuplicated != null)
                    {
                        if (propDuplicated.ParentType != objType)
                            name = objType.FullName + "." + name;
                    }
                }

                var fieldAux = new Field(name, objType, field, value);

                fieldAux.IsStatic = field.IsStatic;
                fieldAux.IsPublic = field.IsPublic;
                fieldAux.IsPrivate = field.IsPrivate;
                fieldAux.IsProtected = field.IsFamily;
                fieldAux.IsInternal = field.IsAssembly;
                fieldAux.IsReadOnly = field.IsInitOnly;
                fieldAux.IsConstant = field.IsLiteral && !field.IsInitOnly;
                instanceType.Fields.Add(fieldAux);
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            if (this.Object != null)
                return this.Object.ToString();

            return null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
                return false;

            var converted = obj as UnitReflaction;
            return (this.Object.Equals(converted.Object));
        }

        public override int GetHashCode()
        {
            if (this.Object != null)
                return this.Object.GetHashCode();

            return 0;
        }

        #endregion

        #region Operators

        public static bool operator ==(UnitReflaction a, UnitReflaction b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(UnitReflaction a, UnitReflaction b)
        {
            return !Equals(a, b);
        }

        #endregion

        #region Fluent

        /// <summary>
        /// Do reflection
        /// </summary>
        /// <returns></returns>
        public UnitReflaction Reflect()
        {
            var typesParents = this._typesReader(this.Object).Distinct().ToList();

            var propertiesAll = new List<Property>();
            var methodsAll = new List<Method>();
            var fieldsAll = new List<Field>();

            foreach (var typeParent in typesParents)
            {
                var instanceType = new InstanceType(typeParent);

                if (this._propertiesReader != null)
                    this.ParseProperties(this._propertiesReader(this.Object, typeParent), instanceType, propertiesAll);

                if (this._methodsReader != null)
                    this.ParseMethods(this._methodsReader(this.Object, typeParent), instanceType, methodsAll);

                if (this._fieldsReader != null)
                    this.ParseFields(this._fieldsReader(this.Object, typeParent), instanceType, fieldsAll);

                this.InstanceTypes.Add(instanceType);

                propertiesAll.AddRange(instanceType.Properties);
                methodsAll.AddRange(instanceType.Methods);
                fieldsAll.AddRange(instanceType.Fields);
            }

            return this;
        }

        /// <summary>
        /// Add a IPropertyReader class to reader values to a specific property
        /// </summary>
        /// <param name="reader">Instance of IPropertyReader</param>
        /// <returns></returns>
        public UnitReflaction AddValueReaderForProperties(IPropertyReader reader)
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
        public UnitReflaction AddValueReaderForProperties(Func<UnitReflaction, Type, PropertyInfo, bool> filter, Func<UnitReflaction, Type, PropertyInfo, IEnumerable<MethodValue>> valuesGetter)
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
        public UnitReflaction AddValueReaderForMethods(IMethodReader reader)
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
        public UnitReflaction AddValueReaderForMethods(Func<UnitReflaction, Type, MethodInfo, bool> filter, Func<UnitReflaction, Type, MethodInfo, IEnumerable<MethodValue>> valuesGetter)
        {
            var reader2 = new DefinitionOfMethodValueReader<MethodInfo>();
            reader2.CanRead = filter;
            reader2.GetValues = valuesGetter;
            this._methodValueReaders.Add(reader2);
            return this;
        }

        /// <summary>
        /// Set the custom selector to get types of object
        /// </summary>
        /// <param name="selector">Selector of the types</param>
        /// <returns></returns>
        public UnitReflaction SelectTypes(Func<object, IEnumerable<Type>> selector)
        {
            this._typesReader = selector;
            return this;
        }

        /// <summary>
        /// Set the custom selector to get fields
        /// </summary>
        /// <param name="selector">Selector of the fields</param>
        /// <returns></returns>
        public UnitReflaction SelectFields(Func<object, Type, IEnumerable<FieldInfo>> selector)
        {
            this._fieldsReader = selector;
            return this;
        }

        /// <summary>
        /// Set bindingAttr to get fields
        /// </summary>
        /// <param name="bindingAttr">Specific binding to return fields</param>
        /// <returns></returns>
        public UnitReflaction SelectFields(BindingFlags bindingAttr)
        {
            this._fieldsReader = (instance, type) => type.GetFields(bindingAttr);
            return this;
        }

        /// <summary>
        /// Set the custom selector to get properties
        /// </summary>
        /// <param name="selector">Selector of the properties</param>
        /// <returns></returns>
        public UnitReflaction SelectProperties(Func<object, Type, IEnumerable<PropertyInfo>> selector)
        {
            this._propertiesReader = selector;
            return this;
        }

        /// <summary>
        /// Set bindingAttr to get properties
        /// </summary>
        /// <param name="bindingAttr">Specific binding to return properties</param>
        /// <returns></returns>
        public UnitReflaction SelectProperties(BindingFlags bindingAttr)
        {
            this._propertiesReader = (instance, type) => type.GetProperties(bindingAttr);
            return this;
        }

        /// <summary>
        /// Set the custom selector to get properties
        /// </summary>
        /// <param name="selector">Selector of the properties</param>
        /// <returns></returns>
        public UnitReflaction SelectMethods(Func<object, Type, IEnumerable<MethodInfo>> selector)
        {
            this._methodsReader = selector;
            return this;
        }

        /// <summary>
        /// Set bindingAttr to get properties
        /// </summary>
        /// <param name="bindingAttr">Specific binding to return properties</param>
        /// <returns></returns>
        public UnitReflaction SelectMethods(BindingFlags bindingAttr)
        {
            this._methodsReader = (instance, type) => type.GetMethods(bindingAttr).Where(f => !f.IsSpecialName);
            return this;
        }

        #region Default settings

        private List<Type> GetAllTypesDefault(object obj)
        {
            var typesParents = new List<Type>();
            var type = obj.GetType();
            typesParents.Add(type);
            typesParents.AddRange(ReflectionHelper.GetAllParentTypes(type, true).Distinct());
            return typesParents;
        }

        private List<PropertyInfo> GetAllPropertiesDefault(object obj, Type type)
        {
            var propsPublics = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            var propsPrivates = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.Name.Contains(".")).ToList();
            var propsImplicits = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.Name.Contains(".")).ToList();
            var propsPublicsStatics = type.GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
            var propsPrivatesStatics = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static).ToList();

            var properties = new List<PropertyInfo>();
            properties.AddRange(propsPublics);
            properties.AddRange(propsPrivates);
            properties.AddRange(propsImplicits);
            properties.AddRange(propsPublicsStatics);
            properties.AddRange(propsPrivatesStatics);

            return properties;
        }

        private List<MethodInfo> GetAllMethodsDefault(object obj, Type type)
        {
            var propsPublics = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsSpecialName);
            var propsPrivates = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.IsSpecialName && !f.Name.Contains(".")).ToList();
            var propsImplicits = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.IsSpecialName && f.Name.Contains(".")).ToList();
            var propsPublicsStatics = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(f => !f.IsSpecialName);
            var propsPrivatesStatics = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(f => !f.IsSpecialName);

            var methods = new List<MethodInfo>();
            methods.AddRange(propsPublics);
            methods.AddRange(propsPrivates);
            methods.AddRange(propsImplicits);
            methods.AddRange(propsPublicsStatics);
            methods.AddRange(propsPrivatesStatics);

            return methods;
        }

        private List<FieldInfo> GetAllFieldInfoDefault(object obj, Type type)
        {
            var fieldsPublics = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var fieldsPrivates = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var fieldsPublicsStatics = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            var fieldsPrivatesStatics = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static);

            var fields = new List<FieldInfo>();
            fields.AddRange(fieldsPublics);
            fields.AddRange(fieldsPrivates);
            fields.AddRange(fieldsPublicsStatics);
            fields.AddRange(fieldsPrivatesStatics);

            return fields;
        }

        #endregion

        #endregion
    }
}