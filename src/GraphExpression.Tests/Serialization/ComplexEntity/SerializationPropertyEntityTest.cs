using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationPropertyEntityTest
    {
        public int PropInt { get; set; } = 100;
        public string PropString { get; set; } = "abc \" def \"ghi\"";

        [Fact]
        public void Normal()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("{@PropInt: 100}", result);
        }

        [Fact]
        public void EncloseItem()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.EncloseItem = false;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("@PropInt: 100", result);
        }

        [Fact]
        public void PropertySymbol()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ItemsSerialize.OfType<PropertySerialize>().First().Symbol = "*";
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("{*PropInt: 100}", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.FullTypeName;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("{@System.Int32.PropInt: 100}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("{@PropInt: 100}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.TypeName;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("{@Int32.PropInt: 100}", result);
        }

        [Fact]
        public void ValueString_Normal()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("{@PropString: \"abc \\\" def \\\"ghi\\\"\"}", result);
        }

        [Fact]
        public void ValueString_Null()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            this.PropString = null;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("{@PropString: null}", result);
        }

        [Fact]
        public void ValueString_Null_WithoutTypeAndEncloseAndSymbolProperty()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            serialization.ItemsSerialize.OfType<PropertySerialize>().First().Symbol = null;
            serialization.EncloseItem = false;

            this.PropString = null;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("PropString: null", result);
        }

        [Fact]
        public void ValueString_Null_Parent_WithoutTypeAndEncloseAndSymbolProperty()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            serialization.ItemsSerialize.OfType<PropertySerialize>().First().Symbol = null;
            serialization.EncloseItem = false;

            this.PropString = null;
            var prop1Complex = new PropertyEntity(expression, null, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("PropString: null", result);
        }

        [Fact]
        public void ValueString_TruncateValue()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ValueFormatter = new TruncateFormatter(3);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("{@PropString: \"abc\"}", result);
        }

        [Fact]
        public void ValueString_TruncateValue2()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ValueFormatter = new TruncateFormatter(3);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("{@PropString: \"abc\"}", result);
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
