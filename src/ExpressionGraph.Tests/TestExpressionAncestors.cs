using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Tests
{
    public partial class TestExpression
    {
        [TestMethod]
        public void TestExpressionAncestorsDepthStartAndDepthEnd()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+O)))+K+(D+E+(P+(U+Y)))";
            var expression = GetExpression(expressionIn, out entities);

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors(0, 0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'depthStart' parameter can not be lower than 1.");
            }

            result = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors(1, 1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "J,P", "Test: DepthStart = 1, DepthEnd = 1");

            result = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors(1, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "J,B,P,D", "Test: DepthStart = 1, DepthEnd = 2");

            result = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors(2, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,D", "Test: DepthStart = 2, DepthEnd = 2");

            result = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors(2, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,A,D,A", "Test: DepthStart = 2, DepthEnd = 3");

            result = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors(3, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "A,A", "Test: DepthStart = 3, DepthEnd = 3");

            result = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors(3, 4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "A,A", "Test: DepthStart = 3, DepthEnd = 4");
        }

        [TestMethod]
        public void TestExpressionAncestorsWithFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+(O+(R+T)))))+K+(D+E+(P+(U+(Y+(L+N)))))";
            var expression = GetExpression(expressionIn, out entities);

            List<ExpressionItem<HierarchicalEntity>> result;
            ExpressionFilterDelegate2<HierarchicalEntity> filter;

            /*
             * return depths that are mod of 2.
             * 
                    A+(B+C+(J+(I+(O+(R+T)))))+K+(D+E+(P+(U+(Y+(L+N)))))
                T = 6  5    4  3  2  1
                T = ^       ^     ^
                N = 6                            5    4  3  2  1
                N = ^                                 ^     ^
            *
            */

            filter = (ancestor, depthAncestor) => depthAncestor % 2 == 0;
            result = expression.Find(f => f.ToString() == "T" || f.ToString() == "N").Ancestors(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "O,J,A,Y,P,A", "Test: return all depth that are pair - with mod of 2");
        }

        [TestMethod]
        public void TestExpressionAncestorsWithStopAndFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+(O+(R+T)))))+K+(D+E+(P+(U+(Y+(L+N)))))";
            var expression = GetExpression(expressionIn, out entities);

            List<ExpressionItem<HierarchicalEntity>> result;
            ExpressionFilterDelegate2<HierarchicalEntity> stop;
            ExpressionFilterDelegate2<HierarchicalEntity> filter;

            stop = (ancestor, depthAncestor) => ancestor.ToString() == "J" || ancestor.ToString() == "P";
            result = expression.Find(f => f.ToString() == "T" || f.ToString() == "N").AncestorsUntil(stop).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "R,O,I,J,L,Y,U,P", "Test: return items until 'stop delegate' return false");

            filter = (ancestor, depthAncestor) => ancestor.ToString() != "R" && ancestor.ToString() != "P" && ancestor.ToString() != "L";
            result = expression.Find(f => f.ToString() == "T" || f.ToString() == "N").AncestorsUntil(stop, filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "O,I,J,Y,U", "Test: return items until 'stop delegate' return false and filter items");
        }

        [TestMethod]
        public void TestExpressionAncestorsWithDepth()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expression = GetExpression("A+(B+C+(J+I))+K+(D+E+(P+U))", out entities);

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(entities.GetByIdentity("I")).Ancestors(0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'depthEnd' parameter can not be lower than 1.");
            }

            result = expression.Find(entities.GetByIdentity("I")).Ancestors(1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "J", "level 1");
            result = expression.Find(entities.GetByIdentity("I")).Ancestors(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "J,B", "level 2");
            result = expression.Find(entities.GetByIdentity("I")).Ancestors(3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "J,B,A", "level 3");
            result = expression.Find(entities.GetByIdentity("I")).Ancestors(4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "J,B,A", "level 4");
        }
    }
}
