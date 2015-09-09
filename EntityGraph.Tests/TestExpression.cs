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
        private GraphConfiguration<HierarchicalEntity> GetConfig()
        {
            return new GraphConfiguration<HierarchicalEntity>
            (
                assignEdgeWeightCallback: (current, parent) => 1,
                entityToStringCallback: f => f.ToString()
            );
        }

        private string ToStringExpressionItems(IEnumerable<ExpressionItem<HierarchicalEntity>> items)
        {
            var str = "";
            foreach (var t in items)
                str += str == "" ? t.ToString() : "," + t.ToString();
            return str;
        }

        private Expression<HierarchicalEntity> GetExpression(string expressionIn, out ListOfHierarchicalEntity entities)
        {
            entities = Utils.FromExpression(expressionIn);
            var graphs = entities.ToGraphs(f => f.Children, GetConfig());
            var graph = graphs.FirstOrDefault(f => f.Vertexes.ElementAt(0).ToString() == expressionIn[0].ToString());
            return graph.Expression;
        }

        [TestMethod]
        public void TestExpressionMixedFilters()
        {
            ListOfHierarchicalEntity entities;
            var expressionIn = "A+(B+C+(J+(I+O)))+K+(D+E+(P+(U+Y)))";
            var expression = GetExpression(expressionIn, out entities);

            var mixed = expression.Where(f => f.ToString() == "B" || f.ToString() == "D").Descendants().ToList();
            Assert.IsTrue(ToStringExpressionItems(mixed) == "C,J,I,O,E,P,U,Y", "Mixed Descendants - all");

            // with where
            mixed = expression.Where(f => f.ToString() == "B" || f.ToString() == "D").Descendants(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(mixed) == "C,J,I,E,P,U", "Mixed Descendants - only until level 2");

            // with find
            mixed = expression.Find(f => f.ToString() == "B" || f.ToString() == "D").Children().ToList();
            Assert.IsTrue(ToStringExpressionItems(mixed) == "C,J,E,P", "Mixed Children");

            mixed = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors().ToList();
            Assert.IsTrue(ToStringExpressionItems(mixed) == "J,B,A,P,D,A", "Mixed Ancestors - all");

            mixed = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Ancestors(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(mixed) == "J,B,P,D", "Mixed Ancestors - only level 2");

            mixed = expression.Find(f => f.ToString() == "I" || f.ToString() == "U").Parents().ToList();
            Assert.IsTrue(ToStringExpressionItems(mixed) == "J,P", "Mixed Parents");
        }

        [TestMethod]
        public void TestExpressionAncestorsDepthStartAndDepthEnd()
        {
            ListOfHierarchicalEntity entities;
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
            ListOfHierarchicalEntity entities;
            var expressionIn = "A+(B+C+(J+(I+(O+(R+T)))))+K+(D+E+(P+(U+(Y+(L+N)))))";
            var expression = GetExpression(expressionIn, out entities);

            List<ExpressionItem<HierarchicalEntity>> result;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> filter;

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
            ListOfHierarchicalEntity entities;
            var expressionIn = "A+(B+C+(J+(I+(O+(R+T)))))+K+(D+E+(P+(U+(Y+(L+N)))))";
            var expression = GetExpression(expressionIn, out entities);

            List<ExpressionItem<HierarchicalEntity>> result;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> stop;
            Func<ExpressionItem<HierarchicalEntity>, int, bool> filter;

            stop = (ancestor, depthAncestor) => ancestor.ToString() == "J" || ancestor.ToString() == "P";
            result = expression.Find(f => f.ToString() == "T" || f.ToString() == "N").AncestorsUntil(stop).ToList();

            Assert.IsTrue(ToStringExpressionItems(result) == "R,O,I,J,L,Y,U,P", "Test: return items until 'stop delegate' return false");
        }

        [TestMethod]
        public void TestExpressionDescendentsWithDepth()
        {
            ListOfHierarchicalEntity entities;
            var expression = GetExpression("A+(B+C+(J+I))+K+(D+E+(P+U))", out entities);

            var debug = expression.ToDebug();
            var result = expression.Find(entities.GetByIdentity("A")).Descendants(0).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,C,J,I,K,D,E,P,U", "level 0");
            result = expression.Find(entities.GetByIdentity("A")).Descendants(1).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,K,D", "level 1");
            result = expression.Find(entities.GetByIdentity("A")).Descendants(2).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,C,J,K,D,E,P", "level 2");
            result = expression.Find(entities.GetByIdentity("A")).Descendants(3).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,C,J,I,K,D,E,P,U", "level 3");
            result = expression.Find(entities.GetByIdentity("A")).Descendants(4).ToList();
            Assert.IsTrue(ToStringExpressionItems(result) == "B,C,J,I,K,D,E,P,U", "level 4");
        }

        [TestMethod]
        public void TestExpressionAncestorsWithDepth()
        {
            ListOfHierarchicalEntity entities;
            var expression = GetExpression("A+(B+C+(J+I))+K+(D+E+(P+U))", out entities);

            var debug = expression.ToDebug();
            List<ExpressionItem<HierarchicalEntity>> result;

            try
            { 
                result = expression.Find(entities.GetByIdentity("I")).Ancestors(0).ToList();
            }
            catch(Exception ex)
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

        [TestMethod]
        public void TestExpressionMultiple()
        {
            var config = GetConfig();
            var iTest = 0;

            var expressionIn = "A+(B+C+(J+I))+(D+B)";
            var expressionOut = "A+(B+C+(J+I))+(D+(B+C+(J+I)))";
            var descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B,C,J,I,D", Children = "B,D" });
            descendentsTest.Add(new { EntityTest = "B", Items = "C,J,I", Children = "C,J" });
            descendentsTest.Add(new { EntityTest = "C", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "J", Items = "I", Children = "I" });
            descendentsTest.Add(new { EntityTest = "I", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "D", Items = "B,C,J,I", Children = "B" });

            var ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Parents = "" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A,D", Parents = "A,D" });
            ancestorsTest.Add(new { EntityTest = "C", Items = "B,A,D", Parents = "B" });
            ancestorsTest.Add(new { EntityTest = "J", Items = "B,A,D", Parents = "B" });
            ancestorsTest.Add(new { EntityTest = "I", Items = "J,B,A,D", Parents = "J" });
            ancestorsTest.Add(new { EntityTest = "D", Items = "A", Parents = "A" });

            this.ExecuteTestAuto(iTest++, config, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+(B+C+(J+I))+K+(D+E+(P+U))";
            expressionOut = "A+(B+C+(J+I))+K+(D+E+(P+U))";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B,C,J,I,K,D,E,P,U", Children = "B,K,D" });
            descendentsTest.Add(new { EntityTest = "B", Items = "C,J,I", Children = "C,J" });
            descendentsTest.Add(new { EntityTest = "C", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "J", Items = "I", Children = "I" });
            descendentsTest.Add(new { EntityTest = "I", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "K", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "D", Items = "E,P,U", Children = "E,P" });
            descendentsTest.Add(new { EntityTest = "E", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "P", Items = "U", Children = "U" });
            descendentsTest.Add(new { EntityTest = "U", Items = "", Children = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Parents = "" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A", Parents = "A" });
            ancestorsTest.Add(new { EntityTest = "C", Items = "B,A", Parents = "B" });
            ancestorsTest.Add(new { EntityTest = "J", Items = "B,A", Parents = "B" });
            ancestorsTest.Add(new { EntityTest = "I", Items = "J,B,A", Parents = "J" });
            ancestorsTest.Add(new { EntityTest = "K", Items = "A", Parents = "A" });
            ancestorsTest.Add(new { EntityTest = "D", Items = "A", Parents = "A" });
            ancestorsTest.Add(new { EntityTest = "E", Items = "D,A", Parents = "D" });
            ancestorsTest.Add(new { EntityTest = "P", Items = "D,A", Parents = "D" });
            ancestorsTest.Add(new { EntityTest = "U", Items = "P,D,A", Parents = "P" });

            this.ExecuteTestAuto(iTest++, config, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+B+(C+B+(D+(J+I)+P)+I)";
            expressionOut = "A+B+(C+B+(D+(J+I)+P)+I)";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B,C,D,J,I,P", Children = "B,C"});
            descendentsTest.Add(new { EntityTest = "B", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "C", Items = "B,D,J,I,P", Children = "B,D,I" });
            descendentsTest.Add(new { EntityTest = "D", Items = "J,I,P", Children = "J,P" });
            descendentsTest.Add(new { EntityTest = "J", Items = "I", Children = "I" });
            descendentsTest.Add(new { EntityTest = "I", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "P", Items = "", Children = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Parents = "" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A,C", Parents = "A,C" });
            ancestorsTest.Add(new { EntityTest = "C", Items = "A", Parents = "A" });
            ancestorsTest.Add(new { EntityTest = "D", Items = "C,A", Parents = "C" });
            ancestorsTest.Add(new { EntityTest = "J", Items = "D,C,A", Parents = "D" });
            ancestorsTest.Add(new { EntityTest = "I", Items = "J,D,C,A", Parents = "J,C" });
            ancestorsTest.Add(new { EntityTest = "P", Items = "D,C,A", Parents = "D" });

            this.ExecuteTestAuto(iTest++, config, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+(B+C)";
            expressionOut = "A+(B+C)";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B,C", Children = "B" });
            descendentsTest.Add(new { EntityTest = "B", Items = "C", Children = "C" });
            descendentsTest.Add(new { EntityTest = "C", Items = "", Children = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Parents = "" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A", Parents = "A" });
            ancestorsTest.Add(new { EntityTest = "C", Items = "B,A", Parents = "B" });

            this.ExecuteTestAuto(iTest++, config, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A";
            expressionOut = "A";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "", Children = ""  });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Parents = "" });

            this.ExecuteTestAuto(iTest++, config, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+B";
            expressionOut = "A+B";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B", Children = "B" });
            descendentsTest.Add(new { EntityTest = "B", Items = "", Children = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Parents = "" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A", Parents = "A" });

            this.ExecuteTestAuto(iTest++, config, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+(B+C+(D+(E+(G+H+A)+K)+F)+I)+M+A";
            expressionOut = "A+(B+C+(D+(E+(G+H+A)+K)+F)+I)+M+A";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B,C,D,E,G,H,A,K,F,I,M", Children = "B,M,A" });
            descendentsTest.Add(new { EntityTest = "B", Items = "C,D,E,G,H,A,K,F,I", Children = "C,D,I" });
            descendentsTest.Add(new { EntityTest = "C", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "D", Items = "E,G,H,A,K,F", Children = "E,F" });
            descendentsTest.Add(new { EntityTest = "E", Items = "G,H,A,K", Children = "G,K" });
            descendentsTest.Add(new { EntityTest = "G", Items = "H,A", Children = "H,A" });
            descendentsTest.Add(new { EntityTest = "H", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "K", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "F", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "I", Items = "", Children = "" });
            descendentsTest.Add(new { EntityTest = "M", Items = "", Children = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "G,E,D,B,A", Parents = "G,A" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A", Parents = "A" });
            ancestorsTest.Add(new { EntityTest = "C", Items = "B,A", Parents = "B" });
            ancestorsTest.Add(new { EntityTest = "D", Items = "B,A", Parents = "B" });
            ancestorsTest.Add(new { EntityTest = "E", Items = "D,B,A", Parents = "D" });
            ancestorsTest.Add(new { EntityTest = "G", Items = "E,D,B,A", Parents = "E" });
            ancestorsTest.Add(new { EntityTest = "H", Items = "G,E,D,B,A", Parents = "G" });
            ancestorsTest.Add(new { EntityTest = "K", Items = "E,D,B,A", Parents = "E" });
            ancestorsTest.Add(new { EntityTest = "F", Items = "D,B,A", Parents = "D" });
            ancestorsTest.Add(new { EntityTest = "I", Items = "B,A", Parents = "B" });
            ancestorsTest.Add(new { EntityTest = "M", Items = "A", Parents = "A" });

            this.ExecuteTestAuto(iTest++, config, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+A";
            expressionOut = "A+A";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "A", Children = "A" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "A", Parents = "A" });

            this.ExecuteTestAuto(iTest++, config, expressionIn, expressionOut, descendentsTest, ancestorsTest);
        }

        public void ExecuteTestAuto
        (
            int iTest,
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
            var testNumberDesc = "Test " + iTest.ToString() + ": ";

            var expressionTests = new List<dynamic>();
            var level = 1;
            var levelInExpression = 1;

            var parent = "-";
            var parentOrder = new Dictionary<int, string>();
            
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
                { 
                    levelInExpression++;
                    level++;
                }

                if (i == 1)
                {
                    parentOrder.Add(level, expressionOut[0].ToString());
                    parent = expressionOut[0].ToString();
                }
                
                if (current == ")")
                {
                    parent = parentOrder[level-1];
                }

                var dynTest = new
                {
                    Name = current,
                    Index = i,
                    Level = (expressionTests.Count > 0 && (current == "(" || current == ")" || previous == "(")) ? level - 1 : level,
                    LevelInExpression = levelInExpression,
                    Previous = previous,
                    Next = next,
                    Parent = parent,
                    Root = expressionOut[0].ToString()
                };

                if (current == ")")
                { 
                    levelInExpression--;
                    level--;
                }

                if (previous == "(")
                {
                    if (parentOrder.ContainsKey(level))
                        parentOrder[level] = current;
                    else
                        parentOrder.Add(level, current);

                    parent = current;
                }

                if (i == 0)
                {
                    level++;
                }

                expressionTests.Add(dynTest);
            }

            var debug1 = "";
            var debug2 = "";
            var debug3 = "";
            var debug4 = "";
            var debug5 = "";
            foreach (var itemDebug in expressionTests)
            {
                debug1 += itemDebug.Name + " ";
                debug2 += itemDebug.LevelInExpression + " ";
                debug3 += itemDebug.Level + " ";
                debug4 += itemDebug.Parent + " ";
                debug5 += itemDebug.Root + " ";
            }

            var outputDebugTest = debug1 + "\r\n" + debug2 + "\r\n" + debug3 + "\r\n" + debug4 + "\r\n" + debug5;
            var outputDebugExpression = expression.ToDebug();

            Assert.IsTrue(outputDebugTest == outputDebugExpression, testNumberDesc + "ToDebug function test");
            Assert.IsTrue(debug1.Replace(" ", "") == expression.ToString().Replace(" ", ""), "ToString function test");

            foreach (var test in expressionTests)
            {
                var debugIndex = string.Format("Expected Index[{0}] == Index[{1}]; ", test.Index, expression[test.Index].Index);
                var debugName = string.Format("Expected Name[{0}] == Name[{1}]; ", test.Name, expression[test.Index].ToString().Trim());
                var debugParent = string.Format("Expected Parent[{0}] == Parent[{1}]; ", test.Parent, (expression[test.Index].Parent == null ? "" : expression[test.Index].Parent.ToString().Trim()));
                var debugRoot = string.Format("Expected Root[{0}] == Root[{1}]; ", test.Root, expression[test.Index].Root.ToString().Trim());
                var debugLevel = string.Format("Expected Level[{0}] == Level[{1}]; ", test.Level, expression[test.Index].Level);
                var debugLevelExpression = string.Format("Expected LevelExpression[{0}] == LevelExpression[{1}]; ", test.LevelInExpression, expression[test.Index].LevelInExpression);

                Assert.IsTrue(test.Index == expression[test.Index].Index, testNumberDesc + debugIndex);
                Assert.IsTrue(test.Name == expression[test.Index].ToString().Trim(), testNumberDesc + debugName);
                Assert.IsTrue(test.Level == expression[test.Index].Level, testNumberDesc + ":" + debugLevel);
                Assert.IsTrue(test.LevelInExpression == expression[test.Index].LevelInExpression, testNumberDesc + debugLevelExpression);
                Assert.IsTrue(test.Parent == (expression[test.Index].Parent == null ? "-" : expression[test.Index].Parent.ToString().Trim()), testNumberDesc + ":" + debugParent);
                Assert.IsTrue(test.Root == expression[test.Index].Root.ToString().Trim(), "Output test:" + debugRoot);
                Assert.IsTrue(test.Previous == (expression[test.Index].Previous == null ? null : expression[test.Index].Previous.ToString().Trim()), testNumberDesc + ":" + debugName);
                Assert.IsTrue(test.Next == (expression[test.Index].Next == null ? null : expression[test.Index].Next.ToString().Trim()), testNumberDesc + debugName);
            }

            for (var e = 0; e < entities.Count; e++)
            {
                var test = descendentsTest[e];
                var descendants = expression.Find(entities[e]).Descendants().ToEntities().ToList();
                var descendantsExpected = string.IsNullOrWhiteSpace(test.Items) ? new string[0] : test.Items.Split(',');

                var children = expression.Find(entities[e]).Children().ToEntities().ToList();
                var childrenExpected = string.IsNullOrWhiteSpace(test.Children) ? new string[0] : test.Children.Split(',');

                Assert.IsTrue(descendants.Count == descendantsExpected.Length, testNumberDesc + string.Format("Descendants of '{0}': Count of descendants not match for entity.", test.EntityTest));
                for (var i = 0; i < descendants.Count; i++)
                    Assert.IsTrue(descendants[i].ToString() == descendantsExpected[i].Trim(), testNumberDesc + string.Format("Descendants of '{0}': Item {1} and {2} not match.", test.EntityTest, descendants[i].ToString(), descendantsExpected[i].Trim()));

                Assert.IsTrue(children.Count == childrenExpected.Length, testNumberDesc + string.Format("Children of '{0}': Count of children not match for entity.", test.EntityTest));
                for (var i = 0; i < children.Count; i++)
                    Assert.IsTrue(children[i].ToString() == childrenExpected[i].Trim(), testNumberDesc + string.Format("Children of '{0}': Item {1} and {2} not match.", test.EntityTest, children[i].ToString(), childrenExpected[i].Trim()));
            }

            for (var e = 0; e < entities.Count; e++)
            {
                var test = ancestorsTest[e];
                var ancestors = expression.Find(entities[e]).Ancestors().ToEntities().ToList();
                var ancestorsExpected = string.IsNullOrWhiteSpace(test.Items) ? new string[0] : test.Items.Split(',');
                var parents = expression.Find(entities[e]).Parents().ToEntities().ToList();
                var parentsExpected = string.IsNullOrWhiteSpace(test.Parents) ? new string[0] : test.Parents.Split(',');

                Assert.IsTrue(ancestors.Count == ancestorsExpected.Length, testNumberDesc + string.Format("Ancestors of '{0}': Count of ancestors not match.", test.EntityTest));
                for (var i = 0; i < ancestors.Count; i++)
                    Assert.IsTrue(ancestors[i].ToString() == ancestorsExpected[i].Trim(), testNumberDesc + string.Format("Ancestors of '{0}': Item {1} and {2} not match.", test.EntityTest, ancestors[i].ToString(), ancestorsExpected[i].Trim()));

                Assert.IsTrue(parents.Count == parentsExpected.Length, testNumberDesc + string.Format("Parents of '{0}': Count of ancestors not match.", test.EntityTest));
                for (var i = 0; i < parents.Count; i++)
                    Assert.IsTrue(parents[i].ToString() == parentsExpected[i].Trim(), testNumberDesc + string.Format("Parents of '{0}': Item {1} and {2} not match.", test.EntityTest, parents[i].ToString(), parentsExpected[i].Trim()));
            }
        }
    }
}
