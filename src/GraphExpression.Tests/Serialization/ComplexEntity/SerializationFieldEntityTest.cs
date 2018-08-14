using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationFieldEntityTest
    {
        public int fieldInt = 100;
        public string fieldString = "abc \" def \"ghi\"";

        [Fact]
        public void Normal()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("{!fieldInt: 100}", result);
        }

        
        [Fact]
        public void EncloseItem()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.EncloseItem = false;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("!fieldInt: 100", result);
        }

        [Fact]
        public void FieldSymbol()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.FieldSymbol = "*";
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("{*fieldInt: 100}", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.FullTypeName;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("{!System.Int32.fieldInt: 100}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("{!fieldInt: 100}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.TypeName;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("{!Int32.fieldInt: 100}", result);
        }

        [Fact]
        public void ValueString_Normal()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("{!fieldString: \"abc \\\" def \\\"ghi\\\"\"}", result);
        }

        [Fact]
        public void ValueString_Null()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            this.fieldString = null;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("{!fieldString: null}", result);
        }

        [Fact]
        public void ValueString_Null_WithoutTypeAndEncloseAndSymbolField()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            serialization.FieldSymbol = null;
            serialization.EncloseItem = false;

            this.fieldString = null;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("fieldString: null", result);
        }

        [Fact]
        public void ValueString_Null_Parent_WithoutTypeAndEncloseAndSymbolField()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ShowType = SerializationAsComplexExpression.ShowTypeOptions.None;
            serialization.FieldSymbol = null;
            serialization.EncloseItem = false;

            this.fieldString = null;
            var field = new FieldEntity(expression, null, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("fieldString: null", result);
        }

        [Fact]
        public void ValueString_TruncateValue()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ValueFormatter = new TruncateFormatter(3);
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("{!fieldString: \"abc\"}", result);
        }

        [Fact]
        public void ValueString_TruncateValue2()
        {
            var expression = GetExpression();
            var serialization = GetSerialization(expression);
            serialization.ValueFormatter = new TruncateFormatter(3);
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("{!fieldString: \"abc\"}", result);
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
