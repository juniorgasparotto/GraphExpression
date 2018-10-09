using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.EntityFactory
{
    public partial class EntityFactoryTests
    {
        [Fact]
        public void FactoryNonTyped_CreateTree()
        {
            var A = GetEntity(123) + (GetEntity("B: valueB") + GetEntity("C", "valueC"));

            TestEntity(A, "123", null, "123", false, "123");
            TestEntity(A["B"], "B", "valueB", null, true, "B: valueB");
            TestEntity(A["B"]["C"], "C", "valueC", null, true, "C: valueC");
            Assert.Same(A["B"], A[0]);
        }

        [Fact]
        public void FactoryTyped_CreateInstance_A_plus_Pharentesis_plus_B()
        {
            var root =
                    new Entity("Root.0")
                    + (
                        new Entity("children.1")
                            + (
                                new Entity("[0].2")
                                    + new Entity("Name: Name1")
                                    + new Entity("Name: Name1 (last)"))
                            + (
                                new Entity("[1].3")
                                    + new Entity("Name: Name2"))
                            + (
                                new Entity("[2].4")
                                    + new Entity("Name: Name3")))
                    + new Entity("Name: Root");

            var build = new ComplexEntityFactory<CircularEntity>(root).Build();
            var value = build.Value;
            Assert.Equal("Root", root.Name);
            Assert.Equal("Root", value.Name);

            Assert.Equal(3, value.Children.Count());
            Assert.Equal("Name1 (last)", value.Children.ElementAt(0).Name);
            Assert.Equal("Name2", value.Children.ElementAt(1).Name);
            Assert.Equal("Name3", value.Children.ElementAt(2).Name);

            Assert.Same(root["children"].Value, value.Children);
            Assert.Same(root["children"]["[0]"].Value, value.Children.ElementAt(0));
            Assert.Same(root["children"]["[1]"].Value, value.Children.ElementAt(1));
            Assert.Same(root["children"]["[2]"].Value, value.Children.ElementAt(2));
            Assert.Equal("Root", root["Name"].Value);
        }

        [Fact]
        public void FactoryTyped_CreateInstance_A_plus_B_plus_Pharentesis()
        {
            var root = (new Entity("CircularEntity.0")
                            + new Entity("Name: Root2")
                            + (new Entity("children.1")
                                + (new Entity("[0].2")
                                    + new Entity("Name: Name1")
                                    + new Entity("Name: Name1 (last)"))
                                + (new Entity("[1].3")
                                    + new Entity("Name: Name2"))
                                + (new Entity("[2].4")
                                    + new Entity("Name: Name3"))));

            var build = new ComplexEntityFactory<CircularEntity>(root).Build();
            var value = build.Value;

            Assert.Equal("CircularEntity", root.Name);
            Assert.Equal("Root2", value.Name);

            Assert.Equal(3, value.Children.Count());
            Assert.Equal("Name1 (last)", value.Children.ElementAt(0).Name);
            Assert.Equal("Name2", value.Children.ElementAt(1).Name);
            Assert.Equal("Name3", value.Children.ElementAt(2).Name);

            Assert.Same(root["children"].Value, value.Children);
            Assert.Same(root["children"]["[0]"].Value, value.Children.ElementAt(0));
            Assert.Same(root["children"]["[1]"].Value, value.Children.ElementAt(1));
            Assert.Same(root["children"]["[2]"].Value, value.Children.ElementAt(2));
            Assert.Equal("Root2", root["Name"].Value);
        }

        [Fact]
        public void FactoryTyped_CreateArray()
        {
            var root = GetEntity("Class1[].55467396")
                            + (GetEntity("[0].56235422")
                                + GetEntity("Prop1: 1"))
                            + (GetEntity("[1].31423778")
                                + GetEntity("Prop1: 2"))
                            + (GetEntity("[2].29279655")
                                + GetEntity("Prop1: 3"))
                            + GetEntity("[3].56235422"); // Repeat 56235422

            var value = new ComplexEntityFactory<Class1[]>(root).Build().Value;            
            Assert.Equal(1, value[0].Prop1);
            Assert.Equal(2, value[1].Prop1);
            Assert.Equal(3, value[2].Prop1);
            Assert.Same(value[0], value[3]);
        }

        [Fact]
        public void FactoryTyped_CreateSimpleClass()
        {
            var root = GetEntity("CircularEntity.0")
                            + GetEntity("Name: A") 
                            + GetEntity("Count: 0") 
                            + (GetEntity("Children.3") 
                                + GetEntity("Capacity: 0") 
                                + GetEntity("Count: 0"));

            var build = new ComplexEntityFactory<CircularEntity>(root).Build();
            Assert.Equal("A", build.Value.Name);
            Assert.Empty(build.Value.Children);
        }

        //[Fact]
        //public void CircularFactoryTyped_CreateSimpleClass()
        //{
        //    var root = GetEntity("CircularEntity.0")
        //                    + GetEntity("Name: A")
        //                    + GetEntity("Count: 0")
        //                    + (GetEntity("Children.3")
        //                        + GetEntity("Capacity: 0")
        //                        + GetEntity("Count: 0"));

        //    var build = new CircularEntityFactory<CircularEntity>().Build(root);
        //    Assert.Equal("A", build.Value.Name);
        //    Assert.Empty(build.Value.Children);
        //}

        private Entity GetEntity(string name)
        {
            return new Entity(name);
        }

        private Entity GetEntity(int complexEntityId)
        {
            return new Entity(complexEntityId);
        }

        private Entity GetEntity(string name, string value)
        {
            return new Entity(name, value);
        }

        #region auxs 

        private void TestEntity(Entity item, string key, string value, string complexId, bool isPrimitive, string raw)
        {
            Assert.Equal(key, item.Name);
            Assert.Equal(complexId, item.ComplexEntityId);
            Assert.Equal(isPrimitive, item.IsPrimitive);
            Assert.Equal(value, item.Value);
            Assert.Equal(raw, item.Raw);
        }

        public class Class1
        {
            public int Prop1 { get; set; }
            public Class1(string prop1) { }
        }
        #endregion
    }
}
