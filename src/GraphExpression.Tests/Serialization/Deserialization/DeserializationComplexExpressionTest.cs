using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Serialization
{
    public class DeserializationComplexExpressionTest
    {
        public class Class1
        {
            public int PropNoSet { get; }
            private Class1() { }
        }

        [Fact]
        public void DeserializeComplex_AnonymousObject_ParseEntityToExpandoObject()
        { 
            var obj = new
            {
                Prop1 = "Value"
            };

            var expressionStr = obj.AsExpression().DefaultSerializer.Serialize();
            var serializer = new ExpressionDeserializer<DeserializeEntity>();
            var deserialized = serializer.Deserialize(expressionStr).Entity;
            var cast = ((dynamic)deserialized);
            Assert.IsType<ExpandoObject>(deserialized);
            Assert.Equal("Value", cast.Prop1);
        }

        [DebuggerDisplay("{Name}")]
        public class DeserializeEntity
        {
            private readonly List<DeserializeEntity> children;

            public string Name { get; private set; }
            public object Entity { get; private set; }
            public string MemberName { get; private set; }
            public string ValueRaw { get; private set; }

            public int Count => children.Count;
            public DeserializeEntity this[int index] => children[index];

            public DeserializeEntity(string name)
            {
                this.children = new List<DeserializeEntity>();
                this.Name = name;
            }

            public IEnumerable<DeserializeEntity> Children { get => children; }

            public static DeserializeEntity operator +(DeserializeEntity a, DeserializeEntity b)
            {
                if (a.Entity == null && a.Name.StartsWith("<>"))
                    a.Entity = new ExpandoObject();

                if (b.Entity == null && b.Name.StartsWith("<>"))
                {
                    b.Entity = new ExpandoObject();

                    int index = b.Name.IndexOf(": ");
                    string name = b.Name.Substring(0, index);
                    string value = b.Name.Substring(index + 1);


                    b.MemberName = name;
                    b.ValueRaw = value;
                }

                if (a.Entity is ExpandoObject obj)
                {
                    obj.TryAdd(b.Name, b.Entity);
                }

                return a;
            }

            public static DeserializeEntity operator -(DeserializeEntity a, DeserializeEntity b)
            {
                a.children.Remove(b);
                return a;
            }

            public override string ToString()
            {
                return this.Name;
            }
        }
    }
}
