using GraphExpression.Serialization;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Serialization
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
            Assert.Equal($"field.{field.GetHashCode()}", result);
        }

        [Fact]
        public void Field()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"field.{field.GetHashCode()}", result);
        }

        [Fact]
        public void ShowTypeFull()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.FullTypeName;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"GraphExpression.Tests.Serialization.SerializationValueComplexTest+ComplexItem.field.{field.GetHashCode()}", result);
        }

        [Fact]
        public void ShowTypeNone()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"field.{field.GetHashCode()}", result);
        }

        [Fact]
        public void ShowTypeOnlyName()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal($"ComplexItem.field.{field.GetHashCode()}", result);
        }

        [Fact]
        public void Value_Null()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            this.field = null;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal("field", result);
        }

        [Fact]
        public void Value_Null_WithoutType()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;

            this.field = null;
            var fieldEntity = new FieldEntity(expression, this, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal("field", result);
        }

        [Fact]
        public void Value_Null_Parent_WithoutType()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.None;

            this.field = null;
            var fieldEntity = new FieldEntity(expression, null, GetFieldByName("field"));
            var result = fieldEntity.ToString();
            Assert.Equal("field", result);
        }

        private System.Reflection.FieldInfo GetFieldByName(string name)
        {
            return this.GetType().GetFields().Where(p => p.Name == name).First();
        }
    }
}
