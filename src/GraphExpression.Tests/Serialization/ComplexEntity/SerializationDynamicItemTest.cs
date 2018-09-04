using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationDynamicItemTest
    {
        [Fact]
        public void Normal()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var dynamicItem = new DynamicItemEntity(expression, "PropInt", 100);
            var result = dynamicItem.ToString();
            Assert.Equal("{@PropInt: 100}", result);
        }

        [Fact]
        public void EncloseItem()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.EncloseItem = false;
            var dynamicItem = new DynamicItemEntity(expression, "PropInt", 100);
            var result = dynamicItem.ToString();
            Assert.Equal("@PropInt: 100", result);
        }

        [Fact]
        public void PropertySymbol()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ItemsSerialize.OfType<DynamicItemSerialize>().First().Symbol = "*";
            var dynamicItem = new DynamicItemEntity(expression, "PropInt", 100);
            var result = dynamicItem.ToString();
            Assert.Equal("{*PropInt: 100}", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.FullTypeName;
            var dynamicItem = new DynamicItemEntity(expression, "PropInt", 100);
            var result = dynamicItem.ToString();
            Assert.Equal("{@System.Int32.PropInt: 100}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            var dynamicItem = new DynamicItemEntity(expression, "PropInt", 100);
            var result = dynamicItem.ToString();
            Assert.Equal("{@PropInt: 100}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.TypeName;
            var dynamicItem = new DynamicItemEntity(expression, "PropInt", 100);
            var result = dynamicItem.ToString();
            Assert.Equal("{@Int32.PropInt: 100}", result);
        }

        [Fact]
        public void ValueNull()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var dynamicItem = new DynamicItemEntity(expression, "PropString", null);
            var result = dynamicItem.ToString();
            Assert.Equal("{@PropString: null}", result);
        }

        [Fact]
        public void ValueNull_WithoutTypeAndEncloseAndSymbolProperty()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var dynamicItem = new DynamicItemEntity(expression, "PropString", null);
            serialization.ItemsSerialize.OfType<DynamicItemSerialize>().First().Symbol = null;
            serialization.EncloseItem = false;

            var result = dynamicItem.ToString();
            Assert.Equal("PropString: null", result);
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
