using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class ExpressionFromComplexEntityTest : EntitiesData
    {
        [Fact]
        public void CreateManualExpression_Surface_ReturnExpressionAsString()
        {
            var expression = new Expression<Entity>();
            
        }
    }
}
