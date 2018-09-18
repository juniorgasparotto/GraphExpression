using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class PathTest
    {
        [Fact]
        public void VerifyExpressionPaths_ShouldBeProgressivePath()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");
            A = A + (B + (C + D));
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(4, expression.Count);
            Assert.Equal("[A]", expression[0].Path.ToString());
            Assert.Equal("[A].[B]", expression[1].Path.ToString());
            Assert.Equal("[A].[B].[C]", expression[2].Path.ToString());
            Assert.Equal("[A].[B].[C].[D]", expression[3].Path.ToString());
        }

        [Fact]
        public void VerifyLastPaths_ShouldHasOnlyLastPaths()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");
            A = A + (B + C) + D;
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(4, expression.Count);
            Assert.Equal(2, expression.Graph.Paths.Count);
            Assert.Equal("[A].[B].[C]", expression.Graph.Paths[0].ToString());
            Assert.Equal("[A].[D]", expression.Graph.Paths[1].ToString());
        }
        
        [Fact]
        public void VerifyIdentity_ShouldEqualsVertexContainer()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);

            var vertexA = VertexContainer<CircularEntity>.GetEntityId(A).Id;
            var vertexB = VertexContainer<CircularEntity>.GetEntityId(B).Id;
            var vertexC = VertexContainer<CircularEntity>.GetEntityId(C).Id;

            Assert.Equal(3, expression.Count);
            Assert.Equal($"[{vertexA}]", expression[0].Path.Identity);
            Assert.Equal($"[{vertexA}].[{vertexB}]", expression[1].Path.Identity);
            Assert.Equal($"[{vertexA}].[{vertexB}].[{vertexC}]", expression[2].Path.Identity);
        }

        [Fact]
        public void VerifyPathItems_ShouldEqualsInExpression()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(3, expression.Count);
            Assert.Single(expression[0].Path.Items);
            Assert.Same(expression[0], expression[0].Path.Items.ElementAt(0));

            Assert.Equal(2, expression[1].Path.Items.Count());
            Assert.Same(expression[0], expression[1].Path.Items.ElementAt(0));
            Assert.Same(expression[1], expression[1].Path.Items.ElementAt(1));

            Assert.Equal(3, expression[2].Path.Items.Count());
            Assert.Same(expression[0], expression[2].Path.Items.ElementAt(0));
            Assert.Same(expression[1], expression[2].Path.Items.ElementAt(1));
            Assert.Same(expression[2], expression[2].Path.Items.ElementAt(2));
        }

        [Fact]
        public void VerifyPathType_AllIsSimple()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(3, expression.Count);
            Assert.Equal(PathType.Simple, expression[0].Path.PathType);
            Assert.Equal(PathType.Simple, expression[1].Path.PathType);
            Assert.Equal(PathType.Simple, expression[2].Path.PathType);
        }

        [Fact]
        public void VerifyPathType_RepeatEntityInLastPath_ReturnCircuitForEntityB()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + A);
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(3, expression.Count);
            Assert.Equal(PathType.Simple, expression[0].Path.PathType);
            Assert.Equal(PathType.Simple, expression[1].Path.PathType);
            Assert.Equal(PathType.Circuit, expression[2].Path.PathType);
        }

        [Fact]
        public void VerifyPathType_RepeatCircularEntityB_ReturnCircleForEntityB()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + B);
            var expression = A.AsExpression(f => f.Children);

            Assert.Equal(3, expression.Count);
            Assert.Equal(PathType.Simple, expression[0].Path.PathType);
            Assert.Equal(PathType.Simple, expression[1].Path.PathType);
            Assert.Equal(PathType.Circle, expression[2].Path.PathType);
        }

        [Fact]
        public void VerifyContainGraph()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");
            var E = new CircularEntity("E");
            var F = new CircularEntity("F");

            A = A + (B + (C + D)) + (F + C);

            // A + (B + (C + D)) + (F + (C + D)) 
            // Deep (repeat C in final)
            var expressionA = A.AsExpression(f => f.Children, true);
            var expressionC = C.AsExpression(f => f.Children);

            Assert.Equal(7, expressionA.Count);
            Assert.Equal(2, expressionC.Count);

            // A.B.C.D contains C.D
            Assert.True(expressionA[3].Path.ContainsPath(expressionC[1].Path));
            Assert.True(expressionA[6].Path.ContainsPath(expressionC[1].Path));

            // check if all paths in graphC exists in graphA
            Assert.True(expressionA.Graph.ContainsGraph(expressionC.Graph));
        }

        [Fact]
        public void VerifyToStringInComplexEntity_ReturnEdgeToString()
        {
            var A = new
            {
                Prop1 = 10
            };

            var expression = A.AsExpression();
            Assert.Equal($"[{A.GetHashCode()}]", expression[0].Path.ToString());
            Assert.Equal($"[{A.GetHashCode()}].[@Prop1: 10]", expression[1].Path.ToString());
        }

        [Fact]
        public void VerifyIfGraphContainAnotherGraph_MustContainsGraph()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");
            var I = new CircularEntity("I");
            var J = new CircularEntity("J");

            A = A + (B + C + D) + A + (I + (J + C) + A);

            var vertexesSources = new CircularEntity[] { A, B, C, D, I, J };
            var graphs = new List<Graph<CircularEntity>>();

            foreach (var v in vertexesSources)
            {
                var expression = v.AsExpression(f => f.Children);
                graphs.Add(expression.Graph);
            }

            var graph = graphs.ElementAt(0);
            var graph1 = graphs.ElementAt(1);
            var graph2 = graphs.ElementAt(2);
            var graph3 = graphs.ElementAt(3);
            var graph4 = graphs.ElementAt(4);
            var graph5 = graphs.ElementAt(5);

            Assert.Equal(9, graph.Edges.Count());
            Assert.Equal(", A", graph.Edges.ElementAt(0).ToString());
            Assert.Equal("A, B", graph.Edges.ElementAt(1).ToString());
            Assert.Equal("B, C", graph.Edges.ElementAt(2).ToString());
            Assert.Equal("B, D", graph.Edges.ElementAt(3).ToString());
            Assert.Equal("A, A", graph.Edges.ElementAt(4).ToString());
            Assert.Equal("A, I", graph.Edges.ElementAt(5).ToString());
            Assert.Equal("I, J", graph.Edges.ElementAt(6).ToString());
            Assert.Equal("J, C", graph.Edges.ElementAt(7).ToString());
            Assert.Equal("I, A", graph.Edges.ElementAt(8).ToString());
            Assert.Equal(6, graph.Vertexes.Count());
            Assert.Equal("A", graph.Vertexes.ElementAt(0).ToString());
            Assert.Equal("B", graph.Vertexes.ElementAt(1).ToString());
            Assert.Equal("C", graph.Vertexes.ElementAt(2).ToString());
            Assert.Equal("D", graph.Vertexes.ElementAt(3).ToString());
            Assert.Equal("I", graph.Vertexes.ElementAt(4).ToString());
            Assert.Equal("J", graph.Vertexes.ElementAt(5).ToString());
            Assert.Equal(5, graph.Paths.Count());
            Assert.Equal("[A].[B].[C]", graph.Paths.ElementAt(0).ToString());
            Assert.Equal("[A].[B].[D]", graph.Paths.ElementAt(1).ToString());
            Assert.Equal("[A].[A]", graph.Paths.ElementAt(2).ToString());
            Assert.Equal("[A].[I].[J].[C]", graph.Paths.ElementAt(3).ToString());
            Assert.Equal("[A].[I].[A]", graph.Paths.ElementAt(4).ToString());
            Assert.True(graph.ContainsGraph(graph1));
            Assert.True(graph.ContainsGraph(graph2));
            Assert.True(graph.ContainsGraph(graph3));
            Assert.False(graph.ContainsGraph(graph4));
            Assert.True(graph.ContainsGraph(graph5));
            
            var graphsClean = graphs.RemoveCoexistents();
            var paths = graphsClean.ToPaths();
            var pathsClean = graphsClean.ToPaths().RemoveCoexistents();

            Assert.Equal(2, graphsClean.Count());
            Assert.Equal("[A].[B].[C]", paths.ElementAt(0).ToString());
            Assert.Equal("[A].[B].[D]", paths.ElementAt(1).ToString());
            Assert.Equal("[A].[A]", paths.ElementAt(2).ToString());
            Assert.Equal("[A].[I].[J].[C]", paths.ElementAt(3).ToString());
            Assert.Equal("[A].[I].[A]", paths.ElementAt(4).ToString());
            Assert.Equal("[I].[J].[C]", paths.ElementAt(5).ToString());
            Assert.Equal("[I].[A].[B].[C]", paths.ElementAt(6).ToString());
            Assert.Equal("[I].[A].[B].[D]", paths.ElementAt(7).ToString());
            Assert.Equal("[I].[A].[A]", paths.ElementAt(8).ToString());
            Assert.Equal("[I].[A].[I]", paths.ElementAt(9).ToString());
            Assert.Equal(this.GetOutputPaths(pathsClean), this.GetOutputPaths(paths.RemoveCoexistents()));
            Assert.Equal("[A].[I].[J].[C]", pathsClean.ElementAt(0).ToString());
            Assert.Equal("[A].[I].[A]", pathsClean.ElementAt(1).ToString());
            Assert.Equal("[I].[A].[B].[C]", pathsClean.ElementAt(2).ToString());
            Assert.Equal("[I].[A].[B].[D]", pathsClean.ElementAt(3).ToString());
            Assert.Equal("[I].[A].[A]", pathsClean.ElementAt(4).ToString());
            Assert.Equal("[I].[A].[I]", pathsClean.ElementAt(5).ToString());
        }

        [Fact]
        public void RemoveCoexistents_CheckPathsAfterRemove_AllOk()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");
            var E = new CircularEntity("E");
            var F = new CircularEntity("F");
            var G = new CircularEntity("G");
            var H = new CircularEntity("H");
            var J = new CircularEntity("J");

            A = A + (B + C + D) + D + (E + B) + F + (G + G + C) + (H + C);

            var vertexesSources = new CircularEntity[] { A, B, C, D, E, F, G, H };
            var graphs = new List<Graph<CircularEntity>>();

            foreach (var v in vertexesSources)
            {
                var expression = v.AsExpression(f => f.Children, true);
                graphs.Add(expression.Graph);
            }

            var graphsClean = graphs.RemoveCoexistents();

            var output = this.GetOutputPaths(graphs.ToPaths());
            var outputClean = this.GetOutputPaths(graphs.ToPaths().RemoveCoexistents());

            var expected = "[A].[B].[C]\r\n[A].[B].[D]\r\n[A].[D]\r\n[A].[E].[B].[C]\r\n[A].[E].[B].[D]\r\n[A].[F]\r\n[A].[G].[G]\r\n[A].[G].[C]\r\n[A].[H].[C]\r\n[B].[C]\r\n[B].[D]\r\n[C]\r\n[D]\r\n[E].[B].[C]\r\n[E].[B].[D]\r\n[F]\r\n[G].[G]\r\n[G].[C]\r\n[H].[C]";
            var expectedClean = "[A].[B].[C]\r\n[A].[B].[D]\r\n[A].[D]\r\n[A].[E].[B].[C]\r\n[A].[E].[B].[D]\r\n[A].[F]\r\n[A].[G].[G]\r\n[A].[G].[C]\r\n[A].[H].[C]";

            Assert.Equal(output, expected);
            Assert.Equal(outputClean, expectedClean);
        }

        //[Fact]
        //public void VerifyToString()
        //{
        //    var A = new HierarchicalEntity("A");
        //    var B = new HierarchicalEntity("B");
        //    var C = new HierarchicalEntity("C");

        //    A = A + (B + C);
        //    var expression = A.AsExpression();

        //    Assert.Equal("", expression[2].Path.ToString());
        //}

        private string GetOutputPaths(IEnumerable<Path<CircularEntity>> paths)
        {
            var output = "";
            foreach (var path in paths)
            {
                output += path.ToString();
                output += "\r\n";
            }

            return output.Trim();
        }
    }
}
