using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ExpressionGraph.Tests.Reflection
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Reflection_Default()
        {
            var testClass = new ClassTestTypes();
            var reflection = testClass
                .AsReflection()
                .SelectTypes((value) => value is string, (value) => ReflectionUtils.GetAllParentTypes(value.GetType(), true));

            var instanceReflecteds = reflection.ReflectTree().ToList();
            var objects = reflection.ReflectTree().Objects().ToList();

            var query = reflection.Query();
            var instanceReflectedsByQuery = query.Where(f => f.Entity.ObjectType == typeof(string)).ToEntities();
        }
    }
}
