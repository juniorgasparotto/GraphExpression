using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using EntityGraph;

namespace Graph.Tests
{
    [TestClass]
    public class TestGraph
    {
        [TestMethod]
        public void TestGraph1()
        {
            var vertexesSources = Utils.FromExpression("A+(B+C+D)+A+(I+(J+C)+A)");
            var graphs = vertexesSources.ToGraphs(f => f.Children);
            var graph = graphs.ElementAt(0);
            //var a = graph.Vertexes.ElementAt(0).GetTokens();

            var graph1 = graphs.ElementAt(1);
            var graph2 = graphs.ElementAt(2);
            var graph3 = graphs.ElementAt(3);
            var graph4 = graphs.ElementAt(4);
            var graph5 = graphs.ElementAt(5);

            var testCount = 1;
            Assert.IsTrue(graph.Edges.Count() == 9, testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(0).ToString() == ", A", testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(1).ToString() == "A, B", testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(2).ToString() == "B, C", testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(3).ToString() == "B, D", testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(4).ToString() == "A, A", testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(5).ToString() == "A, I", testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(6).ToString() == "I, J", testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(7).ToString() == "J, C", testCount++.ToString());
            Assert.IsTrue(graph.Edges.ElementAt(8).ToString() == "I, A", testCount++.ToString());
            Assert.IsTrue(graph.Vertexes.Count() == 6, testCount++.ToString());
            Assert.IsTrue(graph.Vertexes.ElementAt(0).ToString() == "A", testCount++.ToString());
            Assert.IsTrue(graph.Vertexes.ElementAt(1).ToString() == "B", testCount++.ToString());
            Assert.IsTrue(graph.Vertexes.ElementAt(2).ToString() == "C", testCount++.ToString());
            Assert.IsTrue(graph.Vertexes.ElementAt(3).ToString() == "D", testCount++.ToString());
            Assert.IsTrue(graph.Vertexes.ElementAt(4).ToString() == "I", testCount++.ToString());
            Assert.IsTrue(graph.Vertexes.ElementAt(5).ToString() == "J", testCount++.ToString());
            Assert.IsTrue(graph.Paths.Count() == 5, testCount++.ToString());
            Assert.IsTrue(graph.Paths.ElementAt(0).ToString() == "[A].[B].[C]", testCount++.ToString());
            Assert.IsTrue(graph.Paths.ElementAt(1).ToString() == "[A].[B].[D]", testCount++.ToString());
            Assert.IsTrue(graph.Paths.ElementAt(2).ToString() == "[A].[A]", testCount++.ToString());
            Assert.IsTrue(graph.Paths.ElementAt(3).ToString() == "[A].[I].[J].[C]", testCount++.ToString());
            Assert.IsTrue(graph.Paths.ElementAt(4).ToString() == "[A].[I].[A]", testCount++.ToString());
            Assert.IsTrue(graph.ContainsGraph(graph1), testCount++.ToString());
            Assert.IsTrue(graph.ContainsGraph(graph2), testCount++.ToString());
            Assert.IsTrue(graph.ContainsGraph(graph3), testCount++.ToString());
            Assert.IsTrue(!graph.ContainsGraph(graph4), testCount++.ToString());
            Assert.IsTrue(graph.ContainsGraph(graph5), testCount++.ToString());
            
            var graphsClean = graphs.RemoveCoexistents();
            var paths = graphsClean.ToPaths();
            var pathsClean = graphsClean.ToPaths().RemoveCoexistents();

            Assert.IsTrue(graphsClean.Count() == 2, testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(0).ToString() == "[A].[B].[C]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(1).ToString() == "[A].[B].[D]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(2).ToString() == "[A].[A]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(3).ToString() == "[A].[I].[J].[C]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(4).ToString() == "[A].[I].[A]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(5).ToString() == "[I].[J].[C]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(6).ToString() == "[I].[A].[B].[C]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(7).ToString() == "[I].[A].[B].[D]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(8).ToString() == "[I].[A].[A]", testCount++.ToString());
            Assert.IsTrue(paths.ElementAt(9).ToString() == "[I].[A].[I]", testCount++.ToString());
            Assert.IsTrue(this.GetOutputPaths(pathsClean) == this.GetOutputPaths(paths.RemoveCoexistents()), testCount++.ToString());
            Assert.IsTrue(pathsClean.ElementAt(0).ToString() == "[A].[I].[J].[C]", testCount++.ToString());
            Assert.IsTrue(pathsClean.ElementAt(1).ToString() == "[A].[I].[A]", testCount++.ToString());
            Assert.IsTrue(pathsClean.ElementAt(2).ToString() == "[I].[A].[B].[C]", testCount++.ToString());
            Assert.IsTrue(pathsClean.ElementAt(3).ToString() == "[I].[A].[B].[D]", testCount++.ToString());
            Assert.IsTrue(pathsClean.ElementAt(4).ToString() == "[I].[A].[A]", testCount++.ToString());
            Assert.IsTrue(pathsClean.ElementAt(5).ToString() == "[I].[A].[I]", testCount++.ToString());
        }

        [TestMethod]
        public void TestGraph2()
        {
            var vertexesSources = Utils.FromExpression("A+(B+C+D)+D+(E+B)+F+G(G+G+C)+(H+C)");
            var graphs = vertexesSources.ToGraphs(f => f.Children);
            var graphsClean = graphs.RemoveCoexistents();

            var output = this.GetOutputPaths(graphs.ToPaths());
            var outputClean = this.GetOutputPaths(graphs.ToPaths().RemoveCoexistents());

            var expected = "[A].[B].[C]\r\n[A].[B].[D]\r\n[A].[D]\r\n[A].[E].[B].[C]\r\n[A].[E].[B].[D]\r\n[A].[F]\r\n[A].[G].[G]\r\n[A].[G].[C]\r\n[A].[H].[C]\r\n[B].[C]\r\n[B].[D]\r\n[C]\r\n[D]\r\n[E].[B].[C]\r\n[E].[B].[D]\r\n[F]\r\n[G].[G]\r\n[G].[C]\r\n[H].[C]";
            var expectedClean = "[A].[B].[C]\r\n[A].[B].[D]\r\n[A].[D]\r\n[A].[E].[B].[C]\r\n[A].[E].[B].[D]\r\n[A].[F]\r\n[A].[G].[G]\r\n[A].[G].[C]\r\n[A].[H].[C]";

            Assert.IsTrue(output == expected, "All paths");
            Assert.IsTrue(outputClean == expectedClean, "Paths without coexistents");
        }

        [TestMethod]
        public void TestGraphMultiplesRecursive()
        {
            var list = Utils.FromExpression("A+(B+C+D)+D+(E+B)+F+G(G+G+C)+(H+C)").ToGraphs(f => f.Children);
            var list1 = Utils.FromExpression("A+A").ToGraphs(f => f.Children);
            var list2 = Utils.FromExpression("A+B+A").ToGraphs(f => f.Children);
            var list3 = Utils.FromExpression("A+(B+A)").ToGraphs(f => f.Children);
            var list4 = Utils.FromExpression("A+(B+B+A)").ToGraphs(f => f.Children);
            var list5 = Utils.FromExpression("A+(B+A(B+A))").ToGraphs(f => f.Children);
            var list6 = Utils.FromExpression("A+(B+A)+B+A").ToGraphs(f => f.Children);
            var list7 = Utils.FromExpression("A+(B+A)+A+B").ToGraphs(f => f.Children);
            var list8 = Utils.FromExpression("A+(B+A)+A+(C+B)").ToGraphs(f => f.Children);
            var list9 = Utils.FromExpression("A+(B+A)+(I+A+D+B+D)+(C+B)").ToGraphs(f => f.Children);
        }

        private string GetOutputPaths(IEnumerable<Path<HierarchicalEntity>> paths)
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
