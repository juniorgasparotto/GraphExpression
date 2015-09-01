using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using EntityGraph;

namespace Graph.Tests
{
    [TestClass]
    public class TestExpression
    {
        [TestMethod]
        public void TestExpressionDescendents()
        {
            var configuration = new GraphConfiguration<HierarchicalEntity>
            (
                assignEdgeWeightCallback: (current, parent) => 1,
                entityToStringCallback: f => f.ToString()
            );

            var expressionIn = "A+B+(C+B+(D+(J+I)+P)+I)";
            var entities = Utils.FromExpression(expressionIn);
            var graph = entities.ToGraphs(f => f.Children, configuration);
            var expression = graph.ElementAt(0).Expression;
            var result = expression.DescendantsOf(entities.GetByIdentity("A"), 0);
            result = expression.DescendantsOf(entities.GetByIdentity("A"), 1);
            result = expression.DescendantsOf(entities.GetByIdentity("A"), 2);
            result = expression.DescendantsOf(entities.GetByIdentity("A"), 3);
            result = expression.DescendantsOf(entities.GetByIdentity("A"), 4);
            result = expression.DescendantsOf(entities.GetByIdentity("A"), 5);
        }

        [TestMethod]
        public void TestExpressionMultiple()
        {
            var configWithNotStartParenthesis = new GraphConfiguration<HierarchicalEntity>
            (
                assignEdgeWeightCallback: (current, parent) => 1,
                entityToStringCallback: f => f.ToString()
            );

            var configWithStartParenthesis = new GraphConfiguration<HierarchicalEntity>
            (
                assignEdgeWeightCallback: (current, parent) => 1,
                entityToStringCallback: f => f.ToString()
            );

            var expressionIn = "A+B+(C+B+(D+(J+I)+P)+I)";
            var expressionOut = "A+B+(C+B+(D+(J+I)+P)+I)";
            var descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B,C,D,J,I,P", Depths = new[] { "B,C", "B,C,D,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            descendentsTest.Add(new { EntityTest = "B", Items = "", Depths = default(string[]) });
            descendentsTest.Add(new { EntityTest = "C", Items = "B,D,J,I,P", Depths = new[] { "B,I", "B,D,P,I", "B,D,J,I,P" } });
            descendentsTest.Add(new { EntityTest = "D", Items = "J,I,P", Depths = new[] { "P", "J,I,P", "I" } });
            descendentsTest.Add(new { EntityTest = "J", Items = "I", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            descendentsTest.Add(new { EntityTest = "I", Items = "", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            descendentsTest.Add(new { EntityTest = "P", Items = "", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });

            var ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A,C", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            ancestorsTest.Add(new { EntityTest = "C", Items = "A", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            ancestorsTest.Add(new { EntityTest = "D", Items = "C,A", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            ancestorsTest.Add(new { EntityTest = "J", Items = "D,C,A", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            ancestorsTest.Add(new { EntityTest = "I", Items = "J,D,C,A", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });
            ancestorsTest.Add(new { EntityTest = "P", Items = "D,C,A", Depths = new[] { "B,I", "B,C,I", "B,C,D,P,I", "B,C,D,J,I,P" } });

            this.ExecuteTestAuto(configWithNotStartParenthesis, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+(B+C)";
            expressionOut = "A+(B+C)";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B,C" });
            descendentsTest.Add(new { EntityTest = "B", Items = "C" });
            descendentsTest.Add(new { EntityTest = "C", Items = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A" });
            ancestorsTest.Add(new { EntityTest = "C", Items = "B,A" });

            this.ExecuteTestAuto(configWithNotStartParenthesis, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A";
            expressionOut = "A";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "" });

            this.ExecuteTestAuto(configWithNotStartParenthesis, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+B";
            expressionOut = "A+B";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B" });
            descendentsTest.Add(new { EntityTest = "B", Items = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A" });

            this.ExecuteTestAuto(configWithNotStartParenthesis, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+(B+C+(D+(E+(G+H+A)+K)+F)+I)+M+A";
            expressionOut = "A+(B+C+(D+(E+(G+H+A)+K)+F)+I)+M+A";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B,C,D,E,G,H,A,K,F,I,M" });
            descendentsTest.Add(new { EntityTest = "B", Items = "C,D,E,G,H,A,K,F,I" });
            descendentsTest.Add(new { EntityTest = "C", Items = "" });
            descendentsTest.Add(new { EntityTest = "D", Items = "E,G,H,A,K,F" });
            descendentsTest.Add(new { EntityTest = "E", Items = "G,H,A,K" });
            descendentsTest.Add(new { EntityTest = "G", Items = "H,A" });
            descendentsTest.Add(new { EntityTest = "H", Items = "" });
            descendentsTest.Add(new { EntityTest = "K", Items = "" });
            descendentsTest.Add(new { EntityTest = "F", Items = "" });
            descendentsTest.Add(new { EntityTest = "I", Items = "" });
            descendentsTest.Add(new { EntityTest = "M", Items = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "G,E,D,B,A" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A" });
            ancestorsTest.Add(new { EntityTest = "C", Items = "B,A" });
            ancestorsTest.Add(new { EntityTest = "D", Items = "B,A" });
            ancestorsTest.Add(new { EntityTest = "E", Items = "D,B,A" });
            ancestorsTest.Add(new { EntityTest = "G", Items = "E,D,B,A" });
            ancestorsTest.Add(new { EntityTest = "H", Items = "G,E,D,B,A" });
            ancestorsTest.Add(new { EntityTest = "K", Items = "E,D,B,A" });
            ancestorsTest.Add(new { EntityTest = "F", Items = "D,B,A" });
            ancestorsTest.Add(new { EntityTest = "I", Items = "B,A" });
            ancestorsTest.Add(new { EntityTest = "M", Items = "A" });

            this.ExecuteTestAuto(configWithNotStartParenthesis, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            // 
            // "A+B", "C+B"
            // A+(B+Q+(C+H))+(G+H)
        }

        public void ExecuteTestAuto
        (
            GraphConfiguration<HierarchicalEntity> configuration,
            string expressionIn,
            string expressionOut,
            List<dynamic> descendentsTest,
            List<dynamic> ancestorsTest
        )
        {
            var entities = Utils.FromExpression(expressionIn);
            var graph = entities.ToGraphs(f => f.Children, configuration);
            var expression = graph.ElementAt(0).Expression;
            
            var expressionTests = new List<dynamic>();
            var level = 1;
            for(var i = 0; i < expressionOut.Length; i++)
            {
                var previous = default(string);
                var next = default(string);
                var current = expressionOut[i].ToString();

                if (i + 1 < expressionOut.Length)
                    next = expressionOut[i + 1].ToString();

                if (i - 1 >= 0)
                    previous = expressionOut[i - 1].ToString();

                if (current == "(" && i > 0)
                    level++;

                var dynTest = new
                {
                    Name = current,
                    Index = i,
                    Level = (expressionTests.Count > 0 && (current == "(" || current == ")" || previous == "(")) ? level - 1 : level,
                    LevelInExpression = level,
                    Previous = previous,
                    Next = next
                };

                if (current == ")")
                    level--;

                expressionTests.Add(dynTest);
            }

            var debug1 = "";
            var debug2 = "";
            var debug3 = "";
            foreach(var itemDebug in expressionTests)
            {
                debug1 += itemDebug.Name + " ";
                debug2 += itemDebug.LevelInExpression + " ";
                debug3 += itemDebug.Level + " ";
            }

            var outputDebugTest = debug1 + "\r\n" + debug2 + "\r\n" + debug3;
            var outputDebugExpression = expression.ToDebug();

            Assert.IsTrue(outputDebugTest == outputDebugExpression, "Test ToDebug function");
            Assert.IsTrue(debug1.Replace(" ", "") == expression.ToString().Replace(" ", ""), "Test ToString function");

            foreach (var test in expressionTests)
            {
                Assert.IsTrue(test.Name == expression[test.Index].ToString().Trim(), test.Index.ToString());
                Assert.IsTrue(test.Level == expression[test.Index].Level, test.Index.ToString());
                Assert.IsTrue(test.Previous == (expression[test.Index].Previous == null ? null : expression[test.Index].Previous.ToString().Trim()), test.Index.ToString());
                Assert.IsTrue(test.Next == (expression[test.Index].Next == null ? null : expression[test.Index].Next.ToString().Trim()), test.Index.ToString());
            }

            for (var e = 0; e < entities.Count; e++)
            {
                var test = descendentsTest[e];
                var expressionItem = expression[e];

                //for (var depth = 0; depth < test.Depths.Length; depth++)
                //{
                    var descendants = expression.DescendantsOf(entities[e]).ToList();
                    var items = string.IsNullOrWhiteSpace(test.Items) ? new string[0] : test.Items.Split(',');

                    Assert.IsTrue(descendants.Count == items.Length, string.Format("Count of descendants not match for entity {0}.", test.EntityTest));

                    for (var i = 0; i < descendants.Count; i++)
                    {
                        Assert.IsTrue(descendants[i].ToString() == items[i].Trim(), string.Format("Items {0} and {1} not match.", test.EntityTest, descendants[i].ToString()));
                    }
                //}
            }

            for (var e = 0; e < entities.Count; e++)
            {
                var test = ancestorsTest[e];
                var descendants = expression.AncestorsOf(entities[e]).ToList();
                var items = string.IsNullOrWhiteSpace(test.Items) ? new string[0] : test.Items.Split(',');

                Assert.IsTrue(descendants.Count == items.Length, string.Format("Count of ancestors not match for entity {0}.", test.EntityTest));

                for (var i = 0; i < descendants.Count; i++)
                {
                    Assert.IsTrue(descendants[i].ToString() == items[i].Trim(), string.Format("Items {0} and {1} not match.", test.EntityTest, descendants[i].ToString()));
                }
            }
        }
    }
}
