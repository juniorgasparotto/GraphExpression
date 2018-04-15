using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Tests
{
    public partial class TestExpression
    {
        //private GraphConfiguration<HierarchicalEntity> GetConfig()
        //{
        //    return new GraphConfiguration<HierarchicalEntity>
        //    (
        //        assignEdgeWeightCallback: (current, parent) => 1,
        //        entityToStringCallback: f => f.ToString()
        //    );
        //}

        private string ToStringExpressionItems(IEnumerable<ExpressionItem<HierarchicalEntity>> items)
        {
            var str = "";
            foreach (var t in items)
                str += str == "" ? t.ToString() : "," + t.ToString();
            return str;
        }

        private Expression<HierarchicalEntity> GetExpression(string expressionIn, out IEnumerable<HierarchicalEntity> entities, bool usePlus = true, bool useParenthesis = true, bool awaysRepeatDefined = true)
        {
            entities = ExpressionUtils.FromString(expressionIn);
            return ExpressionBuilder<HierarchicalEntity>.Build(entities, f => f.Children, true, usePlus, useParenthesis).FirstOrDefault();

            //entities = Utils.FromExpression(expressionIn);
            //var graphs = entities.ToGraphs(f => f.Children, GetConfig());
            //var graph = graphs.FirstOrDefault(f => f.Vertexes.ElementAt(0).ToString() == expressionIn[0].ToString());
            //return graph.Expression;
        }

        [TestMethod]
        public void TestExpressionMixedFilters()
        {
            IEnumerable<HierarchicalEntity> entities;
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
        public void TestExpressionBuilderToString()
        {
            var debug = "";
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+C))"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + C))" == debug, "Test 1");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+C)+D)"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + C) + D)" == debug, "Test 2");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+C)+C)"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + C) + (C + C))" == debug, "Test 3");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+C))+D"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + C)) + D" == debug, "Test 4");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+C))+C"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + C)) + (C + C)" == debug, "Test 5");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+(D+C)))+C"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + (D + C))) + (C + (D + C))" == debug, "Test 6");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+(D+C))+I)+C"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + (D + C)) + I) + (C + (D + C))" == debug, "Test 7");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+(D+C)+P)+I)+C"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + (D + C) + P) + I) + (C + (D + C) + P)" == debug, "Test 8");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+(D+C)+P)+I+P)+C"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + (D + C) + P) + I + P) + (C + (D + C) + P)" == debug, "Test 9");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+(D+C)+P+G)+I+P)+C"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + (D + C) + P + G) + I + P) + (C + (D + C) + P + G)" == debug, "Test 10");
            debug = ExpressionBuilder<HierarchicalEntity>.Build(ExpressionUtils.FromString("A+(B+(C+(D+C)+P+G))+C"), f => f.Children, true, true, true, f => f.Entity.Identity.ToString()).FirstOrDefault().ToString();
            Assert.IsTrue("A + (B + (C + (D + C) + P + G)) + (C + (D + C) + P + G)" == debug, "Test 11");
        }

        [TestMethod]
        public void TestExpressionRecursion()
        {
            IEnumerable<HierarchicalEntity> entities;

            var debug = GetExpression("A+(B+(C+C)+B)+A", out entities).ToString();
            var test = @"A + (B + (C + C) + B) + A";
            Assert.IsTrue(test == debug, "Test 0 - error");
           
            debug = GetExpression("A+A", out entities).ToString();
            test = @"A + A";
            Assert.IsTrue(test == debug, "Test 1 - error");

            debug = GetExpression("A+(B+B+A)", out entities).ToString();
            test = @"A + (B + B + A)";
            Assert.IsTrue(test == debug, "Test 2 - error");

            debug = GetExpression("A+(A+B)", out entities).ToString();
            test = @"A + B + A";
            Assert.IsTrue(test == debug, "Test 3 - error");

            debug = GetExpression("A+(B+C)+A+A+F", out entities).ToString();
            test = @"A + (B + C) + A + A + F";
            Assert.IsTrue(test == debug, "Test 4 - error");

            debug = GetExpression("A+(B+C)+A+V+F", out entities).ToString();
            test = @"A + (B + C) + A + V + F";
            Assert.IsTrue(test == debug, "Test 5 - error");

            debug = GetExpression("A+(B+K+(C+B+P))+A+V+F", out entities).ToString();
            test = @"A + (B + K + (C + B + P)) + A + V + F";
            Assert.IsTrue(test == debug, "Test 6 - error");

            debug = GetExpression("A+(B+C+D)+D+(E+B)+F+G(G+G+C)+(H+C)", out entities).ToString();
            test = @"A + (B + C + D) + D + (E + (B + C + D)) + F + (G + G + C) + (H + C)";
            Assert.IsTrue(test == debug, "Test 7 - error");
            
            debug = GetExpression("A+B+A", out entities).ToString();
            test = @"A + B + A";
            Assert.IsTrue(test == debug, "Test 8 - error");

            debug = GetExpression("A+(B+A)", out entities).ToString();
            test = @"A + (B + A)";
            Assert.IsTrue(test == debug, "Test 9 - error");

            debug = GetExpression("A+(B+A+(B+A))", out entities).ToString();
            test = @"A + (B + A + A + B)";
            Assert.IsTrue(test == debug, "Test 10 - error");

            debug = GetExpression("A+(B+A)+B+A", out entities).ToString();
            test = @"A + (B + A) + (B + A) + A";
            Assert.IsTrue(test == debug, "Test 11 - error");

            debug = GetExpression("A+(B+A)+A+B", out entities).ToString();
            test = @"A + (B + A) + A + (B + A)";
            Assert.IsTrue(test == debug, "Test 12 - error");

            debug = GetExpression("A+(B+A)+A+(C+B)", out entities).ToString();
            test = @"A + (B + A) + A + (C + (B + A))";
            Assert.IsTrue(test == debug, "Test 13 - error");

            debug = GetExpression("A+(B+A)+(I+A+D+B+D)+(C+B)", out entities).ToString();
            test = @"A + (B + A) + (I + A + D + (B + A) + D) + (C + (B + A))";
            Assert.IsTrue(test == debug, "Test 14 - error");
        }

        [TestMethod]
        public void TestExpressionWithoutRepeat()
        {
            return;
            var debug = GetExpression("A+(B+(C+C)+B)+A", out IEnumerable<HierarchicalEntity> entities, true, true, false).ToString();
            Assert.IsTrue("" == debug);

            debug = GetExpression("A+(B+C+(J+I))+(D+(B+C+(J+I)))", out entities, true, true, false).ToString();
            Assert.IsTrue("" == debug);

            debug = GetExpression("", out entities, true, true, false).ToString();
            Assert.IsTrue("" == debug);

            debug = GetExpression("", out entities, true, true, false).ToString();
            Assert.IsTrue("" == debug);

            debug = GetExpression("", out entities, true, true, false).ToString();
            Assert.IsTrue("" == debug);

            debug = GetExpression("", out entities, true, true, false).ToString();
            Assert.IsTrue("" == debug);

            debug = GetExpression("", out entities, true, true, false).ToString();
            Assert.IsTrue("" == debug);

            debug = GetExpression("", out entities, true, true, false).ToString();
            Assert.IsTrue("" == debug);
        }

        [TestMethod]
        public void TestExpressionMultiple()
        {
            var iTest = 1;

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

            // 1
            this.ExecuteTestAuto(iTest++, expressionIn, expressionOut, descendentsTest, ancestorsTest);

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

            // 2
            this.ExecuteTestAuto(iTest++, expressionIn, expressionOut, descendentsTest, ancestorsTest);

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

            // 3
            this.ExecuteTestAuto(iTest++, expressionIn, expressionOut, descendentsTest, ancestorsTest);

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

            // 4
            this.ExecuteTestAuto(iTest++, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A";
            expressionOut = "A";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "", Children = ""  });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Parents = "" });

            // 5
            this.ExecuteTestAuto(iTest++, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+B";
            expressionOut = "A+B";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "B", Children = "B" });
            descendentsTest.Add(new { EntityTest = "B", Items = "", Children = "" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "", Parents = "" });
            ancestorsTest.Add(new { EntityTest = "B", Items = "A", Parents = "A" });

            // 6
            this.ExecuteTestAuto(iTest++, expressionIn, expressionOut, descendentsTest, ancestorsTest);

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

            // 7
            this.ExecuteTestAuto(iTest++, expressionIn, expressionOut, descendentsTest, ancestorsTest);

            //////////////////////////

            expressionIn = "A+A";
            expressionOut = "A+A";
            descendentsTest = new List<dynamic>();
            descendentsTest.Add(new { EntityTest = "A", Items = "A", Children = "A" });

            ancestorsTest = new List<dynamic>();
            ancestorsTest.Add(new { EntityTest = "A", Items = "A", Parents = "A" });

            // 8
            this.ExecuteTestAuto(iTest++, expressionIn, expressionOut, descendentsTest, ancestorsTest);
        }

        public void ExecuteTestAuto
        (
            int iTest,
            string expressionIn,
            string expressionOut,
            List<dynamic> descendentsTest,
            List<dynamic> ancestorsTest
        )
        {
            dynamic combinations = new[]
            {
                new { usePlus = true, useParenthesis = true },
                new { usePlus = false, useParenthesis = false },
                new { usePlus = true, useParenthesis = false },
                new { usePlus = false, useParenthesis = true }
            };

            foreach(var comb in combinations)
            {
                IEnumerable<HierarchicalEntity> entities;
                Expression<HierarchicalEntity> expression = this.GetExpression(expressionIn, out entities, comb.usePlus, comb.useParenthesis);
                var testNumberDesc = "Test " + iTest.ToString() + ": ";

                //var expressionTests = new List<dynamic>();
                //var level = 1;
                //var levelInExpression = 1;

                //var parent = "-";
                //var parentOrder = new Dictionary<int, string>();
            
                //for(var i = 0; i < expressionOut.Length; i++)
                //{
                //    var previous = default(string);
                //    var next = default(string);
                //    var current = expressionOut[i].ToString();

                //    if (i + 1 < expressionOut.Length)
                //        next = expressionOut[i + 1].ToString();

                //    if (i - 1 >= 0)
                //        previous = expressionOut[i - 1].ToString();

                //    if (current == "(" && i > 0)
                //    { 
                //        levelInExpression++;
                //        level++;
                //    }

                //    if (i == 1)
                //    {
                //        parentOrder.Add(level, expressionOut[0].ToString());
                //        parent = expressionOut[0].ToString();
                //    }
                
                //    if (current == ")")
                //    {
                //        parent = parentOrder[level-1];
                //    }

                //    var dynTest = new
                //    {
                //        Name = current,
                //        Index = i,
                //        Level = (expressionTests.Count > 0 && (current == "(" || current == ")" || previous == "(")) ? level - 1 : level,
                //        LevelInExpression = levelInExpression,
                //        Previous = previous,
                //        Next = next,
                //        Parent = parent,
                //        Root = expressionOut[0].ToString()
                //    };

                //    if (current == ")")
                //    { 
                //        levelInExpression--;
                //        level--;
                //    }

                //    if (previous == "(")
                //    {
                //        if (parentOrder.ContainsKey(level))
                //            parentOrder[level] = current;
                //        else
                //            parentOrder.Add(level, current);

                //        parent = current;
                //    }

                //    if (i == 0)
                //    {
                //        level++;
                //    }

                //    expressionTests.Add(dynTest);
                //}

                //var debug1 = "";
                //var debug2 = "";
                //var debug3 = "";
                //var debug4 = "";
                //var debug5 = "";
                //foreach (var itemDebug in expressionTests)
                //{
                //    debug1 += itemDebug.Name + " ";
                //    debug2 += itemDebug.LevelInExpression + " ";
                //    debug3 += itemDebug.Level + " ";
                //    debug4 += itemDebug.Parent + " ";
                //    debug5 += itemDebug.Root + " ";
                //}

                //var outputDebugTest = debug1 + "\r\n" + debug2 + "\r\n" + debug3 + "\r\n" + debug4 + "\r\n" + debug5;
                //var outputDebugExpression = expression.ToDebug();

                //Assert.IsTrue(outputDebugTest == outputDebugExpression, testNumberDesc + "ToDebug function test");
                //Assert.IsTrue(debug1.Replace(" ", "") == expression.ToString().Replace(" ", ""), "ToString function test");

                //foreach (var test in expressionTests)
                //{
                //    var debugIndex = string.Format("Expected Index[{0}] == Index[{1}]; ", test.Index, expression[test.Index].Index);
                //    var debugName = string.Format("Expected Name[{0}] == Name[{1}]; ", test.Name, expression[test.Index].ToString().Trim());
                //    var debugParent = string.Format("Expected Parent[{0}] == Parent[{1}]; ", test.Parent, (expression[test.Index].Parent == null ? "" : expression[test.Index].Parent.ToString().Trim()));
                //    var debugRoot = string.Format("Expected Root[{0}] == Root[{1}]; ", test.Root, expression[test.Index].Root.ToString().Trim());
                //    var debugLevel = string.Format("Expected Level[{0}] == Level[{1}]; ", test.Level, expression[test.Index].Level);
                //    var debugLevelExpression = string.Format("Expected LevelExpression[{0}] == LevelExpression[{1}]; ", test.LevelInExpression, expression[test.Index].LevelInExpression);

                //    Assert.IsTrue(test.Index == expression[test.Index].Index, testNumberDesc + debugIndex);
                //    Assert.IsTrue(test.Name == expression[test.Index].ToString().Trim(), testNumberDesc + debugName);
                //    Assert.IsTrue(test.Level == expression[test.Index].Level, testNumberDesc + ":" + debugLevel);
                //    Assert.IsTrue(test.LevelInExpression == expression[test.Index].LevelInExpression, testNumberDesc + debugLevelExpression);
                //    Assert.IsTrue(test.Parent == (expression[test.Index].Parent == null ? "-" : expression[test.Index].Parent.ToString().Trim()), testNumberDesc + ":" + debugParent);
                //    Assert.IsTrue(test.Root == expression[test.Index].Root.ToString().Trim(), "Output test:" + debugRoot);
                //    Assert.IsTrue(test.Previous == (expression[test.Index].PrevInExpression == null ? null : expression[test.Index].PrevInExpression.ToString().Trim()), testNumberDesc + ":" + debugName);
                //    Assert.IsTrue(test.Next == (expression[test.Index].NextInExpression == null ? null : expression[test.Index].NextInExpression.ToString().Trim()), testNumberDesc + debugName);
                //}

                Assert.IsTrue(expression.ToString().Replace(" ", "") == expressionOut.Trim(), testNumberDesc + " check toString()");

                for (var e = 0; e < entities.Count(); e++)
                {
                    var test = descendentsTest[e];
                    var descendants = expression.Find(entities.ElementAt(e)).Descendants().ToEntities().ToList();
                    var descendantsExpected = string.IsNullOrWhiteSpace(test.Items) ? new string[0] : test.Items.Split(',');

                    var children = expression.Find(entities.ElementAt(e)).Children().ToEntities().ToList();
                    var childrenExpected = string.IsNullOrWhiteSpace(test.Children) ? new string[0] : test.Children.Split(',');

                    Assert.IsTrue(descendants.Count == descendantsExpected.Length, testNumberDesc + string.Format("Descendants of '{0}': Count of descendants not match for entity.", test.EntityTest));
                    for (var i = 0; i < descendants.Count; i++)
                        Assert.IsTrue(descendants[i].ToString() == descendantsExpected[i].Trim(), testNumberDesc + string.Format("Descendants of '{0}': Item {1} and {2} not match.", test.EntityTest, descendants[i].ToString(), descendantsExpected[i].Trim()));

                    Assert.IsTrue(children.Count == childrenExpected.Length, testNumberDesc + string.Format("Children of '{0}': Count of children not match for entity.", test.EntityTest));
                    for (var i = 0; i < children.Count; i++)
                        Assert.IsTrue(children[i].ToString() == childrenExpected[i].Trim(), testNumberDesc + string.Format("Children of '{0}': Item {1} and {2} not match.", test.EntityTest, children[i].ToString(), childrenExpected[i].Trim()));
                }

                for (var e = 0; e < entities.Count(); e++)
                {
                    var test = ancestorsTest[e];
                    var ancestors = expression.Find(entities.ElementAt(e)).Ancestors().ToEntities().ToList();
                    var ancestorsExpected = string.IsNullOrWhiteSpace(test.Items) ? new string[0] : test.Items.Split(',');
                    var parents = expression.Find(entities.ElementAt(e)).Parents().ToEntities().ToList();
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
}
