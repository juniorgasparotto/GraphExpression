using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphQuery.Reflection
{
    internal class ReflectionUnit
    {
        public Func<object, IEnumerable<Type>> TypesReader { get; set; }
        public Func<object, Type, IEnumerable<FieldInfo>> FieldsReader { get; set; }
        public Func<object, Type, IEnumerable<PropertyInfo>> PropertiesReader { get; set; }
        public Func<object, Type, IEnumerable<MethodInfo>> MethodsReader { get; set; }
        public Func<object, Type, IEnumerable<MethodValue>> EnumeratorReader { get; set; }
        public List<DefinitionOfMethodValueReader<PropertyInfo>> PropertyValueReaders { get; set; }
        public List<DefinitionOfMethodValueReader<MethodInfo>> MethodValueReaders { get; set; }

        public ReflectionUnit()
        {
           
        }

        /// <summary>
        /// Do reflection
        /// </summary>
        /// <returns></returns>
        public ReflectedInstance GetInstance(object obj, string containerName)
        {
            var instance = new ReflectedInstance(obj, null, containerName);

            if (obj != null)
            {
                IEnumerable<Type> typesParents = null;

                if (this.TypesReader != null)
                    typesParents = this.TypesReader(obj);

                if (typesParents != null)
                {
                    typesParents = typesParents.Distinct().ToList();
                    var propertiesAll = new List<Property>();
                    var methodsAll = new List<Method>();
                    var fieldsAll = new List<Field>();

                    foreach (var typeParent in typesParents)
                    {
                        var instanceType = new ReflectedType(typeParent);

                        if (this.FieldsReader != null)
                            this.ParseFields(instance, this.FieldsReader(obj, typeParent), instanceType, fieldsAll);

                        if (this.PropertiesReader != null)
                            this.ParseProperties(instance, this.PropertiesReader(obj, typeParent), instanceType, propertiesAll);

                        if (this.MethodsReader != null)
                            this.ParseMethods(instance, this.MethodsReader(obj, typeParent), instanceType, methodsAll);

                        if (this.EnumeratorReader != null)
                            this.ParseEnumerator(instance, instanceType);

                        instance.Add(instanceType);

                        propertiesAll.AddRange(instanceType.Properties);
                        methodsAll.AddRange(instanceType.Methods);
                        fieldsAll.AddRange(instanceType.Fields);
                    }
                }
            }

            return instance;
        }

        private void ParseProperties(ReflectedInstance instance, IEnumerable<PropertyInfo> properties, ReflectedType instanceType, List<Property> propertiesAddeds)
        {
            if (properties == null) return;

            foreach (var property in properties)
            {
                List<MethodValue> values = null;

                if (property.GetMethod != null)
                {
                    var read = this.PropertyValueReaders.LastOrDefault(f => f.CanRead(instance, instanceType.Type, property));
                    if (read != null)
                        values = read.GetValues(instance, instanceType.Type, property).ToList();
                    //else 
                    //    throw new Exception("No reader has been found for the Property '" + property.Name + "'");
                }

                var name = property.Name;
                
                // Add fullName for property when it exists in other type with the same name.
                if (propertiesAddeds != null)
                {
                    var propDuplicated = propertiesAddeds.FirstOrDefault(f => f.PropertyInfo.Name == property.Name && f.ParentType != instanceType.Type);

                    if (propDuplicated != null)
                    {
                        var typeStr = ReflectionUtils.CSharpName(instanceType.Type);
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

                instanceType.Add(propertyConverted);
            }
        }

        private void ParseMethods(ReflectedInstance instance, IEnumerable<MethodInfo> methods, ReflectedType instanceType, List<Method> methodsAddeds)
        {
            if (methods == null) return;

            methods = methods.Where(m => !m.IsSpecialName);

            var objType = instanceType.Type;
            foreach (var method in methods)
            {
                List<MethodValue> values = null;

                if (method.ReturnType != typeof(void))
                {
                    var read = this.MethodValueReaders.LastOrDefault(f => f.CanRead(instance, instanceType.Type, method));
                    if (read != null)
                        values = read.GetValues(instance, instanceType.Type, method).ToList();
                }

                var name = method.Name;

                // Prevent duplicate indexer (Item) "this[int32 index]" or "this[int32 index, int32 index2]"
                //var parameters = method.GetParameters();
                //if (parameters.Length > 0)
                //{
                //    var paramsStr = "";
                //    foreach (var param in parameters)
                //    {
                //        paramsStr += paramsStr == "" ? "" : ", ";
                //        paramsStr += string.Format("{0} {1}", param.ParameterType.Name, param.Name);
                //    }
                //    name += "[" + paramsStr + "]";
                //}

                if (methodsAddeds != null)
                {
                    var methodDuplicated = methodsAddeds.FirstOrDefault(f => f.Name == name);

                    if (methodDuplicated != null)
                    {
                        if (methodDuplicated.ParentType != objType)
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

                instanceType.Add(methodAux);
            }
        }

        private void ParseEnumerator(ReflectedInstance instance, ReflectedType instanceType)
        {
            var values = this.EnumeratorReader(instance.Object, instanceType.Type);
            if (values != null)
                instanceType.Add(values);
        }

        private void ParseFields(ReflectedInstance instance, IEnumerable<FieldInfo> fields, ReflectedType instanceType, List<Field> fieldsAddeds)
        {
            if (fields == null) return;

            var objType = instanceType.Type;
            foreach (var field in fields)
            {
                var name = field.Name;
                var value = field.GetValue(instance.Object);

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
                instanceType.Add(fieldAux);
            }
        }
    }
}