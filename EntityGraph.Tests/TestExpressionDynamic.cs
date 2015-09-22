using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using EntityGraph;

namespace Graph.Tests
{
    [TestClass]
    public partial class TestExpression
    {
        [TestMethod]
        public void TestExpressionDynamic()
        { 
            dynamic a = new { Name = "A", Items = new List<dynamic>() };
            dynamic b = new { Name = "B", Items = new List<dynamic>() };
            dynamic c = new { Name = "C", Items = new List<dynamic>() };

            a.Items.Add(b);
            a.Items.Add(b);
            a.Items.Add(b);
            a.Items.Add(b);
            a.Items.Add(c);
            a.Items.Add(c);
            a.Items.Add(c);
            a.Items.Add(b);
            a.Items.Add(b);

            List<dynamic> entities = new List<dynamic>();
            entities.Add(a);

            var expression = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, true, true, f => f.Name).FirstOrDefault();

            var debug = expression.ToString();
            var test = @"A + B + B + B + B + C + C + C + B + B";
            Assert.IsTrue(test == debug, "Test -1 - error");
        }

        [TestMethod]
        public void TestExpressionDynamic2()
        {
            dynamic a = new { Name = "A", Items = new List<dynamic>() };
            dynamic b = new { Name = "B", Items = new List<dynamic>() };
            dynamic c = new { Name = "C", Items = new List<dynamic>() };

            a.Items.Add(b);
            b.Items.Add(b);
            b.Items.Add(b);
            a.Items.Add(b);
            a.Items.Add(c);
            a.Items.Add(c);

            List<dynamic> entities = new List<dynamic>();
            entities.Add(a);

            var expression1 = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, false, false, f => f.Name).FirstOrDefault();
            var debug1 = expression1.ToString();

            var expression2 = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, true, false, f => f.Name).FirstOrDefault();
            var debug2 = expression2.ToString();

            var expression3 = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, false, true, f => f.Name).FirstOrDefault();
            var debug3 = expression3.ToString();

            var expression4 = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, true, true, f => f.Name).FirstOrDefault();
            var debug4 = expression4.ToString();
            var test = @"A + (B + B + B) + (B + B + B) + C + C";
            Assert.IsTrue(test == debug4, "Test 2 - error");
        }
    }
}
