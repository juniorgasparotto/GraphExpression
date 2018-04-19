using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Tests
{
    public partial class TestExpression
    {
        [TestMethod]
        public void TestExpressionNextsDepthStartAndDepthEnd()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+R)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+R)
             *B=                            ^  ^
             *O=                 ^ ^                             
             *O=                                                 ^ ^ ^
            */

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(f => f.ToString() == "A").Nexts(0, 0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'positionStart' parameter can not be lower than 1.");
            }

            result = expression.Find(f => f.ToString() == "B").Nexts(1, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "K,D", "Test: positionStart = 1, positionStart = 2");

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
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A + ( B + C + ( J + ( I + H + O + W + Y ) + L ) + S ) + K + ( D + E + ( P + ( U + Y ) + R ) + O + Q + G + S + ( J + ( I + H + O + W + Y ) + L ) ) 
             *B=                                                        1     2
             *B=                                                              ^
             *                                    1   2                                                           1   2   3     4                 1   2    
             *O=                                      ^                                                           
             *O=                                                                                                      ^         ^                 
             *O=                                                                                                                                      ^
            */

            List<ExpressionItem<HierarchicalEntity>> result;
            ExpressionFilterDelegate2<HierarchicalEntity> filter;

            filter = (next, posNext) => posNext % 2 == 0;
            result = expression.Find(f => f.ToString() == "O").Nexts(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "Y,G,J,Y", "Test O: return all depth that are pair - with mod of 2");

            result = expression.Find(f => f.ToString() == "I").Nexts(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "", "Test I: return all depth that are pair - with mod of 2");

            result = expression.Find(f => f.ToString() == "B").Nexts(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "D", "Test B: return all depth that are pair - with mod of 2");
        }

        [TestMethod]
        public void TestExpressionNextsWithStopAndFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A + ( B + C + ( J + ( I + H + O + W + Y ) + L ) + S ) + K + ( D + E + ( P + ( U + Y ) + R ) + O + Q + G + S + ( J + ( I + H + O + W + Y ) + L ) ) 
             *B=                                                        1     2
             *B=                                                              ^
             *                                    1   2                                                           1   2   3     4                 1   2    
             *O=                                      ^                                                           
             *O=                                                                                                      ^                           
             *O=                                                                                                                                      ^
            */

            List<ExpressionItem<HierarchicalEntity>> result;
            ExpressionFilterDelegate2<HierarchicalEntity> stop;
            ExpressionFilterDelegate2<HierarchicalEntity> filter;

            stop = (next, posNext) => posNext == 2;
            result = expression.Find(f => f.ToString() == "B").NextsUntil(stop).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "K,D", "Test D: return items until 'stop delegate' return false");

            result = expression.Find(f => f.ToString() == "O").NextsUntil(stop).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,G,W,Y", "Test O: return items until 'stop delegate' return false");

            filter = (descendant, depthDescendant) => descendant.ToString() != "G";
            result = expression.Find(f => f.ToString() == "O").NextsUntil(stop, filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,W,Y", "Test O: return items until 'stop delegate' return false and filter items");
        }

        [TestMethod]
        public void TestExpressionNextsWithDepth()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+O+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S+J)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A + ( B + C + ( J + ( I + H + O + W + Y ) + L ) + S ) + K + ( D + E + ( P + ( U + Y ) + R ) + O + Q + G + S + ( J + ( I + H + O + W + Y ) + L ) ) 
                1 1 2 2 2 2 2 3 3 3 4 4 4 4 4 4 4 4 4 4 4 3 3 3 2 2 2 1 1 1 2 2 2 2 2 3 3 3 4 4 4 4 4 3 3 3 2 2 2 2 2 2 2 2 2 3 3 3 4 4 4 4 4 4 4 4 4 4 4 3 3 3 2 
             *O=                                  ^   ^                                                           
             *O=                                                                                                  ^   ^   ^     ^                 
             *O=                                                                                                                                  ^   ^
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

            result = expression.Find(entities.GetByIdentity("B")).Nexts().ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "K,D", "A: all");

            result = expression.Find(entities.GetByIdentity("B")).Nexts(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "K,D", "A: get max 2 items");
            
            result = expression.Find(entities.GetByIdentity("O")).Nexts(3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,G,S,W,Y", "O: max 3 items");

            result = expression.Find(entities.GetByIdentity("O")).Nexts(4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,Y,Q,G,S,J,W,Y", "O: all");
        }
    }
}
