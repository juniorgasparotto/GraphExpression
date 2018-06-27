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
            var r = A + (B + C + (D + B) + E + F + (G + H) + I) + (W + (J + B) + (Y + P)) + P;
            var expression = new Expression<Entity>(A, f => f.Children);
            Assert.Equal(16, expression.Count);
            Assert.Equal("A + (B + C + (D + B) + E + F + (G + H) + I) + (W + (J + B) + (Y + P)) + P", expression.ToExpressionAsString());

            var leftDirection = SiblingDirection.Previous;
            var rigthDirection = SiblingDirection.Next;
            
            // A
            var items = expression.Find(A).ElementAt(0).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(A).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Find(A).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            // B
            items = expression.Find(B).ElementAt(0).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "W", index: 10);
            TestItem(items[1], name: "P", index: 15);

            items = expression.Find(B).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Find(B).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "W", index: 10);
            TestItem(items[1], name: "P", index: 15);

            // C
            items = expression.Find(C).ElementAt(0).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "E", index: 5);
            TestItem(items[2], name: "F", index: 6);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Find(C).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Find(C).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "E", index: 5);
            TestItem(items[2], name: "F", index: 6);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            // D
            items = expression.Find(D).ElementAt(0).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "E", index: 5);
            TestItem(items[2], name: "F", index: 6);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Find(D).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "C", index: 2);

            items = expression.Find(D).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "E", index: 5);
            TestItem(items[1], name: "F", index: 6);
            TestItem(items[2], name: "G", index: 7);
            TestItem(items[3], name: "I", index: 9);

            // B
            items = expression.Find(B).ElementAt(1).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(B).ElementAt(1).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Find(B).ElementAt(1).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // E
            items = expression.Find(E).ElementAt(0).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "F", index: 6);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Find(E).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "C", index: 2);

            items = expression.Find(E).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "F", index: 6);
            TestItem(items[1], name: "G", index: 7);
            TestItem(items[2], name: "I", index: 9);

            // F
            items = expression.Find(F).ElementAt(0).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "E", index: 5);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Find(F).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "E", index: 5);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "C", index: 2);

            items = expression.Find(F).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "G", index: 7);
            TestItem(items[1], name: "I", index: 9);

            // G
            items = expression.Find(G).ElementAt(0).Siblings().ToList();            
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "E", index: 5);
            TestItem(items[3], name: "F", index: 6);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Find(G).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "F", index: 6);
            TestItem(items[1], name: "E", index: 5);
            TestItem(items[2], name: "D", index: 3);
            TestItem(items[3], name: "C", index: 2);

            items = expression.Find(G).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "I", index: 9);

            // H
            items = expression.Find(H).ElementAt(0).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(H).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Find(H).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // I
            items = expression.Find(I).ElementAt(0).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "E", index: 5);
            TestItem(items[3], name: "F", index: 6);
            TestItem(items[4], name: "G", index: 7);

            items = expression.Find(I).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "G", index: 7);
            TestItem(items[1], name: "F", index: 6);
            TestItem(items[2], name: "E", index: 5);
            TestItem(items[3], name: "D", index: 3);
            TestItem(items[4], name: "C", index: 2);

            items = expression.Find(I).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // W
            items = expression.Find(W).ElementAt(0).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "P", index: 15);

            items = expression.Find(W).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 1);

            items = expression.Find(W).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "P", index: 15);

            // J
            items = expression.Find(J).ElementAt(0).Siblings().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "Y", index: 13);

            items = expression.Find(J).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Find(J).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "Y", index: 13);

            // B
            items = expression.Find(B).ElementAt(2).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(B).ElementAt(2).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Find(B).ElementAt(2).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // Y
            items = expression.Find(Y).ElementAt(0).Siblings().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "J", index: 11);

            items = expression.Find(Y).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "J", index: 11);

            items = expression.Find(Y).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // P
            items = expression.Find(P).ElementAt(0).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Find(P).ElementAt(0).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Find(P).ElementAt(0).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // P
            items = expression.Find(P).ElementAt(1).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "W", index: 10);

            items = expression.Find(P).ElementAt(1).Siblings(direction: leftDirection).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "W", index: 10);
            TestItem(items[1], name: "B", index: 1);

            items = expression.Find(P).ElementAt(1).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);
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
