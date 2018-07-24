using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class ConceptTest : EntitiesData
    {
        [Fact]
        public void CSharpExpression_CreateGraph()
        {
            var r = A + (B + (C + A) + A) + (D + D + E + (F + (G + A + C) + Y) + Z) + G;

            // A
            Assert.Equal(3, A.Count);
            Assert.Equal("B", A[0].Name);
            Assert.Equal("D", A[1].Name);
            Assert.Equal("G", A[2].Name);

            // B
            Assert.Equal(2, B.Count);
            Assert.Equal("C", B[0].Name);
            Assert.Equal("A", B[1].Name);

            // C
            Assert.Single(C);
            Assert.Equal("A", C[0].Name);

            // D
            Assert.Equal(4, D.Count);
            Assert.Equal("D", D[0].Name);
            Assert.Equal("E", D[1].Name);
            Assert.Equal("F", D[2].Name);
            Assert.Equal("Z", D[3].Name);

            // E
            Assert.Empty(E);

            // F
            Assert.Equal(2, F.Count);
            Assert.Equal("G", F[0].Name);
            Assert.Equal("Y", F[1].Name);

            // G
            Assert.Equal(2, G.Count);
            Assert.Equal("A", G[0].Name);
            Assert.Equal("C", G[1].Name);

            // Y
            Assert.Empty(Y);

            // Z
            Assert.Empty(Z);
        }
    }
}
