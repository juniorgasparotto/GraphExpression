using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class EdgeTest
    {
        [Fact]
        public void VerifyTargetAndSource_CheckMatchWithExpression()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(3, expression.Graph.Edges.Count);

            Assert.Null(expression.Graph.Edges[0].Source);
            Assert.Same(expression[0], expression.Graph.Edges[0].Target);

            Assert.Same(expression[0], expression.Graph.Edges[1].Source);
            Assert.Same(expression[1], expression.Graph.Edges[1].Target);

            Assert.Same(expression[1], expression.Graph.Edges[2].Source);
            Assert.Same(expression[2], expression.Graph.Edges[2].Target);
        }

        [Fact]
        public void VerifyIsLoop_NoEntityIsLooping()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(3, expression.Graph.Edges.Count);
            Assert.False(expression.Graph.Edges[0].IsLoop);
            Assert.False(expression.Graph.Edges[1].IsLoop);
            Assert.False(expression.Graph.Edges[2].IsLoop);
        }

        [Fact]
        public void VerifyIsLoop_AllEntityInLoopingExceptB()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            A = A + (A + A) + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(4, expression.Graph.Edges.Count);
            Assert.False(expression.Graph.Edges[0].IsLoop);
            Assert.True(expression.Graph.Edges[1].IsLoop);
            Assert.True(expression.Graph.Edges[2].IsLoop);
            Assert.False(expression.Graph.Edges[3].IsLoop);
        }

        [Fact]
        public void VerifyIsAntiparallel_AllEntityInLoopingExceptB()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            A = A + (B + A);
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(3, expression.Graph.Edges.Count);
            // [0] -> null, A
            // [1] -> A, B
            // not antiparallel
            Assert.False(expression.Graph.Edges[0].IsAntiparallel(expression.Graph.Edges[1]));

            // [0] -> A, B
            // [1] -> B, A
            // = is antiparallel
            Assert.True(expression.Graph.Edges[1].IsAntiparallel(expression.Graph.Edges[2]));
        }

        [Fact]
        public void VerifyToStringCircular_ReturnEdgeToString()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var TW = new CircularEntity("TWO WORD");
            A = A + (B + C) + TW;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(4, expression.Graph.Edges.Count);
            Assert.Equal(", A", expression.Graph.Edges[0].ToString());
            Assert.Equal("A, B", expression.Graph.Edges[1].ToString());
            Assert.Equal("B, C", expression.Graph.Edges[2].ToString());
            Assert.Equal("A, \"TWO WORD\"", expression.Graph.Edges[3].ToString());
        }

        [Fact]
        public void VerifyToStringComplex_ReturnEdgeToString()
        {
            var A = new
            {
                Prop1 = 10,
                Prop2 = "Test 2",
            };

            var expression = A.AsExpression();
            Assert.Equal(3, expression.Graph.Edges.Count);
            Assert.Equal($", {A.GetHashCode()}", expression.Graph.Edges[0].ToString());
            Assert.Equal($"{A.GetHashCode()}, @Prop1: 10", expression.Graph.Edges[1].ToString());
            Assert.Equal($"{A.GetHashCode()}, @Prop2: \"Test 2\"", expression.Graph.Edges[2].ToString());
        }

        [Fact]
        public void VerifyAllAntiparallelInExpression()
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
        public void VerifyAllIsLooplInExpression()
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
