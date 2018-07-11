using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class ToStringTests
    {
        public int Prop1 { get; set; } = 100;
        public string field1 = "ABC";

        [Fact]
        public void PropertyInt_WithValue_ReturnPropertyEntityItemAsString()
        {
            var prop1 = this.GetType().GetProperties().Where(p => p.Name == "Prop1").First();
            var root = new ComplexEntity(this);
            var prop1Complex = new PropertyEntity(root, prop1);
            var result = prop1Complex.ToString();
            Assert.Equal("@System.Int32.Prop1:100", result);
        }
    }
}
