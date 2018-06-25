using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SearchAncestorsTests : EntitiesData
    {
        [Fact]
        public void SearchAncestors_Surface_CheckAllItems()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + B))", expression.ToExpressionAsString());
            var items = expression.Find(A).ElementAt(0).Descendants().ToList();
            Assert.Equal(7, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "C", index: 2);
            TestItem(items[2], name: "D", index: 3);
            TestItem(items[3], name: "B", index: 4);
            TestItem(items[4], name: "F", index: 5);
            TestItem(items[5], name: "I", index: 6);
            TestItem(items[6], name: "B", index: 7);

            items = expression.Find(B).ElementAt(0).Descendants().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "B", index: 4);

            items = expression.Find(C).ElementAt(0).Descendants().ToList();
            Assert.Empty(items);

            items = expression.Find(D).ElementAt(0).Descendants().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 4);

            items = expression.Find(B).ElementAt(1).Descendants().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "B", index: 4);

            items = expression.Find(F).ElementAt(0).Descendants().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "B", index: 7);

            items = expression.Find(I).ElementAt(0).Descendants().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 7);

            items = expression.Find(B).ElementAt(2).Descendants().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "B", index: 4);
        }

        [Fact]
        public void SearchAncestors_Deep_CheckAllItems()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());

            var items = expression.Find(A).ElementAt(0).Descendants().ToList();
            Assert.Equal(10, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "C", index: 2);
            TestItem(items[2], name: "D", index: 3);
            TestItem(items[3], name: "B", index: 4);
            TestItem(items[4], name: "F", index: 5);
            TestItem(items[5], name: "I", index: 6);
            TestItem(items[6], name: "B", index: 7);
            TestItem(items[7], name: "C", index: 8);
            TestItem(items[8], name: "D", index: 9);
            TestItem(items[9], name: "B", index: 10);

            items = expression.Find(B).ElementAt(0).Descendants().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "B", index: 4);

            items = expression.Find(C).ElementAt(0).Descendants().ToList();
            Assert.Empty(items);

            items = expression.Find(D).ElementAt(0).Descendants().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 4);

            items = expression.Find(B).ElementAt(1).Descendants().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "B", index: 4);

            items = expression.Find(F).ElementAt(0).Descendants().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "B", index: 7);
            TestItem(items[2], name: "C", index: 8);
            TestItem(items[3], name: "D", index: 9);
            TestItem(items[4], name: "B", index: 10);

            items = expression.Find(I).ElementAt(0).Descendants().ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "B", index: 7);
            TestItem(items[1], name: "C", index: 8);
            TestItem(items[2], name: "D", index: 9);
            TestItem(items[3], name: "B", index: 10);

            items = expression.Find(B).ElementAt(2).Descendants().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "C", index: 8);
            TestItem(items[1], name: "D", index: 9);
            TestItem(items[2], name: "B", index: 10);

            items = expression.Find(C).ElementAt(1).Descendants().ToList();
            Assert.Empty(items);

            items = expression.Find(D).ElementAt(1).Descendants().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 10);

            items = expression.Find(B).ElementAt(3).Descendants().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "B", index: 4);
        }

        [Fact]
        public void SearchAncestors_Deep_CheckFilter()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());

            var items = expression.Find(A).ElementAt(0).Descendants(f => f.Entity.Name == "B").ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "B", index: 4);
            TestItem(items[2], name: "B", index: 7);
            TestItem(items[3], name: "B", index: 10);
        }

        [Fact]
        public void SearchAncestors_Deep_CheckFilterWithDepth()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            // Depth of F:                              1    2   3    3   4                                          
            var items = expression.Find(F).ElementAt(0).Descendants((f, depth) => f.Entity.Name == "B" && depth == 2).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 7);

            items = expression.Find(F).ElementAt(0).Descendants((f, depth) => f.Entity.Name == "B" && depth == 4).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 10);
        }

        [Fact]
        public void SearchAncestors_Deep_CheckStop_StopWhenFoundFirstEntity()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            var items = expression.Find(F).ElementAt(0).Descendants(null, f => f.Entity.Name == "C").ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "B", index: 7);
            TestItem(items[2], name: "C", index: 8);
        }

        [Fact]
        public void SearchAncestors_Deep_CheckStopWithDepth_StopWhenFoundFirstMod2()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            // Depth of F:                              1    2   3    3   4                                          
            var items = expression.Find(F).ElementAt(0).Descendants(null, (f, depth) => depth % 2 == 0).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "B", index: 7);
        }

        [Fact]
        public void SearchAncestors_Deep_CheckFilterWithDepth_ReturnGreatGrandchildren()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            // Depth of F:                              1    2   3    3   4                                          
            var items = expression.Find(F).ElementAt(0).Descendants(3, 3).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "C", index: 8);
            TestItem(items[1], name: "D", index: 9);
        }

        [Fact]
        public void SearchAncestors_Deep_CheckFilterWithDepth_ReturnChildrenUntilGreatGrandchildren()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            // Depth of F:                              1    2   3    3   4                                          
            var items = expression.Find(F).ElementAt(0).Descendants(3).ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "B", index: 7);
            TestItem(items[2], name: "C", index: 8);
            TestItem(items[3], name: "D", index: 9);
        }

        [Fact]
        public void SearchAncestors_CheckChildrenInFinalEntityWithChildren_ReturnChildren()
        {
            var r = A + B + (B + C + (D + B));
            var expression = new Expression<Entity>(A, f => f.Children);
            Assert.Equal("A + (B + C + (D + B)) + B", expression.ToExpressionAsString());
            var items = expression.Last().Children().ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], "C", 2);
            TestItem(items[1], "D", 3);
        }

        private static void TestItem(EntityItem<Entity> item, string name, int index)
        {
            Assert.Equal(name, item.Entity.Name);
            Assert.Equal(index, item.Index);
        }
    }
}
