using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class DebugAndToStringTest
    {
        [Fact]
        public void Vertex_VerifyToString_ReturnEntityToString()
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
        public void Edge_VerifyToStringCircular_ReturnEdgeToString()
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
        public void Edge_VerifyToStringComplex_ReturnEdgeToString()
        {
            var A = new
            {
                Prop1 = 10,
                Prop2 = "Test 2",
            };

            var expression = A.AsExpression();
            Assert.Equal(3, expression.Graph.Edges.Count);
            Assert.Equal($", {{{A.GetHashCode()}}}", expression.Graph.Edges[0].ToString());
            Assert.Equal($"{{{A.GetHashCode()}}}, {{@Prop1: 10}}", expression.Graph.Edges[1].ToString());
            Assert.Equal($"{{{A.GetHashCode()}}}, {{@Prop2: \"Test 2\"}}", expression.Graph.Edges[2].ToString());
        }

        [Fact]
        public void Path_VerifyToStringInCircularEntity_ReturnEdgeToString()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            A = A + (B + C);
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("[A]", expression[0].Path.ToString());
            Assert.Equal("[A].[B]", expression[1].Path.ToString());
            Assert.Equal("[A].[B].[C]", expression[2].Path.ToString());
        }

        [Fact]
        public void Path_VerifyToStringInComplexEntity_ReturnEdgeToString()
        {
            var A = new
            {
                Prop1 = 10
            };

            var expression = A.AsExpression();
            Assert.Equal($"[{{{A.GetHashCode()}}}]", expression[0].Path.ToString());
            Assert.Equal($"[{{{A.GetHashCode()}}}].[{{@Prop1: 10}}]", expression[1].Path.ToString());
        }
    }
}
