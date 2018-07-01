using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class EntitiesData
    {
        public EntitiesData()
        {

        }

        public Entity A { get; } = new Entity("A");
        public Entity B { get; } = new Entity("B");
        public Entity C { get; } = new Entity("C");
        public Entity D { get; } = new Entity("D");
        public Entity E { get; } = new Entity("E");
        public Entity F { get; } = new Entity("F");
        public Entity G { get; } = new Entity("G");
        public Entity H { get; } = new Entity("H");
        public Entity I { get; } = new Entity("I");
        public Entity J { get; } = new Entity("J");
        public Entity L { get; } = new Entity("L");
        public Entity K { get; } = new Entity("K");
        public Entity M { get; } = new Entity("M");
        public Entity N { get; } = new Entity("N");
        public Entity O { get; } = new Entity("O");
        public Entity P { get; } = new Entity("P");
        public Entity Q { get; } = new Entity("Q");
        public Entity R { get; } = new Entity("R");
        public Entity S { get; } = new Entity("S");
        public Entity T { get; } = new Entity("T");
        public Entity U { get; } = new Entity("U");
        public Entity V { get; } = new Entity("V");
        public Entity X { get; } = new Entity("X");
        public Entity Z { get; } = new Entity("Z");
        public Entity W { get; } = new Entity("W");
        public Entity Y { get; } = new Entity("Y");

        protected void TestEntityItem(EntityItem<Entity> item, bool isRoot, bool isLast, bool isFirstInParent, bool isLastInParent, string name, int index, int indexAtLevel, int level, int levelAtExpression, string previous, string next, string parent)
        {
            Assert.Equal(isRoot, item.IsRoot);
            Assert.Equal(isLast, item.IsLast);
            Assert.Equal(isFirstInParent, item.IsFirstInParent);
            Assert.Equal(isLastInParent, item.IsLastInParent);
            Assert.Equal(name, item.Entity.Name);
            Assert.Equal(index, item.Index);
            Assert.Equal(indexAtLevel, item.IndexAtLevel);
            Assert.Equal(level, item.Level);
            Assert.Equal(levelAtExpression, item.LevelAtExpression);
            Assert.Equal(next, item.Next?.Entity.Name);
            Assert.Equal(parent, item.Parent?.Entity.Name);
            Assert.Equal(previous, item.Previous?.Entity.Name);
        }
    }
}
