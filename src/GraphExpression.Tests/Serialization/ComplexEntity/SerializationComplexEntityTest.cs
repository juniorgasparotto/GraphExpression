
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Serialization
{
    public class SerializationComplexEntityTest
    {
        [Fact]
        public void Normal_ShowTypeOnlyRoot()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var complex = new ComplexEntity(expression, this);
            var result = complex.ToString();
            Assert.Equal($"{this.GetType().Name}.{this.GetHashCode()}", result);
        }

        [Fact]
        public void NotShowTypeInRoot()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
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
            Assert.Equal($"GraphExpression.Tests.Serialization.SerializationComplexEntityTest.{this.GetHashCode()}", result);
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
            Assert.Equal("", result);
        }

        private System.Reflection.FieldInfo GetFieldByName(string name)
        {
            return this.GetType().GetFields().Where(p => p.Name == name).First();
        }
    }
}
