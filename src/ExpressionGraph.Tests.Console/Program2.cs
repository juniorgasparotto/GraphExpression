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
    class Program2
    {
        static void Main2(string[] args)
        {
            var testClass = new Son();

            var reflection = testClass
                .AsExpression(c => {
                    c.AddValueReaderForProperties(new PropertyReadIndexersTestClass());
                    c.SelectTypes((value) => value is string, (value) => ReflectionUtils.GetAllParentTypes(value.GetType(), true));
                });

            var instanceReflecteds = reflection.AsExpression().ToList();
            var objects = reflection.AsExpression().ToEntities().ToList();

            var query = reflection.AsExpression();
            var instanceReflectedsByQuery = query.Where(f=>f.Entity.ObjectType == typeof(string)).ToEntities();
            //var objectsByQuery = query.Objects().ToList();
                        
            var dic2 = new Dictionary<Son, decimal[, ,]>();
            dic2.Add(new Son(), new decimal[,,] { { { 0.12m }, { 1000.99999m } } });
            dic2.Add(new Son(), new decimal[,,] { { { 1.12m }, { 2000.99999m } } });
            dic2.Add(new Son(), new decimal[,,] { { { 2.12m }, { 3000.99999m } } });
            dic2.Add(new Son(), new decimal[,,] { { { 3.12m }, { 4000.99999m } } });
            var testDictionary2 = GetEntityGraph(dic2);
            var testDictionary2Str = testDictionary2.ToString();
            var outputNewtonsoft2 = JsonConvert.SerializeObject(dic2, Formatting.Indented);

            TestCustomClass();
        }

        private static Expression<ReflectedInstance> GetEntityGraph(object obj)
        {
            return obj.AsExpression(c =>
                //.Settings(SettingsFlags.ShowFullNameOfType | SettingsFlags.ShowParameterName)
                //.Settings(SettingsFlags.ShowParameterName)
                //.Settings(SettingsFlags.ShowFullNameOfType)
                c.Settings(SettingsFlags.Default)
                 .SelectFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                 .SelectProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));
        }

        public static void TestCustomClass()
        {
            var obj = new Son();
            var instanceRoot = new ReflectedInstance(obj);
            //instanceRoot._propertyValueReaders.Add(new PropertyReadIndexersTestClass());
            //instanceRoot._methodValueReaders.Add(new MethodReaderTestClass());
            //instanceRoot.Reflect();

            var build = GetEntityGraph(instanceRoot);
        }
    }
}
