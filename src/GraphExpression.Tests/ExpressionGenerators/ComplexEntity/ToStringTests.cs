using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
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
            var expression = new Expression<object>();
            var prop1Complex = new PropertyEntity(expression, this, prop1);
            expression.AsSerializer();
            var result = prop1Complex.ToString();
            Assert.Equal("@System.Int32.Prop1:100", result);
        }
    }
}
