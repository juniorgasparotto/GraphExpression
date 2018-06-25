using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class ExpressionWithHierarchyEntityTest : EntitiesData
    {
        [Fact]
        public void CreateManualExpression_Surface_ReturnExpressionAsString()
        {
            var expression = new Expression();
            expression.Add(new EntityItem(expression) { Entity = A, Index = 0, IndexAtLevel = 0, Level = 1 });
            expression.Add(new EntityItem(expression) { Entity = B, Index = 1, IndexAtLevel = 0, Level = 2 });
            expression.Add(new EntityItem(expression) { Entity = C, Index = 2, IndexAtLevel = 1, Level = 2 });
            expression.Add(new EntityItem(expression) { Entity = Y, Index = 3, IndexAtLevel = 0, Level = 3 });
            expression.Add(new EntityItem(expression) { Entity = D, Index = 4, IndexAtLevel = 2, Level = 2 });
            expression.Add(new EntityItem(expression) { Entity = E, Index = 5, IndexAtLevel = 0, Level = 3 });
            expression.Add(new EntityItem(expression) { Entity = F, Index = 6, IndexAtLevel = 1, Level = 3 });
            expression.Add(new EntityItem(expression) { Entity = G, Index = 7, IndexAtLevel = 0, Level = 4 });
            expression.Add(new EntityItem(expression) { Entity = B, Index = 8, IndexAtLevel = 0, Level = 5 });
            expression.Add(new EntityItem(expression) { Entity = C, Index = 9, IndexAtLevel = 1, Level = 5 });
            expression.Add(new EntityItem(expression) { Entity = Y, Index = 10, IndexAtLevel = 1, Level = 4 });
            expression.Add(new EntityItem(expression) { Entity = Z, Index = 11, IndexAtLevel = 2, Level = 3 });
            var expressionString = expression.ToExpressionAsString();
            Assert.Equal("(A + B + (C + Y) + (D + E + (F + (G + B + C) + Y) + Z))", expressionString);
        }
    }
}
