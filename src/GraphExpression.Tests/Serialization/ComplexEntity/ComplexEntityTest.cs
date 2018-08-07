using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class ComplexEntityTest
    {
        [Fact]
        public void Normal()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"{{{this.GetHashCode()}}}", result);
        }

        [Fact]
        public void EncloseItem()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.EncloseItem = false;
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"{this.GetHashCode()}", result);
        }
        
        [Fact]
        public void ShowTypeFull()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.FullTypeName;
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"{{GraphExpression.Tests.ComplexEntityTest.{this.GetHashCode()}}}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"{{{this.GetHashCode()}}}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.TypeName;
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"{{ComplexEntityTest.{this.GetHashCode()}}}", result);
        }

        [Fact]
        public void Value_Null()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var complex = new ComplexEntity(expression, null);
            var result = complex.ToString();
            Assert.Equal("{null}", result);
        }

        [Fact]
        public void Value_Null_AndNotEnclose()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.EncloseItem = false;
            var complex = new ComplexEntity(expression, null);
            var result = complex.ToString();
            Assert.Equal("null", result);
        }

        private System.Reflection.FieldInfo GetFieldByName(string name)
        {
            return this.GetType().GetFields().Where(p => p.Name == name).First();
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
