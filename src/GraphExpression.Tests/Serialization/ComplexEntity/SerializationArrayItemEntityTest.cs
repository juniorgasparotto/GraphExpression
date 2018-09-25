
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Serialization
{
    public class SerializationArrayItemEntityTest
    {
        public class ComplexItem
        {
            public int fieldInt = 100;
        }

        [Fact]
        public void Normal()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var listItem = new ArrayItemEntity(expression, 0, "value");
            var result = listItem.ToString();
            Assert.Equal("[0]: value", result);
        }

        [Fact]
        public void Multidimensional()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var listItem = new ArrayItemEntity(expression, new int[] { 10, 10, 10 }, "value");
            var result = listItem.ToString();
            Assert.Equal("[10,10,10]: value", result);
        }

        [Fact]
        public void ComplexValue()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var value = new ComplexItem();
            var listItem = new ArrayItemEntity(expression, 0, value);
            var result = listItem.ToString();
            Assert.Equal($"[0].{value.GetHashCode()}", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.FullTypeName;
            var listItem = new ArrayItemEntity(expression, 1, 100);
            var result = listItem.ToString();
            Assert.Equal("System.Int32.[1]: 100", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            var listItem = new ArrayItemEntity(expression, 1000, 100);
            var result = listItem.ToString();
            Assert.Equal("[1000]: 100", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var listItem = new ArrayItemEntity(expression, 1000, 100);
            var result = listItem.ToString();
            Assert.Equal("Int32.[1000]: 100", result);
        }

        [Fact]
        public void ValueNull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var listItem = new ArrayItemEntity(expression, 1000, null);
            var result = listItem.ToString();
            Assert.Equal("[1000]: null", result);
        }

        [Fact]
        public void ValueNull_WithoutType()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;

            var listItem = new ArrayItemEntity(expression, 1000, null);
            var result = listItem.ToString();
            Assert.Equal("[1000]: null", result);
        }
    }
}
