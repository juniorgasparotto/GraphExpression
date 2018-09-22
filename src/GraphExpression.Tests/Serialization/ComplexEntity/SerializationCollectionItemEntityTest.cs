
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Serialization
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
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var listItem = new CollectionItemEntity(expression, 0, "value");
            var result = listItem.ToString();
            Assert.Equal("[0]: \"value\"", result);
        }

        [Fact]
        public void ComplexValue()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var value = new ComplexItem();
            var listItem = new CollectionItemEntity(expression, 0, value);
            var result = listItem.ToString();
            Assert.Equal($"[0].{value.GetHashCode()}", result);
        }

        [Fact]
        public void PropertyAndFieldSymbol_NotImpactInResult()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ItemsSerialize.OfType<PropertySerialize>().First().Symbol = "*";
            serialization.ItemsSerialize.OfType<FieldSerialize>().First().Symbol = "*";
            var listItem = new CollectionItemEntity(expression, 1, "value");
            var result = listItem.ToString();
            Assert.Equal("[1]: \"value\"", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.FullTypeName;
            var listItem = new CollectionItemEntity(expression, 1, 100);
            var result = listItem.ToString();
            Assert.Equal("System.Int32.[1]: 100", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            var listItem = new CollectionItemEntity(expression, 1000, 100);
            var result = listItem.ToString();
            Assert.Equal("[1000]: 100", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var listItem = new CollectionItemEntity(expression, 1000, 100);
            var result = listItem.ToString();
            Assert.Equal("Int32.[1000]: 100", result);
        }

        [Fact]
        public void ValueNull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var listItem = new CollectionItemEntity(expression, 1000, null);
            var result = listItem.ToString();
            Assert.Equal("[1000]: null", result);
        }

        [Fact]
        public void ValueNull_WithoutType()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;

            var listItem = new CollectionItemEntity(expression, 1000, null);
            var result = listItem.ToString();
            Assert.Equal("[1000]: null", result);
        }
    }
}
