using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class VertexTest
    {
        [Fact]
        public void VerifyEntityItemVertex_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(5, expression.Count);
            Assert.Equal(3, expression.Select(f=>f.Vertex).Distinct().Count());
            Assert.Equal(expression.Graph.Vertexes[0], expression[0].Vertex);
            Assert.Equal(expression.Graph.Vertexes[1], expression[1].Vertex);
            Assert.Equal(expression.Graph.Vertexes[1], expression[2].Vertex);
            Assert.Equal(expression.Graph.Vertexes[2], expression[3].Vertex);
            Assert.Equal(expression.Graph.Vertexes[1], expression[4].Vertex);
        }

        [Fact]
        public void VerifyCountVisitInSurfaceBuild_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(5, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);
            Assert.Equal(1, expression.Graph.Vertexes[0].CountVisited);
            Assert.Equal(3, expression.Graph.Vertexes[1].CountVisited);
            Assert.Equal(1, expression.Graph.Vertexes[2].CountVisited);
        }

        [Fact]
        public void VerifyCountVisitInDeepBuild_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            // "A + (B + B + D) + (B + B + D)"
            var expression = A.AsExpression(f => f.Children, true);

            Assert.Equal(7, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);
            Assert.Equal(1, expression.Graph.Vertexes[0].CountVisited);
            Assert.Equal(4, expression.Graph.Vertexes[1].CountVisited);
            Assert.Equal(2, expression.Graph.Vertexes[2].CountVisited);
        }

        [Fact]
        public void VerifyEntityInSurfaceBuild_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            // "A + (B + B + D) + (B + B + D)"
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(5, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);
            Assert.Equal(A, expression.Graph.Vertexes[0].Entity);
            Assert.Equal(B, expression.Graph.Vertexes[1].Entity);
            Assert.Equal(D, expression.Graph.Vertexes[2].Entity);
        }

        [Fact]
        public void VerifyEntityInDeepBuild_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children, true);

            Assert.Equal(7, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);
            Assert.Equal(A, expression.Graph.Vertexes[0].Entity);            
            Assert.Equal(B, expression.Graph.Vertexes[1].Entity);
            Assert.Equal(D, expression.Graph.Vertexes[2].Entity);
        }

        [Fact]
        public void VerifyOutdegreesChildrenInSurfaceBuild_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(5, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);

            Assert.Equal(2, expression.Graph.Vertexes[0].Children.Count);
            Assert.Equal(2, expression.Graph.Vertexes[0].Outdegrees);
            Assert.Equal(expression[1], expression.Graph.Vertexes[0].Children[0]);
            Assert.Equal(expression[4], expression.Graph.Vertexes[0].Children[1]);

            Assert.Equal(2, expression.Graph.Vertexes[1].Children.Count);
            Assert.Equal(2, expression.Graph.Vertexes[1].Outdegrees);
            Assert.Equal(expression[2], expression.Graph.Vertexes[1].Children[0]);
            Assert.Equal(expression[3], expression.Graph.Vertexes[1].Children[1]);

            Assert.Equal(0, expression.Graph.Vertexes[2].Children.Count);
            Assert.Equal(0, expression.Graph.Vertexes[2].Outdegrees);
        }

        [Fact]
        public void VerifyOutdegreesChildrenInDeepBuild_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            // "A + (B + B + D) + (B + B + D)"
            var expression = A.AsExpression(f => f.Children, true);

            Assert.Equal(7, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);

            Assert.Equal(2, expression.Graph.Vertexes[0].Children.Count);
            Assert.Equal(2, expression.Graph.Vertexes[0].Outdegrees);
            Assert.Equal(expression[1], expression.Graph.Vertexes[0].Children[0]);
            Assert.Equal(expression[4], expression.Graph.Vertexes[0].Children[1]);

            Assert.Equal(4, expression.Graph.Vertexes[1].Children.Count);
            Assert.Equal(4, expression.Graph.Vertexes[1].Outdegrees);
            Assert.Equal(expression[2], expression.Graph.Vertexes[1].Children[0]);
            Assert.Equal(expression[3], expression.Graph.Vertexes[1].Children[1]);
            Assert.Equal(expression[5], expression.Graph.Vertexes[1].Children[2]);
            Assert.Equal(expression[6], expression.Graph.Vertexes[1].Children[3]);

            Assert.Equal(0, expression.Graph.Vertexes[2].Children.Count);
            Assert.Equal(0, expression.Graph.Vertexes[2].Outdegrees);
        }

        [Fact]
        public void VerifyIndegreesParentsInSurfaceBuild_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;            
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(5, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);

            Assert.Equal(0, expression.Graph.Vertexes[0].Parents.Count);
            Assert.Equal(0, expression.Graph.Vertexes[0].Indegrees);

            Assert.Equal(3, expression.Graph.Vertexes[1].Parents.Count);
            Assert.Equal(3, expression.Graph.Vertexes[1].Indegrees);
            Assert.Equal(expression[0], expression.Graph.Vertexes[1].Parents[0]);
            Assert.Equal(expression[1], expression.Graph.Vertexes[1].Parents[1]);
            Assert.Equal(expression[0], expression.Graph.Vertexes[1].Parents[2]);

            Assert.Equal(1, expression.Graph.Vertexes[2].Parents.Count);
            Assert.Equal(1, expression.Graph.Vertexes[2].Indegrees);
            Assert.Equal(expression[1], expression.Graph.Vertexes[2].Parents[0]);
        }

        [Fact]
        public void VerifyIndegreesParentsInDeepBuild_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children, true);

            Assert.Equal(7, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);

            Assert.Equal(0, expression.Graph.Vertexes[0].Parents.Count);
            Assert.Equal(0, expression.Graph.Vertexes[0].Indegrees);

            Assert.Equal(4, expression.Graph.Vertexes[1].Parents.Count);
            Assert.Equal(4, expression.Graph.Vertexes[1].Indegrees);
            Assert.Equal(expression[0], expression.Graph.Vertexes[1].Parents[0]);
            Assert.Equal(expression[1], expression.Graph.Vertexes[1].Parents[1]);
            Assert.Equal(expression[0], expression.Graph.Vertexes[1].Parents[2]);
            Assert.Equal(expression[4], expression.Graph.Vertexes[1].Parents[3]);

            Assert.Equal(2, expression.Graph.Vertexes[2].Parents.Count);
            Assert.Equal(2, expression.Graph.Vertexes[2].Indegrees);
            Assert.Equal(expression[1], expression.Graph.Vertexes[2].Parents[0]);
            Assert.Equal(expression[4], expression.Graph.Vertexes[2].Parents[1]);
        }

        [Fact]
        public void VerifyIdInSameExpression_AllOk()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children);

            var vertexA = VertexContainer<HierarchicalEntity>.Vertexes.IndexOf(expression.Graph.Vertexes[0].Entity);
            var vertexB = VertexContainer<HierarchicalEntity>.Vertexes.IndexOf(expression.Graph.Vertexes[1].Entity);
            var vertexD = VertexContainer<HierarchicalEntity>.Vertexes.IndexOf(expression.Graph.Vertexes[2].Entity);
            Assert.Equal(vertexA, expression.Graph.Vertexes[0].Id);
            Assert.Equal(vertexB, expression.Graph.Vertexes[1].Id);
            Assert.Equal(vertexD, expression.Graph.Vertexes[2].Id);
        }

        [Fact]
        public void VerifyIdInDifferentExpression_AllEquals()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var D = new HierarchicalEntity("D");

            A = A + (B + B + D) + B;
            var expression1 = A.AsExpression(f => f.Children);

            var expression2 = A.AsExpression(f => f.Children);

            Assert.Equal(expression2.Graph.Vertexes[0].Id, expression2.Graph.Vertexes[0].Id);
            Assert.Equal(expression2.Graph.Vertexes[1].Id, expression2.Graph.Vertexes[1].Id);
            Assert.Equal(expression2.Graph.Vertexes[2].Id, expression2.Graph.Vertexes[2].Id);
        }

        [Fact]
        public void VerifyExpressionWithOneEntityOnly_ShouldBe_IsIsolated()
        {
            var A = new HierarchicalEntity("A");
            var expression = A.AsExpression(f => f.Children);
            Assert.True(expression.Graph.Vertexes[0].IsIsolated);
        }

        [Fact]
        public void VerifyExpressionWithTwoEntityOnly_ShouldBe_NotIsIsolated()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            A = A + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.False(expression.Graph.Vertexes[0].IsIsolated);
            Assert.False(expression.Graph.Vertexes[1].IsIsolated);
        }

        [Fact]
        public void VerifyIsSink_ShouldBe_NotIsSinkForAAndSinkForB()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            A = A + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.False(expression.Graph.Vertexes[0].IsSink);
            Assert.True(expression.Graph.Vertexes[1].IsSink);
        }

        [Fact]
        public void VerifyIsSource_ShouldBe_IsSourceForAOnly()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var C = new HierarchicalEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);
            Assert.True(expression.Graph.Vertexes[0].IsSource);
            Assert.False(expression.Graph.Vertexes[1].IsSource);
            Assert.False(expression.Graph.Vertexes[2].IsSource);
        }

        [Fact]
        public void VerifyToStringForCircularEntity()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var C = new HierarchicalEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A", expression.Graph.Vertexes[0].ToString());
            Assert.Equal("B", expression.Graph.Vertexes[1].ToString());
            Assert.Equal("C", expression.Graph.Vertexes[2].ToString());
        }
    }
}
