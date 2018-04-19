using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Tests
{
    public partial class TestExpression
    {
        [TestMethod]
        public void TestExpressionPreviousDepthStartAndDepthEnd()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+G+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+G+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)
             *D=   ^                        ^  
             *J=     ^    
             *G=             ^                   ^  ^          ^ ^
             *C=        
             *B=                               
            */

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(f => f.ToString() == "A").Previous(0, 0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'positionStart' parameter can not be lower than 1.");
            }

            result = expression.Find(f => f.ToString() == "C").Previous(1, 1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "", "Test C: positionStart = 1, positionStart = 1");

            result = expression.Find(f => f.ToString() == "B").Previous(1, 1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "", "Test B: positionStart = 1, positionStart = 1");

            result = expression.Find(f => f.ToString() == "D").Previous(1, 1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "K", "Test D: positionStart = 1, positionStart = 1");

            result = expression.Find(f => f.ToString() == "D").Previous(1, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "K,B", "Test D: positionStart = 1, positionStart = 2");

            result = expression.Find(f => f.ToString() == "G").Previous(1, 1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,Q", "Test D: positionStart = 1, positionStart = 3");

            result = expression.Find(f => f.ToString() == "G").Previous(1, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,Q,O", "Test D: positionStart = 1, positionStart = 2");

            result = expression.Find(f => f.ToString() == "G").Previous(2, 2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "O", "Test D: positionStart = 2, positionStart = 2");

            result = expression.Find(f => f.ToString() == "G").Previous(2, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "O,P", "Test D: positionStart = 2, positionStart = 3");

            result = expression.Find(f => f.ToString() == "G").Previous(3, 3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "P", "Test D: positionStart = 3, positionStart = 3");

            result = expression.Find(f => f.ToString() == "G").Previous(1, 4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "H,Q,O,P,E", "Test D: positionStart = 1, positionStart = 4");
        }

        [TestMethod]
        public void TestExpressionPreviousWithFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+G+W+S)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+G+W+S)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)
             *S=             3 2 1               5  4          3 2 1 
             *S=     2  1     
            */

            List<ExpressionItem<HierarchicalEntity>> result;
            ExpressionFilterDelegate2<HierarchicalEntity> filter;

            filter = (previous, posPrevious) => posPrevious % 2 == 0;
            result = expression.Find(f => f.ToString() == "S").Previous(filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "G,C,Q,P", "Test S: return all depth that are pair - with mod of 2");
        }

        [TestMethod]
        public void TestExpressionPreviousWithStopAndFilter()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+G+W+S)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+G+W+S)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)
             *S=             3 2 1               5  4          3 2 1 
             *S=     2  1     
            */

            List<ExpressionItem<HierarchicalEntity>> result;
            ExpressionFilterDelegate2<HierarchicalEntity> stop;
            ExpressionFilterDelegate2<HierarchicalEntity> filter;

            stop = (previous, posPrevious) => posPrevious == 3;
            result = expression.Find(f => f.ToString() == "S").PreviousUntil(stop).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,G,H,J,C,G,Q,O", "Test S: posPrevious == 3;");

            filter = (previous, posPrevious) => previous.Parent.ToString() != "B";
            result = expression.Find(f => f.ToString() == "S").PreviousUntil(stop, filter).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,G,H,G,Q,O", "Test S: posPrevious == 3 and filter previous.Parent != \"B\"");
        }

        [TestMethod]
        public void TestExpressionPreviousWithDepth()
        {
            IEnumerable<HierarchicalEntity> entities;
            var expressionIn = "A+(B+C+(J+(I+H+G+W+S)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)";
            var expression = GetExpression(expressionIn, out entities);

            /*
                A+(B+C+(J+(I+H+G+W+S)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+G+S)
             *S=             3 2 1               5  4          3 2 1 
             *S=     2  1     
            */

            List<ExpressionItem<HierarchicalEntity>> result;

            try
            {
                result = expression.Find(entities.GetByIdentity("A")).Previous(0).ToList();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The 'positionEnd' parameter can not be lower than 1.");
            }

            result = expression.Find(entities.GetByIdentity("A")).Previous().ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "", "A: empty");

            result = expression.Find(entities.GetByIdentity("S")).Previous(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,G,J,C,G,Q", "S: get max 2 items");

            result = expression.Find(entities.GetByIdentity("S")).Previous(3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,G,H,J,C,G,Q,O", "S: all");

            result = expression.Find(f => f.ToString() == "S").Previous().ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "W,G,H,J,C,G,Q,O,P,E", "Test S: all");
        }
    }
}
