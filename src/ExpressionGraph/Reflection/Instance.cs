using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class Instance
    {
        public string Name { get; private set; }
        public object Object { get; private set; }
        public Type ObjectType { get; private set; }
        public string ContainerName { get; private set; }
        public List<InstanceType> InstanceTypes { get; private set; }
        public List<IPropertyReader> PropertyReaders { get; private set; }
        public List<IMethodReader> MethodReaders { get; private set; }

        public Func<object, Type, List<Type>> FilterTypes { get; set; }
        public Func<object, Type, List<FieldInfo>> FilterFields { get; set; }
        public Func<object, Type, List<PropertyInfo>> FilterProperties { get; set; }
        public Func<object, Type, List<MethodInfo>> FilterMethods { get; set; }

        public Instance(object obj, string name = null, string containerName = null)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            this.ObjectType = obj.GetType();
            this.Name = name ?? this.ObjectType.Name + "_" + obj.GetHashCode();
            this.ContainerName = containerName;
            this.Object = obj;
            this.InstanceTypes = new List<InstanceType>();

            this.PropertyReaders = new List<IPropertyReader>();
            this.PropertyReaders.Add(new PropertyReaderDefault());
            this.PropertyReaders.Add(new PropertyReaderIndexerInt32InAnyClass());
            this.PropertyReaders.Add(new PropertyReaderIndexerInArray());
            this.PropertyReaders.Add(new PropertyReaderIndexerInDictionary());

            this.MethodReaders = new List<IMethodReader>();
            //this.MethodReaders.Add(new MethodReaderDefault());

            //this.ClassReaders = new List<IClassReader>();
            //this.ClassReaders.Add(new ClassReaderDefault());
            //this.ClassReader = new ClassReaderDefault();

            this.FilterTypes = this.GetTypes;
            this.FilterFields = this.GetFields;
            this.FilterProperties = this.GetProperties;
            this.FilterMethods = this.GetMethods;
        }

        public void Reflect()
        {
            if (this.FilterTypes == null)
                throw new NullReferenceException("The property 'FilterInstanceTypes' can't be null");

            //var classReader = this.ClassReaders.Last(f => f.CanRead(this.Object, this.ObjectType));
            //var classReader = this.ClassReader;
            var typesParents = this.FilterTypes(this.Object, this.ObjectType).Distinct().ToList();
            
            var propertiesAll = new List<Property>();
            var methodsAll = new List<Method>();
            var fieldsAll = new List<Field>();

            foreach (var typeParent in typesParents)
            {
                var instanceType = new InstanceType(typeParent);

                if (this.FilterProperties != null)
                    this.ParseProperties(this.FilterProperties(this.Object, typeParent), instanceType, propertiesAll);

                if (this.FilterMethods != null)
                    this.ParseMethods(this.FilterMethods(this.Object, typeParent), instanceType, methodsAll);

                if (this.FilterFields != null)
                    this.ParseFields(this.FilterFields(this.Object, typeParent), instanceType, fieldsAll);

                this.InstanceTypes.Add(instanceType);

                propertiesAll.AddRange(instanceType.Properties);
                methodsAll.AddRange(instanceType.Methods);
                fieldsAll.AddRange(instanceType.Fields);
            }
        }

        public IEnumerable<Property> GetProperties()
        {
            return InstanceTypes.SelectMany(f => f.Properties);
        }

        public IEnumerable<Method> GetMethods()
        {
            return InstanceTypes.SelectMany(f => f.Methods);
        }

        public IEnumerable<Field> GetFields()
        {
            return InstanceTypes.SelectMany(f => f.Fields);
        }

        #region Privates

        private void ParseProperties(List<PropertyInfo> properties, InstanceType instanceType, List<Property> propertiesAddeds)
        {
            if (properties == null) return;

            foreach (var property in properties)
            {
                List<MethodValue> values = null;

                if (property.GetMethod != null)
                { 
                    var read = this.PropertyReaders.LastOrDefault(f => f.CanRead(this.Object, property));
                    if (read != null)
                        values = read.GetValues(this.Object, property).ToList();
                    else 
                        throw new Exception("No reader has been found for the Property '" + property.Name + "'");
                }

                //var nameUnique = property.Name;
                //var originalName = property.Name;
                //var paramsStr = "";

                //// Prevent duplicate indexer (Item) "this[int32 index]" or "this[int32 index, int32 index2]"
                //var parameters = property.GetIndexParameters();
                //if (parameters.Length > 0)
                //{
                //    foreach (var param in parameters)
                //    {
                //        paramsStr += paramsStr == "" ? "" : ", ";
                //        paramsStr += string.Format("{0} {1}", ReflectionHelper.CSharpName(param.ParameterType), param.Name);
                //    }

                //    nameUnique += " [" + paramsStr + "]";
                //}

                //if (propertiesAddeds != null)
                //{
                //    var propDuplicated = propertiesAddeds.FirstOrDefault(f => f.NameUnique == nameUnique);

                //    if (propDuplicated != null)
                //    {
                //        if (propDuplicated.ParentType != objType)
                //            nameUnique = ReflectionHelper.CSharpName(objType) + "." + nameUnique;
                //    }
                //}

                //var propertyConverted = new Property(originalName, nameUnique, objType, property, values);

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

        private void ParseMethods(List<MethodInfo> methods, InstanceType instanceType, List<Method> methodsAddeds)
        {
            if (methods == null) return;

            var objType = instanceType.Type;
            foreach (var method in methods)
            {
                List<MethodValue> values = null;

                if (method.ReturnType != typeof(void))
                {
                    var read = this.MethodReaders.LastOrDefault(f => f.CanRead(this.Object, method));
                    if (read != null)
                        values = read.GetValues(this.Object, method).ToList();
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

        private void ParseFields(List<FieldInfo> fields, InstanceType instanceType, List<Field> fieldsAddeds)
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
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
                return false;

            var converted = obj as Instance;
            return (this.Object.Equals(converted.Object));
        }

        public override int GetHashCode()
        {
            return 0;
        }

        #endregion

        #region Operators

        public static bool operator ==(Instance a, Instance b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Instance a, Instance b)
        {
            return !Equals(a, b);
        }

        #endregion

        #region Methods like delegates

        private List<Type> GetTypes(object obj, Type type)
        {
            var typesParents = new List<Type>();
            typesParents.Add(type);
            typesParents.AddRange(ReflectionHelper.GetAllParentTypes(type, true).Distinct());
            return typesParents;
        }

        private List<PropertyInfo> GetProperties(object obj, Type type)
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

        private List<MethodInfo> GetMethods(object obj, Type type)
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

        private List<FieldInfo> GetFields(object obj, Type type)
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
    }
}