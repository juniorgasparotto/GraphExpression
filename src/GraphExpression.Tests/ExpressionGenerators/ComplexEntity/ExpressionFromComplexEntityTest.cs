using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class ExpressionFromComplexEntityTest
    {
        [Fact]
        public void CreateManualExpression_Surface_ReturnExpressionAsString()
        {
            var b = new CircularEntity("A");
            b.Add(new CircularEntity("B"));
            b.Add(new CircularEntity("C"));
            var expression = b.AsExpression(f => f).DefaultSerializer.Serialize();

            //var a = A.Create();
            //var expression = a.AsExpression();
            //var expressionStr = expression.DefaultSerializer.Serialize();
        }
    }
}
