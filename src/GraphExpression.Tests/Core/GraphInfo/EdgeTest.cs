using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class EdgeTest
    {
        [Fact]
        public void TestGraphEdgeAntiparallel()
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

            var res = "";
            foreach (var edge in graphs.ElementAt(0).Edges)
                foreach (var edge2 in graphs.ElementAt(0).Edges)
                    if (edge.IsAntiparallel(edge2))
                        res += "'" + edge.ToString() + "' antiparallel of '" + edge2.ToString() + "'; ";

            Assert.Equal("'A, A' antiparallel of 'A, A'; 'A, I' antiparallel of 'I, A'; 'I, A' antiparallel of 'A, I'; ", res );

            res = "";
            foreach (var edge in graphs.ElementAt(4).Edges)
                foreach (var edge2 in graphs.ElementAt(4).Edges)
                    if (edge.IsAntiparallel(edge2))
                        res += "'" + edge.ToString() + "' antiparallel of '" + edge2.ToString() + "'; ";

            Assert.Equal("'I, A' antiparallel of 'A, I'; 'A, A' antiparallel of 'A, A'; 'A, I' antiparallel of 'I, A'; ", res);
        }

        [Fact]
        public void TestGraphEdgeLoop()
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

            var res = "";
            foreach (var edge in graphs.ElementAt(0).Edges)
                if (edge.IsLoop)
                    res += edge.ToString();

            Assert.Equal("A, A", res );

            res = "";
            foreach (var edge in graphs.ElementAt(4).Edges)
                if (edge.IsLoop)
                    res += edge.ToString();

            Assert.Equal("A, A", res );
        }
    }
}
