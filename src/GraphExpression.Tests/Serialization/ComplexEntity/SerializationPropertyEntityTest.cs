
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Serialization
{
    public class SerializationPropertyEntityTest
    {
        public int PropInt { get; set; } = 100;
        public string PropString { get; set; } = "abc \" def \"ghi\"";

        [Fact]
        public void Normal()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("@PropInt: 100", result);
        }

        [Fact]
        public void PropertySymbol()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ItemsSerialize.OfType<PropertySerialize>().First().Symbol = "*";
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("*PropInt: 100", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.FullTypeName;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("@System.Int32.PropInt: 100", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("@PropInt: 100", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropInt"));
            var result = prop1Complex.ToString();
            Assert.Equal("@Int32.PropInt: 100", result);
        }

        [Fact]
        public void ValueString_Normal()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("@PropString: \"abc \\\" def \\\"ghi\\\"\"", result);
        }

        [Fact]
        public void ValueString_Null()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            this.PropString = null;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("@PropString: null", result);
        }

        [Fact]
        public void ValueString_Null_WithoutTypeAndSymbolProperty()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            serialization.ItemsSerialize.OfType<PropertySerialize>().First().Symbol = null;

            this.PropString = null;
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("PropString: null", result);
        }

        [Fact]
        public void ValueString_Null_Parent_WithoutTypeAndSymbolProperty()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            serialization.ItemsSerialize.OfType<PropertySerialize>().First().Symbol = null;

            this.PropString = null;
            var prop1Complex = new PropertyEntity(expression, null, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("PropString: null", result);
        }

        [Fact]
        public void ValueString_TruncateValue()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ValueFormatter = new TruncateFormatter(3);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("@PropString: \"abc\"", result);
        }

        [Fact]
        public void ValueString_TruncateValue2()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ValueFormatter = new TruncateFormatter(3);
            var prop1Complex = new PropertyEntity(expression, this, GetPropertyByName("PropString"));
            var result = prop1Complex.ToString();
            Assert.Equal("@PropString: \"abc\"", result);
        }

        private System.Reflection.PropertyInfo GetPropertyByName(string name)
        {
            return this.GetType().GetProperties().Where(p => p.Name == name).First();
        }
    }
}
