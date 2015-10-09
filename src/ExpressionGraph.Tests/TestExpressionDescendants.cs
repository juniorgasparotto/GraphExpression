using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Tests
{
    public partial class TestExpression
    {
        [TestMethod]
        public void TestExpressionDescendentsDepthStartAndDepthEnd()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+O)+L))+K+(D+E+(P+(U+Y)+R))";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+O)+L))+K+(D+E+(P+(U+Y)+R))
                     1  1  2 3  2        1  1  2 3  2
            */

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(f => f.ToString() == "B").Descendants(0, 0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'depthStart' parameter can not be lower than 1.");
            }

            result = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Descendants(1, 1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "C,J,E,P", "Test: DepthStart = 1, DepthEnd = 1");

            result = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Descendants(1, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "C,J,I,L,E,P,U,R", "Test: DepthStart = 1, DepthEnd = 2");

            result = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Descendants(1, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "C,J,I,O,L,E,P,U,Y,R", "Test: DepthStart = 1, DepthEnd = 3");

            result = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Descendants(2, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "I,L,U,R", "Test: DepthStart = 2, DepthEnd = 2");

            result = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Descendants(2, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "I,O,L,U,Y,R", "Test: DepthStart = 2, DepthEnd = 3");

            result = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Descendants(3, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "O,Y", "Test: DepthStart = 3, DepthEnd = 3");

            result = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Descendants(3, 4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "O,Y", "Test: DepthStart = 3, DepthEnd = 4");

            result = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Descendants(4, 4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "", "Test: DepthStart = 4, DepthEnd = 4");
        }

        [TestMethod]
        public void TestExpressionDescendentsWithFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+(O+(R+T)))))+K+(D+E+(P+(U+(Y+(L+N)))))";
            var expression = GetExpression(expressionIn, out entities);

            List<ExpressionItem<HierarchicalEntity>> result;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> filter;

            /*
             * return depths that are mod of 2.
             * 
                    A+(B+C+(J+(I+(O+(R+T)))))+K+(D+E+(P+(U+(Y+(L+N)))))
                A =    1 2  2  3  4  5 6      1  1 2  2  3  4  5 6
                A =      ^  ^          ^           ^  ^     ^    ^ 
                D =                                1  1  2  3  4 5
                P =                                      ^     ^
            *
            */

            filter = (descendant, depthDescendant) => depthDescendant % 2 == 0;
            result = expression.Find(f => f.ToString() == "A").Descendants(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "C,J,O,T,E,P,Y,N", "Test A: return all depth that are pair - with mod of 2");

            result = expression.Find(f => f.ToString() == "D").Descendants(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "U,L", "Test D: return all depth that are pair - with mod of 2");

            result = expression.Find(f => f.ToString() == "A" || f.ToString() == "D").Descendants(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "C,J,O,T,E,P,Y,N,U,L", "Test A and D: return all depth that are pair - with mod of 2");
        }

        [TestMethod]
        public void TestExpressionDescendentsWithStopAndFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+(O+(R+T)))))+K+(D+E+(P+(U+(Y+(L+N)))))";
            var expression = GetExpression(expressionIn, out entities);
            List<ExpressionItem<HierarchicalEntity>> result;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> stop;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> filter;

            stop = (descendant, depthDescendant) => descendant.ToString() == "R";
            result = expression.Find(f => f.ToString() == "A").DescendantsUntil(stop).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,C,J,I,O,R", "Test: return items until 'stop delegate' return false");

            filter = (descendant, depthDescendant) => descendant.ToString() != "C" && descendant.ToString() != "O";
            result = expression.Find(f => f.ToString() == "A").DescendantsUntil(stop, filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,J,I,R", "Test: return items until 'stop delegate' return false and filter items");
        }

        [TestMethod]
        public void TestExpressionDescendentsWithDepth()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expression = GetExpression("A+(B+C+(J+I))+K+(D+E+(P+U))", out entities);

            var debug = expression.ToDebug();

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(entities.GetByIdentity("A")).Descendants(0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'depthEnd' parameter can not be lower than 1.");
            }

            result = expression.Find(entities.GetByIdentity("A")).Descendants(1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,K,D", "level 1");
            result = expression.Find(entities.GetByIdentity("A")).Descendants(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,C,J,K,D,E,P", "level 2");
            result = expression.Find(entities.GetByIdentity("A")).Descendants(3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,C,J,I,K,D,E,P,U", "level 3");
            result = expression.Find(entities.GetByIdentity("A")).Descendants(4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,C,J,I,K,D,E,P,U", "level 4");
        }
    }
}
