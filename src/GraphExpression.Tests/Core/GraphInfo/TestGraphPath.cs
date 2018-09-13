using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class TestGraphPath
    {
        [Fact]
        public void TestPathStringAndContainsGraph()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var C = new HierarchicalEntity("C");
            var D = new HierarchicalEntity("D");
            var I = new HierarchicalEntity("I");
            var J = new HierarchicalEntity("J");

            A = A + (B + C + D) + A + (I + (J + C) + A);

            var vertexesSources = new HierarchicalEntity[] { A, B, C, D, I, J };
            var graphs = new List<GraphInfo<HierarchicalEntity>>();

            foreach (var v in vertexesSources)
            {
                var expression = v.AsExpression(f => f.Children);
                graphs.Add(expression.GraphInfo);
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
            Assert.Equal("[A].[B].[C]", graph.Paths.ElementAt(0).ToString(true));
            Assert.Equal("[A].[B].[D]", graph.Paths.ElementAt(1).ToString(true));
            Assert.Equal("[A].[A]", graph.Paths.ElementAt(2).ToString(true));
            Assert.Equal("[A].[I].[J].[C]", graph.Paths.ElementAt(3).ToString(true));
            Assert.Equal("[A].[I].[A]", graph.Paths.ElementAt(4).ToString(true));
            Assert.True(graph.ContainsGraph(graph1));
            Assert.True(graph.ContainsGraph(graph2));
            Assert.True(graph.ContainsGraph(graph3));
            Assert.False(graph.ContainsGraph(graph4));
            Assert.True(graph.ContainsGraph(graph5));
            
            var graphsClean = graphs.RemoveCoexistents();
            var paths = graphsClean.ToPaths();
            var pathsClean = graphsClean.ToPaths().RemoveCoexistents();

            Assert.Equal(2, graphsClean.Count());
            Assert.Equal("[A].[B].[C]", paths.ElementAt(0).ToString(true));
            Assert.Equal("[A].[B].[D]", paths.ElementAt(1).ToString(true));
            Assert.Equal("[A].[A]", paths.ElementAt(2).ToString(true));
            Assert.Equal("[A].[I].[J].[C]", paths.ElementAt(3).ToString(true));
            Assert.Equal("[A].[I].[A]", paths.ElementAt(4).ToString(true));
            Assert.Equal("[I].[J].[C]", paths.ElementAt(5).ToString(true));
            Assert.Equal("[I].[A].[B].[C]", paths.ElementAt(6).ToString(true));
            Assert.Equal("[I].[A].[B].[D]", paths.ElementAt(7).ToString(true));
            Assert.Equal("[I].[A].[A]", paths.ElementAt(8).ToString(true));
            Assert.Equal("[I].[A].[I]", paths.ElementAt(9).ToString(true));
            Assert.Equal(this.GetOutputPaths(pathsClean), this.GetOutputPaths(paths.RemoveCoexistents()));
            Assert.Equal("[A].[I].[J].[C]", pathsClean.ElementAt(0).ToString(true));
            Assert.Equal("[A].[I].[A]", pathsClean.ElementAt(1).ToString(true));
            Assert.Equal("[I].[A].[B].[C]", pathsClean.ElementAt(2).ToString(true));
            Assert.Equal("[I].[A].[B].[D]", pathsClean.ElementAt(3).ToString(true));
            Assert.Equal("[I].[A].[A]", pathsClean.ElementAt(4).ToString(true));
            Assert.Equal("[I].[A].[I]", pathsClean.ElementAt(5).ToString(true));
        }

        [Fact]
        public void TestRemoveCoexistents()
        {
            var A = new HierarchicalEntity("A");
            var B = new HierarchicalEntity("B");
            var C = new HierarchicalEntity("C");
            var D = new HierarchicalEntity("D");
            var E = new HierarchicalEntity("E");
            var F = new HierarchicalEntity("F");
            var G = new HierarchicalEntity("G");
            var H = new HierarchicalEntity("H");
            var J = new HierarchicalEntity("J");

            A = A + (B + C + D) + D + (E + B) + F + (G + G + C) + (H + C);

            var vertexesSources = new HierarchicalEntity[] { A, B, C, D, E, F, G, H };
            var graphs = new List<GraphInfo<HierarchicalEntity>>();

            foreach (var v in vertexesSources)
            {
                var expression = v.AsExpression(f => f.Children, true);
                graphs.Add(expression.GraphInfo);
            }

            var graphsClean = graphs.RemoveCoexistents();

            var output = this.GetOutputPaths(graphs.ToPaths());
            var outputClean = this.GetOutputPaths(graphs.ToPaths().RemoveCoexistents());

            var expected = "[A].[B].[C]\r\n[A].[B].[D]\r\n[A].[D]\r\n[A].[E].[B].[C]\r\n[A].[E].[B].[D]\r\n[A].[F]\r\n[A].[G].[G]\r\n[A].[G].[C]\r\n[A].[H].[C]\r\n[B].[C]\r\n[B].[D]\r\n[C]\r\n[D]\r\n[E].[B].[C]\r\n[E].[B].[D]\r\n[F]\r\n[G].[G]\r\n[G].[C]\r\n[H].[C]";
            var expectedClean = "[A].[B].[C]\r\n[A].[B].[D]\r\n[A].[D]\r\n[A].[E].[B].[C]\r\n[A].[E].[B].[D]\r\n[A].[F]\r\n[A].[G].[G]\r\n[A].[G].[C]\r\n[A].[H].[C]";

            Assert.Equal(output, expected);
            Assert.Equal(outputClean, expectedClean);
        }

        private string GetOutputPaths(IEnumerable<Path<HierarchicalEntity>> paths)
        {
            var output = "";
            foreach (var path in paths)
            {
                output += path.ToString(true);
                output += "\r\n";
            }

            return output.Trim();
        }
    }
}
