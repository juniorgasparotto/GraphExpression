using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationValueComplexTest
    {
        public class ComplexItem
        {
            public int fieldInt = 100;
        }

        public ComplexItem field = new ComplexItem
        {
            fieldInt = 10,
        };

        [Fact]
        public void Normal()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"{{!field.{field.GetHashCode()}}}", result);
        }
        
        [Fact]
        public void EncloseItem()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.EncloseItem = false;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"!field.{field.GetHashCode()}", result);
        }

        [Fact]
        public void FieldSymbol()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ItemsSerialize.OfType<FieldSerialize>().First().Symbol = "*";
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"{{*field.{field.GetHashCode()}}}", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.FullTypeName;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"{{!GraphExpression.Tests.SerializationValueComplexTest+ComplexItem.field.{field.GetHashCode()}}}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"{{!field.{field.GetHashCode()}}}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"{{!ComplexItem.field.{field.GetHashCode()}}}", result);
        }

        [Fact]
        public void Value_Null()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            this.field = null;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal("{!field: null}", result);
        }

        [Fact]
        public void Value_Null_WithoutTypeAndEncloseAndSymbolField()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            serialization.ItemsSerialize.OfType<FieldSerialize>().First().Symbol = null;
            serialization.EncloseItem = false;

            this.field = null;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal("field: null", result);
        }

        [Fact]
        public void Value_Null_Parent_WithoutTypeAndEncloseAndSymbolField()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            serialization.ItemsSerialize.OfType<FieldSerialize>().First().Symbol = null;
            serialization.EncloseItem = false;

            this.field = null;
            var fieldEntity = new FieldEntity(expression, null, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal("field: null", result);
        }

        private System.Reflection.FieldInfo GetFieldByName(string name)
        {
            return this.GetType().GetFields().Where(p => p.Name == name).First();
        }
    }
}
