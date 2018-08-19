using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class DynamicTest
    {
        [Fact]
        public void CreateDynamicWithEmptyEntity_ReturnExpressionAsString()
        {
            dynamic dyn = new ExpandoObject();

            dyn.A = 123;
            dyn.B = new ExpandoObject();
            dyn.B.C = "abc";

            var expression = ((object)dyn).AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{dyn.GetHashCode()}}} + {{@A: 123}} + ({{@B.{dyn.B.GetHashCode()}}} + {{@C: \"abc\"}})";
            Assert.Equal(4, expression.Count);
            Assert.Equal(expected, result);
        }
    }
}
