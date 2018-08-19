using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class AnonimousTest
    {
        [Fact]
        public void CreateAnonimous_ReturnExpressionAsString()
        {
            var anonimous = new
            {
                Prop1 = 124m,
                Prop2 = (int?)null
            };

            var expression = anonimous.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{anonimous.GetHashCode()}}} + {{@Prop1: 124}} + {{@Prop2: null}}";
            Assert.Equal(3, expression.Count);
            Assert.Equal(expected, result);
        }
    }
}
