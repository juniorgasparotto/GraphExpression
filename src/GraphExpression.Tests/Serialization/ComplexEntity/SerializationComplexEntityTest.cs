
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationComplexEntityTest
    {
        [Fact]
        public void Normal()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"{this.GetHashCode()}", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.FullTypeName;
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"GraphExpression.Tests.SerializationComplexEntityTest.{this.GetHashCode()}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"{this.GetHashCode()}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"SerializationComplexEntityTest.{this.GetHashCode()}", result);
        }

        [Fact]
        public void Value_Null()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var complex = new ComplexEntity(expression, null);
            var result = complex.ToString();
            Assert.Equal("null", result);
        }

        private System.Reflection.FieldInfo GetFieldByName(string name)
        {
            return this.GetType().GetFields().Where(p => p.Name == name).First();
        }
    }
}
