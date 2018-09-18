using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GraphExpression.Serialization;

namespace GraphExpression.Tests.Serialization.Common
{
    public class ExpressionSerializerBaseTest
    {
        [Fact]
        public void CreateExpressionCircularDefaultEnclosed_ShouldNotEnclosed()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");

            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + B", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionCircularEnclosedTrue_ShouldEncloseEachEntity()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");

            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            var serializer = expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
            serializer.EncloseItem = true;
            Assert.Equal(2, expression.Count);
            Assert.Equal("{A} + {B}", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionComplexDefaultEnclosed_ShouldEnclosed()
        {
            var A = new
            {
                P = 1
            };

            var expression = A.AsExpression();
            Assert.Equal($"{{{A.GetHashCode()}}} + {{@P: 1}}", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionComplexEnclosedFALSE_ShouldNotEnclosed()
        {
            var A = new
            {
                P = 1
            };

            var expression = A.AsExpression();
            var serializer = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            serializer.EncloseItem = false;
            Assert.Equal($"{A.GetHashCode()} + @P: 1", expression.DefaultSerializer.Serialize());
        }
    }
}
