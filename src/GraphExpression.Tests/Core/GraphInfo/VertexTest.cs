using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Core
{
    public class VertexTest
    {
        [Fact]
        public void VerifyEntityItemVertex_AllOk()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

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
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

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
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

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
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

            A = A + (B + B + D) + B;
            // "A + (B + B + D) + (B + B + D)"
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(5, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);
            Assert.Same(A, expression.Graph.Vertexes[0].Entity);
            Assert.Same(B, expression.Graph.Vertexes[1].Entity);
            Assert.Same(D, expression.Graph.Vertexes[2].Entity);
        }

        [Fact]
        public void VerifyEntityInDeepBuild_AllOk()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children, true);

            Assert.Equal(7, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);
            Assert.Same(A, expression.Graph.Vertexes[0].Entity);            
            Assert.Same(B, expression.Graph.Vertexes[1].Entity);
            Assert.Same(D, expression.Graph.Vertexes[2].Entity);
        }

        [Fact]
        public void VerifyOutdegreesChildrenInSurfaceBuild_AllOk()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(5, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);

            Assert.Equal(2, expression.Graph.Vertexes[0].Children.Count);
            Assert.Equal(2, expression.Graph.Vertexes[0].Outdegrees);
            Assert.Same(expression[1], expression.Graph.Vertexes[0].Children[0]);
            Assert.Same(expression[4], expression.Graph.Vertexes[0].Children[1]);

            Assert.Equal(2, expression.Graph.Vertexes[1].Children.Count);
            Assert.Equal(2, expression.Graph.Vertexes[1].Outdegrees);
            Assert.Same(expression[2], expression.Graph.Vertexes[1].Children[0]);
            Assert.Same(expression[3], expression.Graph.Vertexes[1].Children[1]);

            Assert.Equal(0, expression.Graph.Vertexes[2].Children.Count);
            Assert.Equal(0, expression.Graph.Vertexes[2].Outdegrees);
        }

        [Fact]
        public void VerifyOutdegreesChildrenInDeepBuild_AllOk()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

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
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

            A = A + (B + B + D) + B;            
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(5, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);

            Assert.Equal(0, expression.Graph.Vertexes[0].Parents.Count);
            Assert.Equal(0, expression.Graph.Vertexes[0].Indegrees);

            Assert.Equal(3, expression.Graph.Vertexes[1].Parents.Count);
            Assert.Equal(3, expression.Graph.Vertexes[1].Indegrees);
            Assert.Same(expression[0], expression.Graph.Vertexes[1].Parents[0]);
            Assert.Same(expression[1], expression.Graph.Vertexes[1].Parents[1]);
            Assert.Same(expression[0], expression.Graph.Vertexes[1].Parents[2]);

            Assert.Equal(1, expression.Graph.Vertexes[2].Parents.Count);
            Assert.Equal(1, expression.Graph.Vertexes[2].Indegrees);
            Assert.Same(expression[1], expression.Graph.Vertexes[2].Parents[0]);
        }

        [Fact]
        public void VerifyIndegreesParentsInDeepBuild_AllOk()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children, true);

            Assert.Equal(7, expression.Count);
            Assert.Equal(3, expression.Graph.Vertexes.Count);

            Assert.Equal(0, expression.Graph.Vertexes[0].Parents.Count);
            Assert.Equal(0, expression.Graph.Vertexes[0].Indegrees);

            Assert.Equal(4, expression.Graph.Vertexes[1].Parents.Count);
            Assert.Equal(4, expression.Graph.Vertexes[1].Indegrees);
            Assert.Same(expression[0], expression.Graph.Vertexes[1].Parents[0]);
            Assert.Same(expression[1], expression.Graph.Vertexes[1].Parents[1]);
            Assert.Same(expression[0], expression.Graph.Vertexes[1].Parents[2]);
            Assert.Same(expression[4], expression.Graph.Vertexes[1].Parents[3]);

            Assert.Equal(2, expression.Graph.Vertexes[2].Parents.Count);
            Assert.Equal(2, expression.Graph.Vertexes[2].Indegrees);
            Assert.Same(expression[1], expression.Graph.Vertexes[2].Parents[0]);
            Assert.Same(expression[4], expression.Graph.Vertexes[2].Parents[1]);
        }

        [Fact]
        public void VerifyIdInSameExpression_AllOk()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

            A = A + (B + B + D) + B;
            var expression = A.AsExpression(f => f.Children);

            var vertexA = VertexContainer<CircularEntity>.GetEntityId(expression.Graph.Vertexes[0].Entity).Id;
            var vertexB = VertexContainer<CircularEntity>.GetEntityId(expression.Graph.Vertexes[1].Entity).Id;
            var vertexD = VertexContainer<CircularEntity>.GetEntityId(expression.Graph.Vertexes[2].Entity).Id;
            Assert.Equal(vertexA, expression.Graph.Vertexes[0].Id);
            Assert.Equal(vertexB, expression.Graph.Vertexes[1].Id);
            Assert.Equal(vertexD, expression.Graph.Vertexes[2].Id);
        }

        [Fact]
        public void VerifyIdInDifferentExpression_AllEquals()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var D = new CircularEntity("D");

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
            var A = new CircularEntity("A");
            var expression = A.AsExpression(f => f.Children);
            Assert.True(expression.Graph.Vertexes[0].IsIsolated);
        }

        [Fact]
        public void VerifyExpressionWithTwoEntityOnly_ShouldBe_NotIsIsolated()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            A = A + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.False(expression.Graph.Vertexes[0].IsIsolated);
            Assert.False(expression.Graph.Vertexes[1].IsIsolated);
        }

        [Fact]
        public void VerifyIsSink_ShouldBe_NotIsSinkForAAndSinkForB()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            A = A + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.False(expression.Graph.Vertexes[0].IsSink);
            Assert.True(expression.Graph.Vertexes[1].IsSink);
        }

        [Fact]
        public void VerifyIsSource_ShouldBe_IsSourceForAOnly()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);
            Assert.True(expression.Graph.Vertexes[0].IsSource);
            Assert.False(expression.Graph.Vertexes[1].IsSource);
            Assert.False(expression.Graph.Vertexes[2].IsSource);
        }

        [Fact]
        public void VerifyToString_ReturnEntityToString()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A", expression.Graph.Vertexes[0].ToString());
            Assert.Equal("B", expression.Graph.Vertexes[1].ToString());
            Assert.Equal("C", expression.Graph.Vertexes[2].ToString());
        }

        [Fact]
        public void VerifyAllVertex_AllOk()
        {
            var SPL = new CircularEntity("SPL");
            var COR = new CircularEntity("COR");
            var PAL = new CircularEntity("PAL");
            var SAN = new CircularEntity("SAN");

            SPL = SPL + (COR + PAL) + (SAN + PAL + COR);

            var graph = SPL.AsExpression(f => f.Children, null, true);
            var output = graph.DefaultSerializer.Serialize();
            Assert.Equal("SPL + (COR + PAL) + (SAN + PAL + (COR + PAL))", output);
            var spl = graph.Graph.Vertexes.ElementAt(0);

            // number of visit in vertex
            Assert.Equal(1, spl.CountVisited);

            // verify if is root (source)
            Assert.True(spl.IsSource);

            // verify if is sink (leaf - without children)
            Assert.False(spl.IsSink);

            // verify if is isolated (source and sink in the same time)
            Assert.False(spl.IsIsolated);

            // verify all parents
            Assert.Empty(spl.Parents);

            // verify all children
            Assert.Equal(2, spl.Children.Count());
            Assert.Equal("COR", spl.Children.ElementAt(0).ToString());
            Assert.Equal("SAN", spl.Children.ElementAt(1).ToString());

            var cor = graph.Graph.Vertexes.ElementAt(1);

            // number of visit in vertex
            Assert.True(cor.CountVisited == 2);

            // verify if is root (source)
            Assert.True(cor.IsSource == false);

            // verify if is sink (leaf - without children)
            Assert.True(cor.IsSink == false);

            // verify if is isolated (source and sink in the same time)
            Assert.True(cor.IsIsolated == false);

            // verify all parents
            Assert.True(cor.Parents.Count() == 2);
            Assert.True(cor.Parents.ElementAt(0).ToString() == "SPL");
            Assert.True(cor.Parents.ElementAt(1).ToString() == "SAN");

            // verify all children
            Assert.True(cor.Children.Count() == 2);
            Assert.True(cor.Children.ElementAt(0).ToString() == "PAL");
            Assert.True(cor.Children.ElementAt(0).Index == 2);
            Assert.True(cor.Children.ElementAt(1).ToString() == "PAL");
            Assert.True(cor.Children.ElementAt(1).Index == 6);

            var pal = graph.Graph.Vertexes.ElementAt(2);

            // number of visit in vertex
            Assert.Equal(3, pal.CountVisited);

            // verify if is root (source)
            Assert.False(pal.IsSource);

            // verify if is sink (leaf - without children)
            Assert.True(pal.IsSink);

            // verify if is isolated (source and sink in the same time)
            Assert.False(pal.IsIsolated);

            // verify all parents
            Assert.Equal(3, pal.Parents.Count());

            Assert.Equal("COR", pal.Parents[0].ToString());
            Assert.Equal(1, pal.Parents[0].Index);

            Assert.Equal("SAN", pal.Parents[1].ToString());
            Assert.Equal(3, pal.Parents[1].Index);

            Assert.Equal("COR", pal.Parents[2].ToString());
            Assert.Equal(5, pal.Parents[2].Index);

            // verify all children
            Assert.Empty(pal.Children);

            var san = graph.Graph.Vertexes.ElementAt(3);

            // number of visit in vertex
            Assert.Equal(1, san.CountVisited);

            // verify if is root (source)
            Assert.False(san.IsSource);

            // verify if is sink (leaf - without children)
            Assert.False(san.IsSink);

            // verify if is isolated (source and sink in the same time)
            Assert.False(san.IsIsolated);

            // verify all parents
            Assert.Single(san.Parents);
            Assert.Equal("SPL", san.Parents.ElementAt(0).ToString());

            // verify all children
            Assert.Equal(2, san.Children.Count());
            Assert.Equal("PAL", san.Children.ElementAt(0).ToString());
            Assert.Equal("COR", san.Children.ElementAt(1).ToString());
        }
    }
}
