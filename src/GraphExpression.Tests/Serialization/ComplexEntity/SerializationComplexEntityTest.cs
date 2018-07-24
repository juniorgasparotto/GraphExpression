using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationComplexEntityTest
    {
        public int Prop1 { get; set; } = 100;
        public string field1 = "ABC";

        [Fact]
        public void PropertyIntWithValue_SerilazationFull_ReturnEntityItemAsString()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("Prop1"));
            var result = prop1Complex.ToString();
            Assert.Equal("@System.Int32.Prop1:100", result);
        }

        [Fact]
        public void PropertyIntWithValue_SerilazationFullAndChangePropertySymbol_ReturnEntityItemAsString()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.PropertySymbol = "*";
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("Prop1"));
            var result = prop1Complex.ToString();
            Assert.Equal("*System.Int32.Prop1:100", result);
        }

        private System.Reflection.PropertyInfo GetPropertyByName(string name)
        {
            return this.GetType().GetProperties().Where(p => p.Name == name).First();
        }

        private Expression<object> GetExpression()
        {
            var expression = new Expression<object>();
            expression.DefaultSerializer = new SerializationAsComplexExpression(expression);
            return expression;
        }

        private SerializationAsComplexExpression GetSerialization(Expression<object> expression)
        {
            return (SerializationAsComplexExpression)expression.DefaultSerializer;
        }
    }
}
