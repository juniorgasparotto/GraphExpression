
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

        public CircularEntity A { get; } = new CircularEntity("A");
        public CircularEntity B { get; } = new CircularEntity("B");
        public CircularEntity C { get; } = new CircularEntity("C");
        public CircularEntity D { get; } = new CircularEntity("D");
        public CircularEntity E { get; } = new CircularEntity("E");
        public CircularEntity F { get; } = new CircularEntity("F");
        public CircularEntity G { get; } = new CircularEntity("G");
        public CircularEntity H { get; } = new CircularEntity("H");
        public CircularEntity I { get; } = new CircularEntity("I");
        public CircularEntity J { get; } = new CircularEntity("J");
        public CircularEntity L { get; } = new CircularEntity("L");
        public CircularEntity K { get; } = new CircularEntity("K");
        public CircularEntity M { get; } = new CircularEntity("M");
        public CircularEntity N { get; } = new CircularEntity("N");
        public CircularEntity O { get; } = new CircularEntity("O");
        public CircularEntity P { get; } = new CircularEntity("P");
        public CircularEntity Q { get; } = new CircularEntity("Q");
        public CircularEntity R { get; } = new CircularEntity("R");
        public CircularEntity S { get; } = new CircularEntity("S");
        public CircularEntity T { get; } = new CircularEntity("T");
        public CircularEntity U { get; } = new CircularEntity("U");
        public CircularEntity V { get; } = new CircularEntity("V");
        public CircularEntity X { get; } = new CircularEntity("X");
        public CircularEntity Z { get; } = new CircularEntity("Z");
        public CircularEntity W { get; } = new CircularEntity("W");
        public CircularEntity Y { get; } = new CircularEntity("Y");

        protected void TestEntityItem(EntityItem<CircularEntity> item, bool isRoot, bool isLast, bool isFirstInParent, bool isLastInParent, string name, int index, int indexAtLevel, int level, int levelAtExpression, string previous, string next, string parent)
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
