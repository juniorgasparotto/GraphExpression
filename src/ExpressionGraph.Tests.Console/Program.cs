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
            // 3 dimensions of chars
            var chars4 = new char[2, 3, 2];
            chars4[0, 0, 0] = 'A';
            chars4[0, 0, 1] = 'B';
            chars4[0, 1, 0] = 'C';
            chars4[0, 1, 1] = 'D';
            chars4[0, 2, 0] = 'E';
            chars4[0, 2, 1] = 'F';
            chars4[1, 0, 0] = 'G';
            chars4[1, 0, 1] = 'H';
            chars4[1, 1, 0] = 'I';
            chars4[1, 1, 1] = 'J';
            chars4[1, 2, 0] = 'L';
            chars4[1, 2, 1] = 'M';
            var i4 = GetInstance(chars4, "");

            // 3 dimensions of chars
            var chars3 = new char[2, 2, 2] { { { 'A', 'B' }, { 'C', 'D' } }, { { 'E', 'F' }, { 'G', 'H' } } };
            chars3[0, 0, 0] = 'A';
            chars3[0, 0, 1] = 'B';
            chars3[0, 1, 0] = 'C';
            chars3[0, 1, 1] = 'D';
            chars3[1, 0, 0] = 'E';
            chars3[1, 0, 1] = 'F';
            chars3[1, 1, 0] = 'G';
            chars3[1, 1, 1] = 'H';
            var i3 = GetInstance(chars3, "");

            // single array of chars
            var chars = new char[] { 'A', 'B', 'C' };
            var type = typeof(System.Collections.IList);
            var props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(f=> f.GetIndexParameters().Length > 0).ToList();
            var i = GetInstance(chars, "");

            // 2 dimentions array
              var chars2 = new char[1,2] { {'A', 'B'} };
            chars2[0, 0] = 'A';
            chars2[0, 1] = 'B';
            var i2 = GetInstance(chars2, "");

            var type2 = typeof(System.Collections.IList);
            var props2 = type2.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(f => f.GetIndexParameters().Length > 0).ToList();


            var type3 = chars3.GetType();
            var props3 = type3.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();
            //var ins = GetInstance(chars3, "");

            var instanceRoot = GetInstance(4, "Root");
            var build2 = ExpressionBuilder<Instance>.Build(
                new List<Instance>() { instanceRoot },
                f =>
                {
                    return GetChildren(f);
                }
                , false, true, true,
                f => GetInstanceString(f)
            ).FirstOrDefault();

            var r = build2.ToString();

            instanceRoot = GetInstance("ABC", "Root");
            var build3 = ExpressionBuilder<Instance>.Build(
                new List<Instance>() { instanceRoot },
                f =>
                {
                    return GetChildren(f);
                }
                , false, true, true,
                 f => GetInstanceString(f)
            ).FirstOrDefault();

            instanceRoot = GetInstance(true, "Root");
            var build4 = ExpressionBuilder<Instance>.Build(
                new List<Instance>() { instanceRoot },
                f =>
                {
                    return GetChildren(f);
                }
                , false, true, true,
                 f => GetInstanceString(f)
            ).FirstOrDefault();

            instanceRoot = GetInstance(10.999m, "Root");
            var build5 = ExpressionBuilder<Instance>.Build(
                new List<Instance>() { instanceRoot },
                f =>
                {
                    return GetChildren(f);
                }
                , false, true, true,
                 f => GetInstanceString(f)
            ).FirstOrDefault();

            instanceRoot = GetInstance(new char[] { 'A', 'B', 'C' }, "Root");
            var build6 = ExpressionBuilder<Instance>.Build(
                new List<Instance>() { instanceRoot },
                f =>
                {
                    return GetChildren(f);
                }
                , false, true, true,
                f => GetInstanceString(f)
            ).FirstOrDefault();

            var s = build3.ToString();
            var b = build4.ToString();
            var d = build5.ToString();
            var ac = build6.ToString();

            var strBuilder = new StringBuilder();

            var obj = new TestClass();
            instanceRoot = new Instance(obj);
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

                    // prevent property withow get or method with return void
                    //if (f.Values != null)
                    //{ 
                    //    var value = f.Values.Where(w => w.Parameters == null).Select(s => s.Value).FirstOrDefault();
                    //    var instance2 = new Instance(value);
                    //    instance2.PropertyReaders.Add(new PropertyReadIndexersTestClass());
                    //    instance2.MethodReaders.Add(new MethodReaderTestClass());
                    //    instance2.Reflect();
                    //    return instance2.GetProperties();
                    //}

                    //return null;
                }
                , true, true, true,
                f =>
                {
                    return f.Source + "_" + f.Name;
                }).FirstOrDefault();

            //var properties = instance.GetProperties();
            
            //var print = properties.Select(f => f.Name + ", " + f.ParentType.Name + ", IsOverride=" + f.IsOverride + ", IsGetPublic=" + f.IsGetPublic + ", IsGetPrivate=" + f.IsGetPrivate + ", IsStatic=" + f.IsStatic + ", IsVirtual=" + f.IsVirtual + ", IsExplicitlyImpl=" + f.IsExplicitlyImpl);
            //var outputNewtonsoft2 = JsonConvert.SerializeObject(print, Formatting.Indented);
            //var outputNewtonsoft = JsonConvert.SerializeObject(obj);
        }

        public static List<Instance> GetChildren(Instance instance)
        {
            var list = new List<Instance>();
            var fields = instance.GetFields().ToList();
            foreach (var field in fields)
                list.Add(GetInstance(field.Value, field.Name));
                
            var properties = instance.GetProperties().ToList();
            foreach (var property in properties)
            { 
                foreach (var value in property.Values)
                {
                    list.Add(GetInstance(value.Value, property.Name + value.GetParametersToString()));
                }
            }

            return list;
        }

        public static string GetInstanceString(Instance instance)
        {
            //return instance.ObjectType.ToString() + ":" + instance.Source + ":" + instance.Object.ToString();
            return instance.Source + ":" + instance.Object.ToString();
        }

        public static Instance GetInstance(object obj, string source)
        {
            var instance = new Instance(obj, null, source);
            instance.MethodReaders.Clear();
            instance.MethodReaders.Add(new MethodReaderTestClass());

            instance.FilterMethods = (value, type) =>
            {
                List<MethodInfo> methods = null;
                //if (!type.IsPrimitive)
                //    methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(f => !f.IsSpecialName).ToList();

                return methods;
            }; ;

            instance.FilterFields = (value, type) =>
            {
                //if (value is Array)
                //    return null;

                List<FieldInfo> fields = null;
                if (!type.IsPrimitive)
                    fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();

                return fields;
            };

            instance.FilterProperties = (value, type) =>
            {
                List<PropertyInfo> properties = null;
                if (!type.IsPrimitive)
                    properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();

                return properties;
            };

            instance.FilterTypes = (value, type) =>
            {
                var typesParents = new List<Type>();
                typesParents.Add(type);
                typesParents.AddRange(ReflectionHelper.GetAllParentTypes(type, true).Distinct());
                return typesParents;
            };

            instance.Reflect();

            return instance;
        }
    }
}
