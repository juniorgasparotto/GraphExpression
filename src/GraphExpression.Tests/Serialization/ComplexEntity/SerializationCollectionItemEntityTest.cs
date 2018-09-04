using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationCollectionItemEntityTest
    {
        public class ComplexItem
        {
            public int fieldInt = 100;
        }

        [Fact]
        public void Normal()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var listItem = new CollectionItemEntity(expression, 0, "value");
            var result = listItem.ToString();
            Assert.Equal("{[0]: \"value\"}", result);
        }

        [Fact]
        public void ComplexValue()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var value = new ComplexItem();
            var listItem = new CollectionItemEntity(expression, 0, value);
            var result = listItem.ToString();
            Assert.Equal($"{{[0].{value.GetHashCode()}}}", result);
        }

        [Fact]
        public void EncloseItem()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.EncloseItem = false;
            var listItem = new CollectionItemEntity(expression, 1, "value");
            var result = listItem.ToString();
            Assert.Equal("[1]: \"value\"", result);
        }

        [Fact]
        public void PropertyAndFieldSymbol_NotImpactInResult()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ItemsSerialize.OfType<PropertySerialize>().First().Symbol = "*";
            serialization.ItemsSerialize.OfType<FieldSerialize>().First().Symbol = "*";
            var listItem = new CollectionItemEntity(expression, 1, "value");
            var result = listItem.ToString();
            Assert.Equal("{[1]: \"value\"}", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.FullTypeName;
            var listItem = new CollectionItemEntity(expression, 1, 100);
            var result = listItem.ToString();
            Assert.Equal("{System.Int32.[1]: 100}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            var listItem = new CollectionItemEntity(expression, 1000, 100);
            var result = listItem.ToString();
            Assert.Equal("{[1000]: 100}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var listItem = new CollectionItemEntity(expression, 1000, 100);
            var result = listItem.ToString();
            Assert.Equal("{Int32.[1000]: 100}", result);
        }

        [Fact]
        public void ValueNull()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var listItem = new CollectionItemEntity(expression, 1000, null);
            var result = listItem.ToString();
            Assert.Equal("{[1000]: null}", result);
        }

        [Fact]
        public void ValueNull_WithoutTypeAndEnclose()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            serialization.EncloseItem = false;

            var listItem = new CollectionItemEntity(expression, 1000, null);
            var result = listItem.ToString();
            Assert.Equal("[1000]: null", result);
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
