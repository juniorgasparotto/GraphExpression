using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SearchIEnumerableExtensionsDescendantsTests : EntitiesData
    {
        [Fact]
        public void SearchDescendants_CheckDescendantsMultipleEntities()
        {
            var r = A + (B + C + (D + B)) + (F + (I + B));
            var expression = A.AsExpression(f => f.Children);
            var items = expression.Where(f => f.Entity.Name == "D" || f.Entity.Name == "F").Descendants().ToList();
            Assert.Equal(3, items.Count);
            TestItem(items[0], "B", 4);
            TestItem(items[1], "I", 6);
            TestItem(items[2], "B", 7);
        }

        [Fact]
        public void SearchDescendants_CheckDescendantsInFinalEntityWithChildren_ReturnDescendantsFirstOcurrence()
        {
            var r = A + B + (B + C + (D + B));
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A + (B + C + (D + B)) + B", expression.DefaultSerializer.Serialize());
            var items = expression.Where(f => f.Index == 5 ).Descendants().ToList();
            Assert.Equal(3, items.Count);
            TestItem(items[0], "C", 2);
            TestItem(items[1], "D", 3);
            TestItem(items[2], "B", 4);
        }

        [Fact]
        public void SearchDescendants_CheckDescendantsInFinalEntityWithChildren_ReturnOnlyChildren()
        {
            var r = A + B + (B + C + (D + B));
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A + (B + C + (D + B)) + B", expression.DefaultSerializer.Serialize());
            var items = expression.Where(f => f.Index == 5).Descendants(1).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], "C", 2);
            TestItem(items[1], "D", 3);

            items = expression.Where(f => f.Index == 5).Descendants(1, 1).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], "C", 2);
            TestItem(items[1], "D", 3);
        }

        [Fact]
        public void SearchDescendants_CheckChildrenInFinalEntityWithChildren_ReturnChildren()
        {
            var r = A + B + (B + C + (D + B));
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A + (B + C + (D + B)) + B", expression.DefaultSerializer.Serialize());
            var items = expression.Where(f => f.Index == 5).Children().ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], "C", 2);
            TestItem(items[1], "D", 3);
        }

        [Fact]
        public void SearchDescendants_CheckDescendantsInFinalEntityWithChildren_ReturnOnlyGrandchildren()
        {
            var r = A + (B + (C + (D + B)) + (I + J) + K) + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A + (B + (C + (D + B)) + (I + J) + K) + B", expression.DefaultSerializer.Serialize());
            var items = expression.Where(f => f.Index == 8).Descendants(2, 2).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], "D", 3);
            TestItem(items[1], "J", 6);
        }

        [Fact]
        public void SearchDescendants_CheckDescendantsWithFilter_ReturnSecondEntity()
        {
            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A + B", expression.DefaultSerializer.Serialize());
            var items = expression.Descendants(f => f.Index == 1).ToList();
            Assert.Single(items);
            TestItem(items[0], "B", 1);
        }

        [Fact]
        public void SearchDescendants_CheckDescendantsWithFilterAndDepth_ReturnMod2()
        {
            var r = A + (B + (C + (D + (E + (F + G)))));
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A + (B + (C + (D + (E + (F + G)))))", expression.DefaultSerializer.Serialize());
            //                 1    2    3    4    5   6
            var items = expression.Where(f => f.Index == 0).Descendants((f, depht) => depht % 2 == 0, (f, depht) => depht == 4).ToList();
            Assert.Equal(2, items.Count);
            TestItem(items[0], "C", 2);
            TestItem(items[1], "E", 4);
        }

        [Fact]
        public void SearchDescendants_CheckDescendantsUntil_StopWhenUntilDepth()
        {
            var r = A + (B + (C + (D + (E + (F + G)))));
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A + (B + (C + (D + (E + (F + G)))))", expression.DefaultSerializer.Serialize());
            //                      1    2    3    4   5
            var items = expression.Where(f => f.Index == 1).DescendantsUntil((f, depht) => depht == 4).ToList();
            Assert.Equal(4, items.Count);
            TestItem(items[0], "C", 2);
            TestItem(items[1], "D", 3);
            TestItem(items[2], "E", 4);
            TestItem(items[3], "F", 5);
        }

        [Fact]
        public void SearchDescendants_CheckDescendantsUntil_StopWhenFoundEntity()
        {
            var r = A + (B + (C + (D + (E + (F + G)))));
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal("A + (B + (C + (D + (E + (F + G)))))", expression.DefaultSerializer.Serialize());
            //                      1    2    3    4   5
            var items = expression.Where(f => f.Index == 1).DescendantsUntil(f => f.Entity.Name == "F").ToList();
            Assert.Equal(4, items.Count);
            TestItem(items[0], "C", 2);
            TestItem(items[1], "D", 3);
            TestItem(items[2], "E", 4);
            TestItem(items[3], "F", 5);
        }

        private static void TestItem(EntityItem<CircularEntity> item, string name, int index)
        {
            Assert.Equal(name, item.Entity.Name);
            Assert.Equal(index, item.Index);
        }
    }
}
