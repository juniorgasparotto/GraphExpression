using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SearchSiblingsTests : EntitiesData
    {
        [Fact]
        public void SearchSiblings_Surface_CheckAllItems()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children);
            Assert.Equal(8, expression.Count);

            Assert.Equal("A + (B + C + (D + B) + E + F + (G + H) + I) + (F + (J + B) + Y) + P", expression.ToExpressionAsString());
            var items = expression.Find(A).ElementAt(0).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(B).ElementAt(0).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "F", index: 5);

            items = expression.Find(C).ElementAt(0).Siblings().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "D", index: 1);

            items = expression.Find(D).ElementAt(0).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(B).ElementAt(1).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(F).ElementAt(0).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(I).ElementAt(0).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "F", index: 5);
            TestItem(items[1], name: "A", index: 0);

            items = expression.Find(B).ElementAt(2).Siblings().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "F", index: 5);
            TestItem(items[2], name: "A", index: 0);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckAllItems()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal(11, expression.Count);

            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());

            var items = expression.Find(A).ElementAt(0).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(B).ElementAt(0).Siblings().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "A", index: 0);

            items = expression.Find(C).ElementAt(0).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "A", index: 0);

            items = expression.Find(D).ElementAt(0).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "A", index: 0);

            items = expression.Find(B).ElementAt(1).Siblings().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "B", index: 1);
            TestItem(items[2], name: "A", index: 0);

            items = expression.Find(F).ElementAt(0).Siblings().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "A", index: 0);

            items = expression.Find(I).ElementAt(0).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "F", index: 5);
            TestItem(items[1], name: "A", index: 0);

            items = expression.Find(B).ElementAt(2).Siblings().ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "F", index: 5);
            TestItem(items[2], name: "A", index: 0);

            items = expression.Find(C).ElementAt(1).Siblings().ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "B", index: 7);
            TestItem(items[1], name: "I", index: 6);
            TestItem(items[2], name: "F", index: 5);
            TestItem(items[3], name: "A", index: 0);

            items = expression.Find(D).ElementAt(1).Siblings().ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "B", index: 7);
            TestItem(items[1], name: "I", index: 6);
            TestItem(items[2], name: "F", index: 5);
            TestItem(items[3], name: "A", index: 0);

            items = expression.Find(B).ElementAt(3).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "D", index: 9);
            TestItem(items[1], name: "B", index: 7);
            TestItem(items[2], name: "I", index: 6);
            TestItem(items[3], name: "F", index: 5);
            TestItem(items[4], name: "A", index: 0);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckFilter()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());

            var items = expression.Find(B).ElementAt(3).Siblings(f => f.Entity.Name == "B" || f.Entity.Name == "A").ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 7);
            TestItem(items[1], name: "A", index: 0);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckFilterWithDepth()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            //Depth of D: 4                        3    2    1
            var items = expression.Find(D).ElementAt(1).Siblings((f, depth) => depth == 1).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 7);

            items = expression.Find(D).ElementAt(1).Siblings((f, depth) => depth == 2).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "I", index: 6);

            items = expression.Find(D).ElementAt(1).Siblings((f, depth) => depth == 3).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "F", index: 5);

            items = expression.Find(D).ElementAt(1).Siblings((f, depth) => depth == 4).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "A", index: 0);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckStop_StopWhenFoundSpecifyAncestor()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            var items = expression.Find(B).ElementAt(3).Siblings(null, f => f.Entity.Name == "F").ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "D", index: 9);
            TestItem(items[1], name: "B", index: 7);
            TestItem(items[2], name: "I", index: 6);
            TestItem(items[3], name: "F", index: 5);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckStopWithDepth_StopWhenFoundFirstMod2()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            //Depth of B: 3                        2    1    
            var items = expression.Find(B).ElementAt(2).Siblings(null, (f, depth) => depth % 2 == 0).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "F", index: 5);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckFilterWithStartAndEndDepth_ReturnUntilGreatGrandfather()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            //Depth of D: 4                        3    2    1                                     
            var items = expression.Find(D).ElementAt(1).Siblings(1, 3).ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "B", index: 7);
            TestItem(items[1], name: "I", index: 6);
            TestItem(items[2], name: "F", index: 5);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckFilterWithEndDepth_ReturnUntilGreatGrandfather()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            //Depth of D: 4                        3    2    1                                     
            var items = expression.Find(D).ElementAt(1).Siblings(3).ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "B", index: 7);
            TestItem(items[1], name: "I", index: 6);
            TestItem(items[2], name: "F", index: 5);
        }

        [Fact]
        public void SearchSiblings_CheckUntil_ReturnUntilFoundDepthWithMod2()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            //Depth of B: 3                        2    1    
            var items = expression.Find(B).ElementAt(2).SiblingsUntil((f, depth) => depth % 2 == 0).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "I", index: 6);
            TestItem(items[1], name: "F", index: 5);
        }

        [Fact]
        public void SearchSiblings_CheckUntil_ReturnUntilFoundSpecifyEntity()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = new Expression<Entity>(A, f => f.Children, true);
            Assert.Equal("A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))", expression.ToExpressionAsString());
            //Depth of B: 3                        2    1    
            var items = expression.Find(B).ElementAt(2).SiblingsUntil(f => f.Entity.Name == "I").ToList();
            Assert.Single(items);
            TestItem(items[0], name: "I", index: 6);
        }

        private static void TestItem(EntityItem<Entity> item, string name, int index)
        {
            Assert.Equal(name, item.Entity.Name);
            Assert.Equal(index, item.Index);
        }
    }
}
