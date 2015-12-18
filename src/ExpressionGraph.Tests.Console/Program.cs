using ExpressionGraph.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExpressionGraph;

namespace ExpressionGraph.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var testClass = new TestClass();
            var tree = testClass
                .AsReflection()
                .AddValueReaderForProperties(new PropertyReadIndexersTestClass())
                .SelectTypes((value) => value is string, (value) => ReflectionUtils.GetAllParentTypes(value.GetType(), true))
                .ReflectTree();

            var entities = tree.ToEntities().ToList();

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

            var dic2 = new Dictionary<TestClass, decimal[, ,]>();
            dic2.Add(new TestClass(), new decimal[,,] { { { 0.12m }, { 1000.99999m } } });
            dic2.Add(new TestClass(), new decimal[,,] { { { 1.12m }, { 2000.99999m } } });
            dic2.Add(new TestClass(), new decimal[,,] { { { 2.12m }, { 3000.99999m } } });
            dic2.Add(new TestClass(), new decimal[,,] { { { 3.12m }, { 4000.99999m } } });
            var testDictionary2 = GetEntityGraph(dic2);
            var testDictionary2Str = testDictionary2.ToString();
            var outputNewtonsoft2 = JsonConvert.SerializeObject(dic2, Formatting.Indented);

            TestCustomClass();
            
            //var properties = instance.GetProperties();
            //var print = properties.Select(f => f.Name + ", " + f.ParentType.Name + ", IsOverride=" + f.IsOverride + ", IsGetPublic=" + f.IsGetPublic + ", IsGetPrivate=" + f.IsGetPrivate + ", IsStatic=" + f.IsStatic + ", IsVirtual=" + f.IsVirtual + ", IsExplicitlyImpl=" + f.IsExplicitlyImpl);
            //var outputNewtonsoft2 = JsonConvert.SerializeObject(print, Formatting.Indented);
            //var outputNewtonsoft = JsonConvert.SerializeObject(obj);
        }

        private static Expression<ReflectInstance> GetEntityGraph(object obj)
        {
            return obj.AsReflection()
                //.Settings(SettingsFlags.ShowFullNameOfType | SettingsFlags.ShowParameterName)
                //.Settings(SettingsFlags.ShowParameterName)
                //.Settings(SettingsFlags.ShowFullNameOfType)
                .Settings(SettingsFlags.Default)
                .SelectFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .SelectProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .ReflectTree();
        }

        public static void TestCustomClass()
        {
            var obj = new TestClass();
            var instanceRoot = new ReflectInstance(obj);
            //instanceRoot._propertyValueReaders.Add(new PropertyReadIndexersTestClass());
            //instanceRoot._methodValueReaders.Add(new MethodReaderTestClass());
            //instanceRoot.Reflect();

            var build = GetEntityGraph(instanceRoot);
        }
    }
}
