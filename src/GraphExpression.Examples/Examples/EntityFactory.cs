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

        [Action(Help = "")]
        public void EntityFactory4()
        {
            var root = new Entity(0) + new Entity("System.Int32._intValue: 1000");
            var factory = new ComplexEntityFactory<MyClass>(root);

            // Build entity
            factory.Build();

            var entity = factory.Value;
            System.Console.WriteLine(typeof(MyClass).GetField("_intValue", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(entity));
        }

        [Action(Help = "")]
        public void EntityFactory5()
        {
            var root = new Entity(0) 
                + (new Entity("A", 1) + new Entity("MyProp", "10"))
                + (new Entity("B", 2) + new Entity("MyProp", "20"));

            var factory = new ComplexEntityFactory<ClassWithAbstractAndInterface>(root);
            factory.AddMapType<Interface, ImplementAbstractAndInterface>();
            factory.AddMapType<AbstractClass, ImplementAbstractAndInterface>();

            // Build entity
            factory.Build();

            // Build entity and get typed value
            var entity = factory.Build().Value;
            System.Console.WriteLine(entity.A.MyProp);
            System.Console.WriteLine(entity.A.GetType().Name);
            System.Console.WriteLine(entity.B.MyProp);
            System.Console.WriteLine(entity.B.GetType().Name);
        }

        private class MyClass
        {
            private int _intValue;
            public MyClass Child { get; set; }
            public int IntValue => _intValue;
        }

        public interface Interface
        {
            int MyProp { get; set; }
        }

        public abstract class AbstractClass : Interface
        {
            public int MyProp { get; set; }
        }

        public class ImplementAbstractAndInterface : AbstractClass
        {
        }

        public class ClassWithAbstractAndInterface
        {
            public Interface A { get; set; }
            public AbstractClass B { get; set; }
        }
    }
}
