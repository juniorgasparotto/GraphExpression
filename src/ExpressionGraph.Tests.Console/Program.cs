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
            var strBuilder = new StringBuilder();

            var obj = new TestClass();
            var instance = new Instance(obj);
            instance.PropertyReaders.Add(new PropertyReadIndexersTestClass());
            instance.MethodReaders.Add(new MethodReaderTestClass());
            instance.Reflect();

            var properties = instance.GetProperties();
            
            var print = properties.Select(f => f.Name + ", " + f.ParentType.Name + ", IsOverride=" + f.IsOverride + ", IsGetPublic=" + f.IsGetPublic + ", IsGetPrivate=" + f.IsGetPrivate + ", IsStatic=" + f.IsStatic + ", IsVirtual=" + f.IsVirtual + ", IsExplicitlyImpl=" + f.IsExplicitlyImpl);
            var outputNewtonsoft2 = JsonConvert.SerializeObject(print, Formatting.Indented);
            var outputNewtonsoft = JsonConvert.SerializeObject(obj);
        }
    }
}
