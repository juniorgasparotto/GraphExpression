using ExpressionGraph.Serialization;
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
            var a = A.Create();
            var expression = a.AsExpression();
        }
    }
}
