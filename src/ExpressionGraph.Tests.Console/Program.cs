using ExpressionGraph.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var testInt = GetEntityGraph(4);
            var testIntStr = testInt.ToString();

            var testStr = GetEntityGraph("ABC");
            var testStrStr = testStr.ToString();

            var testBool = GetEntityGraph(true);
            var testBoolStr = testBool.ToString();

            var testDecimal = GetEntityGraph(10.999m);
            var testDecimalStr = testDecimal.ToString();

            // 1 dimension of chars
            var testArrayChar = GetEntityGraph(new char[] { 'A', 'B', 'C' });
            var testArrayCharStr = testArrayChar.ToString();

            // 2 dimentions array
            var chars2D = new char[1, 2];
            chars2D[0, 0] = 'A';
            chars2D[0, 1] = 'B';
            var testArray2DChar = GetEntityGraph(chars2D);
            var testArray2DCharStr = testArray2DChar.ToString();

            // 3 dimensions of chars
            var chars3D = new char[2, 3, 2];
            chars3D[0, 0, 0] = 'A';
            chars3D[0, 0, 1] = 'B';
            chars3D[0, 1, 0] = 'C';
            chars3D[0, 1, 1] = 'D';
            chars3D[0, 2, 0] = 'E';
            chars3D[0, 2, 1] = 'F';
            chars3D[1, 0, 0] = 'G';
            chars3D[1, 0, 1] = 'H';
            chars3D[1, 1, 0] = 'I';
            chars3D[1, 1, 1] = 'J';
            chars3D[1, 2, 0] = 'L';
            chars3D[1, 2, 1] = 'M';
            var testArray3DChar = GetEntityGraph(chars3D);
            var testArray3DCharStr = testArray3DChar.ToString();

            var dic = new Dictionary<string, char[, ,]>();
            dic.Add("A", chars3D);
            dic.Add("B", chars3D);
            dic.Add("C", chars3D);
            dic.Add("D", chars3D);
            var testDictionary = GetEntityGraph(dic);
            var testDictionaryStr = testDictionary.ToString();

            var dic2 = new Dictionary<TestClass, char[, ,]>();
            dic2.Add(new TestClass(), chars3D);
            dic2.Add(new TestClass(), chars3D);
            var testDictionary2 = GetEntityGraph(dic2);
            var testDictionary2Str = testDictionary2.ToString();
            var outputNewtonsoft2 = JsonConvert.SerializeObject(dic2, Formatting.Indented);

            TestCustomClass();
            
            //var properties = instance.GetProperties();
            //var print = properties.Select(f => f.Name + ", " + f.ParentType.Name + ", IsOverride=" + f.IsOverride + ", IsGetPublic=" + f.IsGetPublic + ", IsGetPrivate=" + f.IsGetPrivate + ", IsStatic=" + f.IsStatic + ", IsVirtual=" + f.IsVirtual + ", IsExplicitlyImpl=" + f.IsExplicitlyImpl);
            //var outputNewtonsoft2 = JsonConvert.SerializeObject(print, Formatting.Indented);
            //var outputNewtonsoft = JsonConvert.SerializeObject(obj);
        }

        public static Expression<Instance> GetEntityGraph(object obj)
        {
            var instanceRoot = GetInstance(obj, "");
            var graph = ExpressionBuilder<Instance>.Build(
                new List<Instance>() { instanceRoot },
                f =>
                {
                    return GetChildren(f);
                }
                , true, true, true,
                f => GetInstanceString(f)
            ).FirstOrDefault();

            return graph;
        }

        public static List<Instance> GetChildren(Instance instance)
        {
            var list = new List<Instance>();

            var fields = instance.GetFields().ToList();
            foreach (var field in fields)
            { 
                list.Add(GetInstance(field.Value, field.Name));
            }
  
            var properties = instance.GetProperties().ToList();
            foreach (var property in properties)
            { 
                foreach (var value in property.Values)
                {
                    var parameterStr = "";
                    if (value.Parameters != null)
                    {
                        foreach (var param in value.Parameters)
                        {
                            parameterStr += parameterStr == "" ? "" : ", ";
                            parameterStr += param.Value.ToString();
                        }

                        parameterStr = "[" + parameterStr + "]";
                    }

                    list.Add(GetInstance(value.Value, property.Name + parameterStr));
                }
            }

            return list;
        }

        public static string GetInstanceString(Instance instance)
        {
            //return instance.ObjectType.ToString() + ":" + instance.Source + ":" + instance.Object.ToString();
            var objString = "";
            if (CanGetChildren(instance.Object, instance.ObjectType))
                objString = instance.ToString();
            else
                objString = instance.Object.ToString();

            if (instance.ContainerName == "")
                return objString;

            return instance.ContainerName + ":" + objString;
        }

        public static Instance GetInstance(object obj, string containerName)
        {
            var instance = new Instance(obj, null, containerName);
            instance.MethodReaders.Clear();
            instance.MethodReaders.Add(new MethodReaderTestClass());

            instance.FilterTypes = (value, type) =>
            {
                var typesParents = new List<Type>();
                typesParents.Add(type);
                typesParents.AddRange(ReflectionHelper.GetAllParentTypes(type, true).Distinct());
                return typesParents;
            };

            instance.FilterMethods = (value, type) =>
            {
                List<MethodInfo> methods = null;
                //if (!type.IsPrimitive)
                //    methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(f => !f.IsSpecialName).ToList();

                return methods;
            }; ;

            instance.FilterFields = (value, type) =>
            {
                List<FieldInfo> fields = null;
                if (CanGetChildren(value, type) && !(value is Array) && !(value is System.Collections.IDictionary))
                    fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();

                return fields;
            };

            instance.FilterProperties = (value, type) =>
            {
                List<PropertyInfo> properties = null;

                if (value is Array)
                { 
                    if (type ==  typeof(System.Collections.IList))
                        properties = typeof(System.Collections.IList).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(f => f.GetIndexParameters().Length > 0).ToList();
                }
                else if (value is System.Collections.IDictionary)
                { 
                    if (type == typeof(System.Collections.IDictionary))
                        properties = typeof(System.Collections.IDictionary).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(f => f.GetIndexParameters().Length > 0).ToList();
                }
                else if (CanGetChildren(value, type))
                { 
                    properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();
                }

                return properties;
            };

            instance.Reflect();

            return instance;
        }

        public static bool CanGetChildren(object value, Type type)
        {
            if (value is Array || value is System.Collections.IDictionary)
                return true;

            return (!type.IsPrimitive && !type.Namespace.Equals("System") && !type.Namespace.StartsWith("System."));
        }

        public static bool GetMembers(object value, Type type)
        {
            if (value is Array)
                return true;

            return (!type.IsPrimitive && !type.Namespace.Equals("System") && !type.Namespace.StartsWith("System."));
        }

        public static void TestCustomClass()
        {
            var obj = new TestClass();
            var instanceRoot = new Instance(obj);
            instanceRoot.PropertyReaders.Add(new PropertyReadIndexersTestClass());
            instanceRoot.MethodReaders.Add(new MethodReaderTestClass());
            instanceRoot.Reflect();

            var build = ExpressionBuilder<Instance>.Build(
                new List<Instance>() { instanceRoot },
                f =>
                {
                    var list = new List<Instance>();
                    foreach (var field in f.GetFields())
                        list.Add(GetInstance(field.Value, field.Name));

                    return list;
                }
                , true, true, true,
                f =>
                {
                    return f.ContainerName + "_" + f.Name;
                }).FirstOrDefault();
        }
    }
}
