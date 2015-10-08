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
            b.Items.Add(c);
            b.Items.Add(b);
            b.Items.Add(b);
            b.Items.Add(c);
            a.Items.Add(b);
            a.Items.Add(c);
            a.Items.Add(c);

            List<dynamic> entities = new List<dynamic>();
            entities.Add(a);

            var expression1 = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, false, false, f => f.Name).FirstOrDefault();
            Assert.IsTrue("A + (B + C + B + B + C) + (B + C + B + B + C) + C + C" == expression1.ToString(), "Test 12");

            var expression2 = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, true, false, f => f.Name).FirstOrDefault();
            Assert.IsTrue("A + (B + C + B + B + C) + (B + C + B + B + C) + C + C" == expression2.ToString(), "Test 13");

            var expression3 = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, false, true, f => f.Name).FirstOrDefault();
            Assert.IsTrue("A + (B + C + B + B + C) + (B + C + B + B + C) + C + C" == expression3.ToString(), "Test 14");

            var expression4 = ExpressionBuilder<dynamic>.Build(entities, f => f.Items, true, true, f => f.Name).FirstOrDefault();
            Assert.IsTrue("A + (B + C + B + B + C) + (B + C + B + B + C) + C + C" == expression4.ToString(), "Test 15");
        }
    }
}
