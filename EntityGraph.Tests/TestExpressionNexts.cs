using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using EntityGraph;

namespace Graph.Tests
{
    public partial class TestExpression
    {
        [TestMethod]
        public void TestExpressionNextsDepthStartAndDepthEnd()
        {
            ListOfHierarchicalEntity entities;
            var expressionIn = "A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)
             *A=   ^                        ^  ^
             *J=        
             *C=        ^                ^
             *K=                               ^
             *O=                 ^ ^                             ^ ^ ^
            */

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(f => f.ToString() == "A").Descendants(0, 0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'positionStart' parameter can not be lower than 1.");
            }

            result = expression.Find(f => f.ToString() == "A").Nexts(1, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,K", "Test: positionStart = 1, positionStart = 2");

            result = expression.Find(f => f.ToString() == "O").Nexts(1, 1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Q", "Test: positionStart = 1, positionStart = 1");

            result = expression.Find(f => f.ToString() == "O").Nexts(1, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,G", "Test: positionStart = 1, positionStart = 2");

            result = expression.Find(f => f.ToString() == "O").Nexts(1, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,G,R", "Test: positionStart = 1, positionStart = 3");

            result = expression.Find(f => f.ToString() == "O").Nexts(2, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "Y,G", "Test: positionStart = 2, positionStart = 2");

            result = expression.Find(f => f.ToString() == "O").Nexts(2, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "Y,G,R", "Test: positionStart = 2, positionStart = 3");

            result = expression.Find(f => f.ToString() == "O").Nexts(3, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "R", "Test: positionStart = 3, positionStart = 3");
        }

        [TestMethod]
        public void TestExpressionNextsWithFilter()
        {

            ListOfHierarchicalEntity entities;
            var expressionIn = "A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)
             *A=   1                        2  3
             *I=        
             *O=                 1 2                             1 2 3 4
            */

            List<ExpressionItem<HierarchicalEntity>> result;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> filter;

            filter = (next, posNext) => posNext % 2 == 0;
            result = expression.Find(f => f.ToString() == "O").Nexts(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "Y,G,J", "Test O: return all depth that are pair - with mod of 2");

            result = expression.Find(f => f.ToString() == "I").Nexts(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "", "Test I: return all depth that are pair - with mod of 2");

            result = expression.Find(f => f.ToString() == "A").Nexts(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "K", "Test A: return all depth that are pair - with mod of 2");
        }

        [TestMethod]
        public void TestExpressionNextsWithStopAndFilter()
        {
            ListOfHierarchicalEntity entities;
            var expressionIn = "A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)
             *A=   1                        2  3
             *I=        
             *O=                 1 2                             1 2 3 4
            */

            List<ExpressionItem<HierarchicalEntity>> result;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> stop;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> filter;

            stop = (next, posNext) => posNext == 2;
            result = expression.Find(f => f.ToString() == "A").NextsUntil(stop).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,K", "Test A: return items until 'stop delegate' return false");

            result = expression.Find(f => f.ToString() == "O").NextsUntil(stop).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,G", "Test O: return items until 'stop delegate' return false");

            filter = (descendant, depthDescendant) => descendant.ToString() != "Q" && descendant.ToString() != "W";
            result = expression.Find(f => f.ToString() == "O").NextsUntil(stop, filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "Y,G", "Test O: return items until 'stop delegate' return false and filter items");
        }

        [TestMethod]
        public void TestExpressionNextsWithDepth()
        {
            ListOfHierarchicalEntity entities;
            var expressionIn = "A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)
             *A=   1                        2  3
             *O=                 1 2                             1 2 3 4
            */

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(entities.GetByIdentity("A")).Nexts(0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'positionEnd' parameter can not be lower than 1.");
            }

            result = expression.Find(entities.GetByIdentity("A")).Nexts().ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,K,D", "A: all");

            result = expression.Find(entities.GetByIdentity("A")).Nexts(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,K", "A: get max 2 items");
            
            result = expression.Find(entities.GetByIdentity("O")).Nexts().ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,G,S,J", "O: all");

            result = expression.Find(entities.GetByIdentity("O")).Nexts(3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,G,S", "O: max 3 items");
        }
    }
}
