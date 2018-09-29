
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class EntitiesData
    {
        public CircularEntity A { get; set;  } = new CircularEntity("A");
        public CircularEntity B { get; set; } = new CircularEntity("B");
        public CircularEntity C { get; set; } = new CircularEntity("C");
        public CircularEntity D { get; set; } = new CircularEntity("D");
        public CircularEntity E { get; set; } = new CircularEntity("E");
        public CircularEntity F { get; set; } = new CircularEntity("F");
        public CircularEntity G { get; set; } = new CircularEntity("G");
        public CircularEntity H { get; set; } = new CircularEntity("H");
        public CircularEntity I { get; set; } = new CircularEntity("I");
        public CircularEntity J { get; set; } = new CircularEntity("J");
        public CircularEntity L { get; set; } = new CircularEntity("L");
        public CircularEntity K { get; set; } = new CircularEntity("K");
        public CircularEntity M { get; set; } = new CircularEntity("M");
        public CircularEntity N { get; set; } = new CircularEntity("N");
        public CircularEntity O { get; set; } = new CircularEntity("O");
        public CircularEntity P { get; set; } = new CircularEntity("P");
        public CircularEntity Q { get; set; } = new CircularEntity("Q");
        public CircularEntity R { get; set; } = new CircularEntity("R");
        public CircularEntity S { get; set; } = new CircularEntity("S");
        public CircularEntity T { get; set; } = new CircularEntity("T");
        public CircularEntity U { get; set; } = new CircularEntity("U");
        public CircularEntity V { get; set; } = new CircularEntity("V");
        public CircularEntity X { get; set; } = new CircularEntity("X");
        public CircularEntity Z { get; set; } = new CircularEntity("Z");
        public CircularEntity W { get; set; } = new CircularEntity("W");
        public CircularEntity Y { get; set; } = new CircularEntity("Y");

        public EntitiesData()
        {
            
        }
        
        public void Clear()
        {
            A = new CircularEntity("A");
            B = new CircularEntity("B");
            C = new CircularEntity("C");
            D = new CircularEntity("D");
            E = new CircularEntity("E");
            F = new CircularEntity("F");
            G = new CircularEntity("G");
            H = new CircularEntity("H");
            I = new CircularEntity("I");
            J = new CircularEntity("J");
            L = new CircularEntity("L");
            K = new CircularEntity("K");
            M = new CircularEntity("M");
            N = new CircularEntity("N");
            O = new CircularEntity("O");
            P = new CircularEntity("P");
            Q = new CircularEntity("Q");
            R = new CircularEntity("R");
            S = new CircularEntity("S");
            T = new CircularEntity("T");
            U = new CircularEntity("U");
            V = new CircularEntity("V");
            X = new CircularEntity("X");
            Z = new CircularEntity("Z");
            W = new CircularEntity("W");
            Y = new CircularEntity("Y");
        }

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
