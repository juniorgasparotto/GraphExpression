using GraphExpression.Examples.Models;
using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphExpression.Examples
{
    public partial class Examples
    {
        [Action(Help = "")]
        public void EntityFactory()
        {
            var root = new Entity(0) + new Entity("Name: Entity name ;)");
            var factory = new ComplexEntityFactory<CircularEntity>(root);

            // Build entity
            factory.Build();

            var entity = factory.Value;
            System.Console.WriteLine(entity.Name);
        }

        [Action(Help = "")]
        public void EntityFactory2()
        {
            var root = new Entity(0) + new Entity("Child", 0);
            var factory = new ComplexEntityFactory<MyClass>(root);

            // Build entity
            factory.Build();

            var entity = factory.Value;
            System.Console.WriteLine(entity == entity.Child);
        }

        [Action(Help = "")]
        public void EntityFactory3()
        {
            var root = new Entity(0) + new Entity("_intValue", "1000");
            var factory = new ComplexEntityFactory<MyClass>(root);

            // Build entity
            factory.Build();

            var entity = factory.Value;
            System.Console.WriteLine(typeof(MyClass).GetField("_intValue", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(entity));
        }

        private class MyClass
        {
            private int _intValue;
            public MyClass Child { get; set; }
            public int IntValue => _intValue;
        }
    }
}
