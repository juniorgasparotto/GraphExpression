//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace ExpressionGraph.Reflection
//{
//    public class Instance
//    {
//        public string Name { get; private set; }
//        public object Object { get; private set; }
//        public Type ObjectType { get; private set; }
//        public string Source { get; private set; }
//        public List<InstanceType> InstanceTypes { get; private set; }
//        public List<IPropertyReader> PropertyReaders { get; private set; }
//        public List<IMethodReader> MethodReaders { get; private set; }

//        public Instance(object obj, string name = null, string source = null)
//        {
//            if (obj == null)
//                throw new ArgumentNullException("obj");

//            this.ObjectType = obj.GetType();
//            this.Name = name ?? this.ObjectType.Name + "_" + obj.GetHashCode();
//            this.Source = source;
//            this.Object = obj;
//            this.InstanceTypes = new List<InstanceType>();

//            this.PropertyReaders = new List<IPropertyReader>();
//            this.PropertyReaders.Add(new PropertyReadDefault());
//            this.PropertyReaders.Add(new PropertyReadIndexerItemInt32());

//            this.MethodReaders = new List<IMethodReader>();
//            this.MethodReaders.Add(new MethodReaderDefault());
//        }

//        public void Reflect()
//        {
//            var typesParents = new List<Type>();
//            typesParents.Add(this.ObjectType);
//            typesParents.AddRange(ReflectionHelper.GetAllParentTypes(this.ObjectType, true).Distinct());
            
//            var propertiesAll = new List<Property>();
//            var methodsAll = new List<Method>();
//            var fieldsAll = new List<Field>();

//            foreach (var typeParent in typesParents)
//            {
//                var instanceType = new InstanceType(typeParent);
//                this.ParseProperties(instanceType, propertiesAll);
//                this.ParseMethods(instanceType, methodsAll);
//                this.ParseFields(instanceType, fieldsAll);

//                this.InstanceTypes.Add(instanceType);

//                propertiesAll.AddRange(instanceType.Properties);
//                methodsAll.AddRange(instanceType.Methods);
//                fieldsAll.AddRange(instanceType.Fields);
//            }
//        }

//        public IEnumerable<Property> GetProperties()
//        {
//            return InstanceTypes.SelectMany(f => f.Properties);
//        }

//        public IEnumerable<Method> GetMethods()
//        {
//            return InstanceTypes.SelectMany(f => f.Methods);
//        }

//        public IEnumerable<Field> GetFields()
//        {
//            return InstanceTypes.SelectMany(f => f.Fields);
//        }

//        #region Privates

//        private void ParseProperties(InstanceType instanceType, List<Property> propertiesAddeds)
//        {
//            var objType = instanceType.Type;
//            var propsPublics = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
//            var propsPrivates = objType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.Name.Contains(".")).ToList();
//            var propsImplicits = objType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.Name.Contains(".")).ToList();
//            var propsPublicsStatics = objType.GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
//            var propsPrivatesStatics = objType.GetProperties(BindingFlags.NonPublic | BindingFlags.Static).ToList();
//            var properties = new List<PropertyInfo>();
//            properties.AddRange(propsPublics);
//            properties.AddRange(propsPrivates);
//            properties.AddRange(propsImplicits);
//            properties.AddRange(propsPublicsStatics);
//            properties.AddRange(propsPrivatesStatics);

//            foreach (var property in properties)
//            {
//                List<MethodValue> values = null;

//                if (property.GetMethod != null)
//                { 
//                    var read = this.PropertyReaders.LastOrDefault(f => f.CanRead(this.Object, property));
//                    if (read != null)
//                        values = read.GetValues(this.Object, property).ToList();
//                    else 
//                        throw new Exception("No reader has been found for the Property '" + property.Name + "'");
//                }

//                var name = property.Name;

//                // Prevent duplicate indexer (Item) "this[int32 index]" or "this[int32 index, int32 index2]"
//                var parameters = property.GetIndexParameters();
//                if (parameters.Length > 0)
//                {
//                    var paramsStr = "";
//                    foreach (var param in parameters)
//                    {
//                        paramsStr += paramsStr == "" ? "" : ", ";
//                        paramsStr += string.Format("{0} {1}", param.ParameterType.Name, param.Name);
//                    }
//                    name += " [" + paramsStr + "]";
//                }

//                if (propertiesAddeds != null)
//                {
//                    var propDuplicated = propertiesAddeds.FirstOrDefault(f => f.Name == name);

//                    if (propDuplicated != null)
//                    {
//                        if (propDuplicated.ParentType != objType)
//                            name = objType.FullName + "." + name;
//                    }
//                }

//                var propertyAux = new Property(property.Name, name, objType, property, values);

//                // check if is override
//                var method = property.GetMethod != null ? property.GetMethod : property.SetMethod;

//                if (method.GetBaseDefinition() != method)
//                    propertyAux.IsOverride = true;

//                propertyAux.IsStatic = method.IsStatic;
//                propertyAux.IsVirtual = method.IsVirtual;
//                propertyAux.IsAbstract = method.IsAbstract;
//                propertyAux.IsExplicitlyImpl = propsImplicits.Contains(property);
//                propertyAux.IsNew = method.IsHideBySig;
//                propertyAux.IsSealed = method.IsFinal;

//                if (property.GetMethod != null)
//                {
//                    propertyAux.IsGetPublic = property.GetMethod.IsPublic;
//                    propertyAux.IsGetPrivate = property.GetMethod.IsPrivate;
//                    propertyAux.IsGetProtected = property.GetMethod.IsFamily;
//                    propertyAux.IsGetInternal = property.GetMethod.IsAssembly;
//                }

//                if (property.SetMethod != null)
//                {
//                    propertyAux.IsSetPublic = property.SetMethod.IsPublic;
//                    propertyAux.IsSetPrivate = property.SetMethod.IsPrivate;
//                    propertyAux.IsSetProtected = property.SetMethod.IsFamily;
//                    propertyAux.IsSetInternal = property.SetMethod.IsAssembly;
//                }

//                instanceType.Properties.Add(propertyAux);
//            }
//        }

//        private void ParseMethods(InstanceType instanceType, List<Method> methodsAddeds)
//        {
//            var objType = instanceType.Type;
//            var propsPublics = objType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsSpecialName);
//            var propsPrivates = objType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.IsSpecialName && !f.Name.Contains(".")).ToList();
//            var propsImplicits = objType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => !f.IsSpecialName && f.Name.Contains(".")).ToList();
//            var propsPublicsStatics = objType.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(f => !f.IsSpecialName);
//            var propsPrivatesStatics = objType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(f => !f.IsSpecialName);

//            var methods = new List<MethodInfo>();
//            methods.AddRange(propsPublics);
//            methods.AddRange(propsPrivates);
//            methods.AddRange(propsImplicits);
//            methods.AddRange(propsPublicsStatics);
//            methods.AddRange(propsPrivatesStatics);

//            foreach (var method in methods)
//            {
//                List<MethodValue> values = null;

//                if (method.ReturnType != typeof(void))
//                {
//                    var read = this.MethodReaders.LastOrDefault(f => f.CanRead(this.Object, method));
//                    if (read != null)
//                        values = read.GetValues(this.Object, method).ToList();
//                }

//                var name = method.Name;

//                // Prevent duplicate indexer (Item) "this[int32 index]" or "this[int32 index, int32 index2]"
//                var parameters = method.GetParameters();
//                if (parameters.Length > 0)
//                {
//                    var paramsStr = "";
//                    foreach (var param in parameters)
//                    {
//                        paramsStr += paramsStr == "" ? "" : ", ";
//                        paramsStr += string.Format("{0} {1}", param.ParameterType.Name, param.Name);
//                    }
//                    name += " [" + paramsStr + "]";
//                }

//                if (methodsAddeds != null)
//                {
//                    var propDuplicated = methodsAddeds.FirstOrDefault(f => f.Name == name);

//                    if (propDuplicated != null)
//                    {
//                        if (propDuplicated.ParentType != objType)
//                            name = objType.FullName + "." + name;
//                    }
//                }

//                var methodAux = new Method(name, objType, method, values);

//                if (method.GetBaseDefinition() != method)
//                    methodAux.IsOverride = true;

//                methodAux.IsPublic = method.IsPublic;
//                methodAux.IsPrivate = method.IsPrivate;
//                methodAux.IsProtected = method.IsFamily;
//                methodAux.IsInternal = method.IsAssembly;

//                methodAux.IsStatic = method.IsStatic;
//                methodAux.IsVirtual = method.IsVirtual;
//                methodAux.IsAbstract = method.IsAbstract;
//                methodAux.IsExplicitlyImpl = propsImplicits.Contains(method);
//                methodAux.IsNew = method.IsHideBySig;
//                methodAux.IsSealed = method.IsFinal;

//                instanceType.Methods.Add(methodAux);
//            }
//        }

//        private void ParseFields(InstanceType instanceType, List<Field> fieldsAddeds)
//        {
//            var objType = instanceType.Type;
//            var fieldsPublics = objType.GetFields(BindingFlags.Public | BindingFlags.Instance);
//            var fieldsPrivates = objType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
//            var fieldsPublicsStatics = objType.GetFields(BindingFlags.Public | BindingFlags.Static);
//            var fieldsPrivatesStatics = objType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);

//            var fields = new List<FieldInfo>();
//            fields.AddRange(fieldsPublics);
//            fields.AddRange(fieldsPrivates);
//            fields.AddRange(fieldsPublicsStatics);
//            fields.AddRange(fieldsPrivatesStatics);

//            foreach (var field in fields)
//            {
//                var name = field.Name;
//                var value = field.GetValue(this.Object);

//                if (fieldsAddeds != null)
//                {
//                    var propDuplicated = fieldsAddeds.FirstOrDefault(f => f.Name == name);

//                    if (propDuplicated != null)
//                    {
//                        if (propDuplicated.ParentType != objType)
//                            name = objType.FullName + "." + name;
//                    }
//                }

//                var fieldAux = new Field(name, objType, field, value);

//                fieldAux.IsStatic = field.IsStatic;
//                fieldAux.IsPublic = field.IsPublic;
//                fieldAux.IsPrivate = field.IsPrivate;
//                fieldAux.IsProtected = field.IsFamily;
//                fieldAux.IsInternal = field.IsAssembly;
//                fieldAux.IsReadOnly = field.IsInitOnly;
//                fieldAux.IsConstant = field.IsLiteral && !field.IsInitOnly;
//                instanceType.Fields.Add(fieldAux);
//            }
//        }

//        #endregion

//        #region Overrides

//        public override string ToString()
//        {
//            return Name;
//        }

//        #endregion

//        public static bool operator ==(Instance a, Instance b)
//        {
//            return Equals(a, b);
//        }

//        public static bool operator !=(Instance a, Instance b)
//        {
//            return !Equals(a, b);
//        }

//        public override bool Equals(object obj)
//        {
//            if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
//                return false;

//            var converted = obj as Instance;
//            return (this.Object.Equals(converted.Object));
//        }

//        public override int GetHashCode()
//        {
//            return 0;
//        }

//        //public Dictionary<string, object> GetValues()
//        //{
//        //    var dictionary = new Dictionary<string, object>(); 
//        //    var fields = this.GetFields();

//        //    foreach (var field in fields)
//        //        dictionary.Add(field.Name, field.Value);
//        //    var properties = this.GetProperties();
//        //    var methods = this.GetMethods();
//        //}
//    }
//}