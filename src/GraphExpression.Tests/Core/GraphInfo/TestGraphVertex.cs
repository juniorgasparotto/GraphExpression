using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class TestGraphVertex
    {
        [Fact]
        public void TestGraphVertexPropertiesG1()
        {
            var SPL = new HierarchicalEntity("SPL");
            var COR = new HierarchicalEntity("COR");
            var PAL = new HierarchicalEntity("PAL");
            var SAN = new HierarchicalEntity("SAN");

            SPL = SPL + (COR + PAL) + (SAN + PAL + COR);

            var graph = SPL.AsExpression(f => f.Children, true);
            var output = graph.DefaultSerializer.Serialize();
            Assert.Equal("SPL + (COR + PAL) + (SAN + PAL + (COR + PAL))", output);
            var spl = graph.GraphInfo.Vertexes.ElementAt(0);
            
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

            var cor = graph.GraphInfo.Vertexes.ElementAt(1);

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

            var pal = graph.GraphInfo.Vertexes.ElementAt(2);

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

            var san = graph.GraphInfo.Vertexes.ElementAt(3);

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
