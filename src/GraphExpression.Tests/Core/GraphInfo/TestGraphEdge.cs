using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class TestGraphEdge
    {
        [Fact]
        public void TestGraphEdgeAntiparallel()
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
