using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationDictionaryItemEntityTest
    {
        public class ComplexItem
        {
            public int fieldInt = 100;
        }

        [Fact]
        public void DateTimeKey()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var key = DateTime.Now;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, "value");
            var result = listItem.ToString();
            Assert.Equal($"{{[{keyHashCode}]: \"value\"}}", result);
        }

        [Fact]
        public void ComplexTypeKey()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var key = new ComplexItem();
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, "value");
            var result = listItem.ToString();
            Assert.Equal($"{{[{keyHashCode}]: \"value\"}}", result);
        }

        [Fact]
        public void Normal()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, "value");
            var result = listItem.ToString();
            Assert.Equal($"{{[{keyHashCode}]: \"value\"}}", result);
        }

        [Fact]
        public void ComplexValue()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var value = new ComplexItem();
            var listItem = new ArrayItemEntity(expression, key, value);
            var result = listItem.ToString();
            Assert.Equal($"{{[0].{value.GetHashCode()}}}", result);
            Assert.Equal($"{{[{keyHashCode}].{value.GetHashCode()}}}", result);
        }

        [Fact]
        public void EncloseItem()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.EncloseItem = false;
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, "value");
            var result = listItem.ToString();
            Assert.Equal($"[{keyHashCode}]: \"value\"", result);
        }

        [Fact]
        public void PropertyAndFieldSymbol_NotImpactInResult()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.PropertySymbol = "*";
            serialization.FieldSymbol = "*";
            serialization.EncloseItem = false;
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, "value");
            var result = listItem.ToString();
            Assert.Equal($"[{keyHashCode}]: \"value\"", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.FullTypeName;
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, 100);
            var result = listItem.ToString();
            Assert.Equal($"{{System.Int32.[{keyHashCode}]: 100}}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, 100);
            var result = listItem.ToString();
            Assert.Equal($"{{[{keyHashCode}]: 100}}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.TypeName;
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, 100);
            var result = listItem.ToString();
            Assert.Equal($"{{Int32.[{keyHashCode}]: 100}}", result);
        }

        [Fact]
        public void ValueNullWithTypeName_ReturnWithoutType()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.TypeName;
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, null);
            var result = listItem.ToString();
            Assert.Equal($"{{[{keyHashCode}]: null}}", result);
        }

        [Fact]
        public void ValueNull_WithoutTypeAndEnclose()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.TypeName;
            serialization.EncloseItem = false;
            var key = 0;
            var keyHashCode = key.GetHashCode();
            var listItem = new DictionaryItemEntity(expression, key, null);
            var result = listItem.ToString();
            Assert.Equal($"[{keyHashCode}]: null", result);
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
