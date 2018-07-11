using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SearchIEnumerableExtensionsSiblingsTests : EntitiesData
    {
        [Fact]
        public void SearchSiblings_Surface_CheckAllItems()
        {
            var r = A + (B + C + (D + B) + E + F + (G + H) + I) + (W + (J + B) + (Y + P)) + P;
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Equal(16, expression.Count);
            Assert.Equal("A + (B + C + (D + B) + E + F + (G + H) + I) + (W + (J + B) + (Y + P)) + P", expression.AsSerializer().Serialize());

            var leftDirection = SiblingDirection.Previous;
            var rigthDirection = SiblingDirection.Next;
            
            // A
            var items = expression.Where(f=> f.Index == 0).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 0).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 0).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // B
            items = expression.Where(f => f.Index == 1).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "W", index: 10);
            TestItem(items[1], name: "P", index: 15);

            items = expression.Where(f => f.Index == 1).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 1).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "W", index: 10);
            TestItem(items[1], name: "P", index: 15);

            // C
            items = expression.Where(f => f.Index == 2).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "E", index: 5);
            TestItem(items[2], name: "F", index: 6);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Where(f => f.Index == 2).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 2).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "E", index: 5);
            TestItem(items[2], name: "F", index: 6);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            // D
            items = expression.Where(f => f.Index == 3).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "E", index: 5);
            TestItem(items[2], name: "F", index: 6);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Where(f => f.Index == 3).Siblings(direction: leftDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "C", index: 2);

            items = expression.Where(f => f.Index == 3).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "E", index: 5);
            TestItem(items[1], name: "F", index: 6);
            TestItem(items[2], name: "G", index: 7);
            TestItem(items[3], name: "I", index: 9);

            // B
            items = expression.Where(f => f.Index == 4).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 4).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 4).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // E
            items = expression.Where(f => f.Index == 5).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "F", index: 6);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Where(f => f.Index == 5).Siblings(direction: leftDirection).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "C", index: 2);

            items = expression.Where(f => f.Index == 5).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "F", index: 6);
            TestItem(items[1], name: "G", index: 7);
            TestItem(items[2], name: "I", index: 9);

            // F
            items = expression.Where(f => f.Index == 6).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "E", index: 5);
            TestItem(items[3], name: "G", index: 7);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Where(f => f.Index == 6).Siblings(direction: leftDirection).ToList();
            Assert.Equal(3, items.Count());
            TestItem(items[0], name: "E", index: 5);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "C", index: 2);

            items = expression.Where(f => f.Index == 6).Siblings(direction: rigthDirection).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "G", index: 7);
            TestItem(items[1], name: "I", index: 9);

            // G
            items = expression.Where(f => f.Index == 7).Siblings().ToList();            
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "E", index: 5);
            TestItem(items[3], name: "F", index: 6);
            TestItem(items[4], name: "I", index: 9);

            items = expression.Where(f => f.Index == 7).Siblings(direction: leftDirection).ToList();
            Assert.Equal(4, items.Count());
            TestItem(items[0], name: "F", index: 6);
            TestItem(items[1], name: "E", index: 5);
            TestItem(items[2], name: "D", index: 3);
            TestItem(items[3], name: "C", index: 2);

            items = expression.Where(f => f.Index == 7).Siblings(direction: rigthDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "I", index: 9);

            // H
            items = expression.Where(f => f.Index == 8).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 8).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 8).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // I
            items = expression.Where(f => f.Index == 9).Siblings().ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "E", index: 5);
            TestItem(items[3], name: "F", index: 6);
            TestItem(items[4], name: "G", index: 7);

            items = expression.Where(f => f.Index == 9).Siblings(direction: leftDirection).ToList();
            Assert.Equal(5, items.Count());
            TestItem(items[0], name: "G", index: 7);
            TestItem(items[1], name: "F", index: 6);
            TestItem(items[2], name: "E", index: 5);
            TestItem(items[3], name: "D", index: 3);
            TestItem(items[4], name: "C", index: 2);

            items = expression.Where(f => f.Index == 9).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // W
            items = expression.Where(f => f.Index == 10).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "P", index: 15);

            items = expression.Where(f => f.Index == 10).Siblings(direction: leftDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "B", index: 1);

            items = expression.Where(f => f.Index == 10).Siblings(direction: rigthDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "P", index: 15);

            // J
            items = expression.Where(f => f.Index == 11).Siblings().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "Y", index: 13);

            items = expression.Where(f => f.Index == 11).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 11).Siblings(direction: rigthDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "Y", index: 13);

            // B
            items = expression.Where(f => f.Index == 12).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 12).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 12).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // Y
            items = expression.Where(f => f.Index == 13).Siblings().ToList();
            Assert.Single(items);
            TestItem(items[0], name: "J", index: 11);

            items = expression.Where(f => f.Index == 13).Siblings(direction: leftDirection).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "J", index: 11);

            items = expression.Where(f => f.Index == 13).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // P
            items = expression.Where(f => f.Index == 14).Siblings().ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 14).Siblings(direction: leftDirection).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 14).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);

            // P
            items = expression.Where(f => f.Index == 15).Siblings().ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "W", index: 10);

            items = expression.Where(f => f.Index == 15).Siblings(direction: leftDirection).ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "W", index: 10);
            TestItem(items[1], name: "B", index: 1);

            items = expression.Where(f => f.Index == 15).Siblings(direction: rigthDirection).ToList();
            Assert.Empty(items);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckFilter()
        {
            var r = A + (D + I) + I + (B + C + J + D + Y);
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal("A + (D + I) + I + (B + C + J + (D + I) + Y)", expression.AsSerializer().Serialize());
            // Index:     0    1   2    3    4   5   6    7   8    9              

            var items = expression.Where(f => f.Index == 7).Siblings(f => f.Entity.Name == "C" || f.Entity.Name == "Y").ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "C", index: 5);
            TestItem(items[1], name: "Y", index: 9);

            // NEXT - RIGTH
            // C is left J - ignored
            // Y is in filter
            // D is ignored in filter
            items = expression.Where(f => f.Index == 6).Siblings(f => f.Entity.Name == "C" || f.Entity.Name == "Y", direction: SiblingDirection.Next).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "Y", index: 9);

            // Previous - LEFT
            // Y is rigth D - ignored
            // J is in filter
            // C is ignored in filter
            items = expression.Where(f => f.Index == 7).Siblings(f => f.Entity.Name == "J", direction: SiblingDirection.Previous).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "J", index: 6);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckFilterWithPosition()
        {
            var r = A + (D + I) + I + (B + C + J + D + Y);
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal("A + (D + I) + I + (B + C + J + (D + I) + Y)", expression.AsSerializer().Serialize());
            //            0    1   2    3    4   5   6    7   8    9   
            // ALL:                              1   2    3        4  

            var items = expression.Where(f => f.Index == 7).Siblings((f, pos) => pos == 1).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "C", index: 5);            

            items = expression.Where(f => f.Index == 7).Siblings((f, pos) => pos == 2).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "J", index: 6);

            // not return it's own
            items = expression.Where(f => f.Index == 7).Siblings((f, pos) => pos == 3).ToList();
            Assert.Empty(items);

            items = expression.Where(f => f.Index == 7).Siblings((f, pos) => pos == 4).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "Y", index: 9);

            // rigth test
            items = expression.Where(f => f.Index == 7).Siblings((f, pos) => pos == 1, direction: SiblingDirection.Next).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "Y", index: 9);

            // left test
            items = expression.Where(f => f.Index == 7).Siblings((f, pos) => pos == 1, direction: SiblingDirection.Previous).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "J", index: 6);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckStop_StopWhenFoundSpecifyEntity()
        {
            var r = A + B + C + (D + F + (J + I)) + D + J;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal("A + B + C + (D + F + (J + I)) + (D + F + (J + I)) + (J + I)", expression.AsSerializer().Serialize());
            //            0   1   2    3   4    5   6      7   8    9   10     11  12

            var items = expression.Where(f => f.Index == 2).Siblings(null, f => f.Entity.Name == "D").ToList();
            Assert.Equal(2, items.Count());
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "D", index: 3);

            // Next
            items = expression.Where(f => f.Index == 2).Siblings(null, f => f.Entity.Name == "D", direction: SiblingDirection.Next).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "D", index: 3);

            // Previous of D (pos=1)
            items = expression.Where(f => f.Index == 7).Siblings(null, f => f.Entity.Name == "D", direction: SiblingDirection.Previous).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "D", index: 3);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckStopWithPosition_StopWhenFoundFirstMod2()
        {
            var r = A + B + C + (D + F + (J + I)) + D + J;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal("A + B + C + (D + F + (J + I)) + (D + F + (J + I)) + (J + I)", expression.AsSerializer().Serialize());
            //            0   1   2    3   4    5   6      7   8    9   10     11  12
            // ALL:           1   2    3                   4                   5
            // NEXT FROM D0:                               1                   2
            // PREV FROM D0:  2   1    

            // All - stop when find first mod 2 from C0
            var items = expression.Where(f => f.Index == 2).Siblings(null, (f, pos) => pos % 2 == 0).ToList();
            Assert.Equal(3, items.Count);
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "D", index: 7);

            // Next
            items = expression.Where(f => f.Index == 3).Siblings(null, (f, pos) => pos % 2 == 0, direction: SiblingDirection.Next).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], name: "D", index: 7);
            TestItem(items[1], name: "J", index: 11);

            // Previous of D (pos=0)
            items = expression.Where(f => f.Index == 3).Siblings(null, (f, pos) => pos % 2 == 0, direction: SiblingDirection.Previous).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "B", index: 1);
        }

        [Fact]
        public void SearchSiblings_CheckUntil_ReturnUntilFoundPositionWithMod2()
        {
            var r = A + B + C + (D + F + (J + I)) + D + J;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal("A + B + C + (D + F + (J + I)) + (D + F + (J + I)) + (J + I)", expression.AsSerializer().Serialize());
            //            0   1   2    3   4    5   6      7   8    9   10     11  12
            // ALL:           1   2    3                   4                   5
            // NEXT FROM D0:                               1                   2
            // PREV FROM D0:  2   1

            // All - stop when find first mod 2 from C0
            var items = expression.Where(f => f.Index == 2).SiblingsUntil((f, pos) => pos % 2 == 0).ToList();
            Assert.Equal(3, items.Count);
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "D", index: 7);

            // Next
            items = expression.Where(f => f.Index == 3).SiblingsUntil((f, pos) => pos % 2 == 0, direction: SiblingDirection.Next).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], name: "D", index: 7);
            TestItem(items[1], name: "J", index: 11);

            // Previous of D (pos=0)
            items = expression.Where(f => f.Index == 3).SiblingsUntil((f, pos) => pos % 2 == 0, direction: SiblingDirection.Previous).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], name: "C", index: 2);
            TestItem(items[1], name: "B", index: 1);
        }

        [Fact]
        public void SearchSiblings_CheckUntil_ReturnUntilFoundSpecifyEntity()
        {
            var r = A + B + C + (D + F + (J + I)) + D + J;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal("A + B + C + (D + F + (J + I)) + (D + F + (J + I)) + (J + I)", expression.AsSerializer().Serialize());
            //            0   1   2    3   4    5   6      7   8    9   10     11  12
            // ALL:           1   2    3                   4                   5
            // NEXT FROM D0:                               1                   2
            // PREV FROM D0:  2   1    

            // All - stop when find first mod 2 from C0
            var items = expression.Where(f => f.Index == 2).SiblingsUntil(f => f.Index == 7).ToList();
            Assert.Equal(3, items.Count);
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "D", index: 7);

            // Next
            items = expression.Where(f => f.Index == 3).SiblingsUntil(f => f.Entity.Name == "J", direction: SiblingDirection.Next).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], name: "D", index: 7);
            TestItem(items[1], name: "J", index: 11);

            // Previous of D (pos=0)
            items = expression.Where(f => f.Index == 3).SiblingsUntil(f => f.Entity.Name == "C", direction: SiblingDirection.Previous).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "C", index: 2);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckFilterWithStartAndEndPosition_ReturnUntilPosEnd()
        {
            var r = A + B + C + (D + F + (J + I)) + D + J;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal("A + B + C + (D + F + (J + I)) + (D + F + (J + I)) + (J + I)", expression.AsSerializer().Serialize());
            //            0   1   2    3   4    5   6      7   8    9   10     11  12
            // ALL:           1   2    3                   4                   5
            // NEXT FROM D0:                               1                   2
            // PREV FROM D1:  3   2    1

            // All - get position 2 to 4 starting from 'B' (first child's A)
            var items = expression.Where(f => f.Index == 2).Siblings(2, 4).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "D", index: 7);

            // Next
            items = expression.Where(f => f.Index == 3).Siblings(1, 1, direction: SiblingDirection.Next).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "D", index: 7);

            // Previous of D (pos=0)
            items = expression.Where(f => f.Index == 7).Siblings(1, 2, direction: SiblingDirection.Previous).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "C", index: 2);
        }

        [Fact]
        public void SearchSiblings_Deep_CheckFilterEndPos_ReturnUntilPosEnd()
        {
            var r = A + B + C + (D + F + (J + I)) + D + J;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal("A + B + C + (D + F + (J + I)) + (D + F + (J + I)) + (J + I)", expression.AsSerializer().Serialize());
            //            0   1   2    3   4    5   6      7   8    9   10     11  12
            // ALL:           1   2    3                   4                   5
            // NEXT FROM D0:                               1                   2
            // PREV FROM D1:  3   2    1

            // All 
            var items = expression.Where(f => f.Index == 2).Siblings(4).ToList();
            Assert.Equal(3, items.Count);
            TestItem(items[0], name: "B", index: 1);
            TestItem(items[1], name: "D", index: 3);
            TestItem(items[2], name: "D", index: 7);

            // Next
            items = expression.Where(f => f.Index == 3).Siblings(1, direction: SiblingDirection.Next).ToList();
            Assert.Single(items);
            TestItem(items[0], name: "D", index: 7);

            // Previous of D (pos=0)
            items = expression.Where(f => f.Index == 7).Siblings(2, direction: SiblingDirection.Previous).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], name: "D", index: 3);
            TestItem(items[1], name: "C", index: 2);
        }
        
        private static void TestItem(EntityItem<CircularEntity> item, string name, int index)
        {
            Assert.Equal(name, item.Entity.Name);
            Assert.Equal(index, item.Index);
        }
    }
}
