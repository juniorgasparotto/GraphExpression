using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.IO;

namespace ExpressionGraph.Tests
{
    [TestClass]
    public class TestGraphVertex
    {
        [TestMethod]
        public void TestGraphVertexPropertiesG1()
        {
            var vertexesSources = ExpressionUtils.FromString("SPL+(COR+PAL)+(SAN+PAL+COR)");
            var graphs = vertexesSources.ToGraphs(f => f.Children);
            var graph = graphs.First();

            var spl = graph.Vertexes.ElementAt(0);
            var testCount = 1;
            // number of visit in vertex
            Assert.IsTrue(spl.CountVisited == 1, testCount++.ToString());

            // verify if is root (source)
            Assert.IsTrue(spl.IsSource == true, testCount++.ToString());

            // verify if is sink (leaf - without children)
            Assert.IsTrue(spl.IsSink == false, testCount++.ToString());

            // verify if is isolated (source and sink in the same time)
            Assert.IsTrue(spl.IsIsolated == false, testCount++.ToString());

            // verify all parents
            Assert.IsTrue(spl.Parents.Count() == 0, testCount++.ToString());

            // verify all children
            Assert.IsTrue(spl.Children.Count() == 2, testCount++.ToString());
            Assert.IsTrue(spl.Children.ElementAt(0).ToString() == "COR", testCount++.ToString());
            Assert.IsTrue(spl.Children.ElementAt(1).ToString() == "SAN", testCount++.ToString());

            var cor = graph.Vertexes.ElementAt(1);

            // number of visit in vertex
            Assert.IsTrue(cor.CountVisited == 2, testCount++.ToString());

            // verify if is root (source)
            Assert.IsTrue(cor.IsSource == false, testCount++.ToString());

            // verify if is sink (leaf - without children)
            Assert.IsTrue(cor.IsSink == false, testCount++.ToString());

            // verify if is isolated (source and sink in the same time)
            Assert.IsTrue(cor.IsIsolated == false, testCount++.ToString());

            // verify all parents
            Assert.IsTrue(cor.Parents.Count() == 2, testCount++.ToString());
            Assert.IsTrue(cor.Parents.ElementAt(0).ToString() == "SPL", testCount++.ToString());
            Assert.IsTrue(cor.Parents.ElementAt(1).ToString() == "SAN", testCount++.ToString());

            // verify all children
            Assert.IsTrue(cor.Children.Count() == 1, testCount++.ToString());
            Assert.IsTrue(cor.Children.ElementAt(0).ToString() == "PAL", testCount++.ToString());

            var pal = graph.Vertexes.ElementAt(2);

            // number of visit in vertex
            Assert.IsTrue(pal.CountVisited == 3, testCount++.ToString());

            // verify if is root (source)
            Assert.IsTrue(pal.IsSource == false, testCount++.ToString());

            // verify if is sink (leaf - without children)
            Assert.IsTrue(pal.IsSink == true, testCount++.ToString());

            // verify if is isolated (source and sink in the same time)
            Assert.IsTrue(pal.IsIsolated == false, testCount++.ToString());

            // verify all parents
            Assert.IsTrue(pal.Parents.Count() == 2, testCount++.ToString());
            Assert.IsTrue(pal.Parents.ElementAt(0).ToString() == "COR", testCount++.ToString());
            Assert.IsTrue(pal.Parents.ElementAt(1).ToString() == "SAN", testCount++.ToString());

            // verify all children
            Assert.IsTrue(pal.Children.Count() == 0, testCount++.ToString());

            var san = graph.Vertexes.ElementAt(3);

            // number of visit in vertex
            Assert.IsTrue(san.CountVisited == 1, testCount++.ToString());

            // verify if is root (source)
            Assert.IsTrue(san.IsSource == false, testCount++.ToString());

            // verify if is sink (leaf - without children)
            Assert.IsTrue(san.IsSink == false, testCount++.ToString());

            // verify if is isolated (source and sink in the same time)
            Assert.IsTrue(san.IsIsolated == false, testCount++.ToString());

            // verify all parents
            Assert.IsTrue(san.Parents.Count() == 1, testCount++.ToString());
            Assert.IsTrue(san.Parents.ElementAt(0).ToString() == "SPL", testCount++.ToString());

            // verify all children
            Assert.IsTrue(san.Children.Count() == 2, testCount++.ToString());
            Assert.IsTrue(san.Children.ElementAt(0).ToString() == "PAL", testCount++.ToString());
            Assert.IsTrue(san.Children.ElementAt(1).ToString() == "COR", testCount++.ToString());
        }
    }
}
