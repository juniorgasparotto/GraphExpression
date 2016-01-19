using ExpressionGraph.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExpressionGraph;
//using Microsoft.CSharp.RuntimeBinder;

namespace ExpressionGraph.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var testReflection = new TestsReflection(true);
            testReflection.TestReflection_Lists();
            testReflection.TestReflection_Custom();
            testReflection.TestReflection_Primitives();
            testReflection.TestReflection_Array();
            testReflection.TestReflection_CallMethod();

            //var lst = new List<object>();
            //lst.Add(new TestClass3());
            //lst.Add("Test");

            //var query = lst.AsReflection()
            //    .SelectTypes
            //    (
            //        (obj) =>
            //        {
            //            return obj.GetType() == typeof(TestClass3);
            //        },
            //        (obj) =>
            //        {
            //            return ReflectionUtils.GetAllParentTypes(obj.GetType());
            //        }
            //    )
            //    .Query();
        }
    }
}
