using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Tests
{
    public partial class TestExpression
    {
        [TestMethod]
        public void TestExpressionSiblingsDepthStartAndDepthEnd()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)
             *S=             ^   ^ ^              
             *S=     ^  ^  
             *S=                                 ^  ^          ^ ^   ^
            */

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(f => f.ToString() == "A").Siblings(0, 0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'positionStart' parameter can not be lower than 1.");
            }

            result = expression.Find(f => f.ToString() == "S").Siblings(1, 1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,W,J,Q,V", "Test S: positionStart = 1, positionStart = 1");

            result = expression.Find(f => f.ToString() == "S").Siblings(1, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,W,Y,J,C,Q,O,V", "Test S: positionStart = 1, positionStart = 2");

            result = expression.Find(f => f.ToString() == "S").Siblings(2, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "Y,C,O", "Test S: positionStart = 2, positionStart = 2");

            result = expression.Find(f => f.ToString() == "S").Siblings(2, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "Y,C,O,P", "Test S: positionStart = 2, positionStart = 3");

            result = expression.Find(f => f.ToString() == "S").Siblings(3, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "P", "Test S: positionStart = 3, positionStart = 3");

            result = expression.Find(f => f.ToString() == "S").Siblings(1, 4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,W,Y,J,C,Q,O,P,E,V", "Test S: positionStart = 1, positionStart = 4");
        }

        [TestMethod]
        public void TestExpressionSiblingsWithFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)
             *S=             1   1 2              
             *S=     2  1  
             *S=                                 4  3          2 1   1
            */

            List<ExpressionItem<HierarchicalEntity>> result;
            ExpressionFilterDelegate2<HierarchicalEntity> filter;

            filter = (sibling, posSibling) => posSibling % 2 == 0 || sibling.ToString() == "H" || sibling.ToString() == "V";
            result = expression.Find(f => f.ToString() == "S").Siblings(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,Y,C,O,E,V", "Test S: return all depth that are pair include H and V- with mod of 2");
        }

        [TestMethod]
        public void TestExpressionSiblingsWithStopAndFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)
             *S=             1   1 2              
             *S=     2  1  
             *S=                                 4  3          2 1   1
            */

            List<ExpressionItem<HierarchicalEntity>> result;
            ExpressionFilterDelegate2<HierarchicalEntity> stop;
            ExpressionFilterDelegate2<HierarchicalEntity> filter;

            stop = (sibling, posSibling) => posSibling == 1;
            result = expression.Find(f => f.ToString() == "S").SiblingsUntil(stop).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,W,J,Q,V", "Test S: posPrevious == 1;");

            filter = (previous, posSibling) => previous.Parent.ToString() != "B";
            result = expression.Find(f => f.ToString() == "S").SiblingsUntil(stop, filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,W,Q,V", "Test S: posPrevious == 1 and filter previous.Parent != \"B\"");
        }

        [TestMethod]
        public void TestExpressionSiblingsWithDepth()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)
             *S=             1   1 2              
             *S=     2  1  
             *S=                                 4  3          2 1   1
            */

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(entities.GetByIdentity("A")).Siblings(0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'positionEnd' parameter can not be lower than 1.");
            }

            result = expression.Find(entities.GetByIdentity("B")).Siblings().ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "K,D", "A: Siblings");

            result = expression.Find(entities.GetByIdentity("S")).Siblings(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,W,Y,J,C,Q,O,V", "S: get max 2 items");

            result = expression.Find(entities.GetByIdentity("S")).Siblings(3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,W,Y,J,C,Q,O,P,V", "S: all");

            result = expression.Find(f => f.ToString() == "S").Siblings().ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,W,Y,J,C,Q,O,P,E,V", "Test S: all");
        }
    }
}
