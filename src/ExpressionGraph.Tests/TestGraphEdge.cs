using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Tests
{
    [TestClass]
    public class TestGraphEdge
    {
        [TestMethod]
        public void TestGraphEdgeAntiparallel()
        {
            var vertexesSources = ExpressionUtils.FromString("A+(B+C+D)+A+(I+(J+C)+A)");
            var graphs = vertexesSources.ToGraphs(f => f.Children);
            var res = "";
            foreach (var edge in graphs.ElementAt(0).Edges)
                foreach (var edge2 in graphs.ElementAt(0).Edges)
                    if (edge.IsAntiparallel(edge2))
                        res += "'" + edge.ToString() + "' antiparallel of '" + edge2.ToString() + "'; ";

            Assert.IsTrue(res == "'A, A' antiparallel of 'A, A'; 'A, I' antiparallel of 'I, A'; 'I, A' antiparallel of 'A, I'; ", "Graph A");

            res = "";
            foreach (var edge in graphs.ElementAt(4).Edges)
                foreach (var edge2 in graphs.ElementAt(4).Edges)
                    if (edge.IsAntiparallel(edge2))
                        res += "'" + edge.ToString() + "' antiparallel of '" + edge2.ToString() + "'; ";

            Assert.IsTrue(res == "'I, A' antiparallel of 'A, I'; 'A, A' antiparallel of 'A, A'; 'A, I' antiparallel of 'I, A'; ", "Graph I");
        }

        [TestMethod]
        public void TestGraphEdgeLoop()
        {
            var vertexesSources = ExpressionUtils.FromString("A+(B+C+D)+A+(I+(J+C)+A)");
            var graphs = vertexesSources.ToGraphs(f => f.Children);
            var res = "";
            foreach (var edge in graphs.ElementAt(0).Edges)
                if (edge.IsLoop)
                    res += edge.ToString();

            Assert.IsTrue(res == "A, A", "Graph A");

            res = "";
            foreach (var edge in graphs.ElementAt(4).Edges)
                if (edge.IsLoop)
                    res += edge.ToString();

            Assert.IsTrue(res == "A, A", "Graph I");
        }
    }
}
