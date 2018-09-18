
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
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("!fieldInt: 100", result);
        }
        
        [Fact]
        public void FieldSymbol()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ItemsSerialize.OfType<FieldSerialize>().First().Symbol = "*";
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("*fieldInt: 100", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.FullTypeName;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("!System.Int32.fieldInt: 100", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("!fieldInt: 100", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldInt"));
            var result = field.ToString();
            Assert.Equal("!Int32.fieldInt: 100", result);
        }

        [Fact]
        public void ValueString_Normal()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("!fieldString: \"abc \\\" def \\\"ghi\\\"\"", result);
        }

        [Fact]
        public void ValueString_Null()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            this.fieldString = null;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("!fieldString: null", result);
        }

        [Fact]
        public void ValueString_Null_WithoutTypeAndSymbolField()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            serialization.ItemsSerialize.OfType<FieldSerialize>().First().Symbol = null;

            this.fieldString = null;
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("fieldString: null", result);
        }

        [Fact]
        public void ValueString_Null_Parent_WithoutTypeAndSymbolField()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            serialization.ItemsSerialize.OfType<FieldSerialize>().First().Symbol = null;

            this.fieldString = null;
            var field = new FieldEntity(expression, null, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("fieldString: null", result);
        }

        [Fact]
        public void ValueString_TruncateValue()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ValueFormatter = new TruncateFormatter(3);
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("!fieldString: \"abc\"", result);
        }

        [Fact]
        public void ValueString_TruncateValue2()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ValueFormatter = new TruncateFormatter(3);
            var field = new FieldEntity(expression, this, GetFieldByName("fieldString"));
            var result = field.ToString();
            Assert.Equal("!fieldString: \"abc\"", result);
        }

        private System.Reflection.FieldInfo GetFieldByName(string name)
        {
            return this.GetType().GetFields().Where(p => p.Name == name).First();
        }
    }
}
