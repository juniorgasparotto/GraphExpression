using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class MatrixToExpressionAsStringTest : EntitiesData
    {
        [Fact]
        public void CreateMatrix_MultiLevelAndRecursive_Surface_VerifyMatrix()
        {
            var r = A + (B + (C + A) + A) + (D + D + E + (F + (G + A + C) + Y) + Z) + G;
            var expression = new Expression(A, false);
            Assert.Equal(15, expression.Count);
            TestEntityItem(expression[0], name: "A", index: 0, indexAtLevel: 0, level: 1,  levelAtExpression: 1, previous: null, next: "B", parent: null);
            TestEntityItem(expression[1], name: "B", index: 1, indexAtLevel: 0, level: 2,  levelAtExpression: 2, previous: "A", next: "C", parent: "A");
            TestEntityItem(expression[2], name: "C", index: 2, indexAtLevel: 0, level: 3,  levelAtExpression: 3, previous: "B", next: "A", parent: "B");
            TestEntityItem(expression[3], name: "A", index: 3, indexAtLevel: 0, level: 4,  levelAtExpression: 3, previous: "C", next: "A", parent: "C");
            TestEntityItem(expression[4], name: "A", index: 4, indexAtLevel: 1, level: 3,  levelAtExpression: 2, previous: "A", next: "D", parent: "B");
            TestEntityItem(expression[5], name: "D", index: 5, indexAtLevel: 1, level: 2,  levelAtExpression: 2, previous: "A", next: "D", parent: "A");
            TestEntityItem(expression[6], name: "D", index: 6, indexAtLevel: 0, level: 3,  levelAtExpression: 2, previous: "D", next: "E", parent: "D");
            TestEntityItem(expression[7], name: "E", index: 7, indexAtLevel: 1, level: 3,  levelAtExpression: 2, previous: "D", next: "F", parent: "D");
            TestEntityItem(expression[8], name: "F", index: 8, indexAtLevel: 2, level: 3,  levelAtExpression: 3, previous: "E", next: "G", parent: "D");
            TestEntityItem(expression[9], name: "G", index: 9, indexAtLevel: 0, level: 4,  levelAtExpression: 4, previous: "F", next: "A", parent: "F");
            TestEntityItem(expression[10], name: "A", index: 10, indexAtLevel: 0, level: 5,  levelAtExpression: 4, previous: "G", next: "C", parent: "G");
            TestEntityItem(expression[11], name: "C", index: 11, indexAtLevel: 1, level: 5,  levelAtExpression: 4, previous: "A", next: "Y", parent: "G");
            TestEntityItem(expression[12], name: "Y", index: 12, indexAtLevel: 1, level: 4,  levelAtExpression: 3, previous: "C", next: "Z", parent: "F");
            TestEntityItem(expression[13], name: "Z", index: 13, indexAtLevel: 3, level: 3,  levelAtExpression: 2, previous: "Y", next: "G", parent: "D");
            TestEntityItem(expression[14], name: "G", index: 14, indexAtLevel: 2, level: 2,  levelAtExpression: 1, previous: "Z", next: null, parent: "A");
        }

        [Fact]
        public void CreateMatrix_MultiLevelAndRecursive_Deep_VerifyMatrix()
        {
            var r = A + (B + (C + A) + A) + (D + D + E + (F + (G + A + C) + Y) + Z) + G;
            var expression = new Expression(A);
            Assert.Equal(19, expression.Count);
            TestEntityItem(expression[0], name: "A", index: 0, indexAtLevel: 0, level: 1, levelAtExpression: 1, previous: null, next: "B", parent: null);
            TestEntityItem(expression[1], name: "B", index: 1, indexAtLevel: 0, level: 2, levelAtExpression: 2, previous: "A", next: "C", parent: "A");
            TestEntityItem(expression[2], name: "C", index: 2, indexAtLevel: 0, level: 3, levelAtExpression: 3, previous: "B", next: "A", parent: "B");
            TestEntityItem(expression[3], name: "A", index: 3, indexAtLevel: 0, level: 4, levelAtExpression: 3, previous: "C", next: "A", parent: "C");
            TestEntityItem(expression[4], name: "A", index: 4, indexAtLevel: 1, level: 3, levelAtExpression: 2, previous: "A", next: "D", parent: "B");
            TestEntityItem(expression[5], name: "D", index: 5, indexAtLevel: 1, level: 2, levelAtExpression: 2, previous: "A", next: "D", parent: "A");
            TestEntityItem(expression[6], name: "D", index: 6, indexAtLevel: 0, level: 3, levelAtExpression: 2, previous: "D", next: "E", parent: "D");
            TestEntityItem(expression[7], name: "E", index: 7, indexAtLevel: 1, level: 3, levelAtExpression: 2, previous: "D", next: "F", parent: "D");
            TestEntityItem(expression[8], name: "F", index: 8, indexAtLevel: 2, level: 3, levelAtExpression: 3, previous: "E", next: "G", parent: "D");
            TestEntityItem(expression[9], name: "G", index: 9, indexAtLevel: 0, level: 4, levelAtExpression: 4, previous: "F", next: "A", parent: "F");
            TestEntityItem(expression[10], name: "A", index: 10, indexAtLevel: 0, level: 5, levelAtExpression: 4, previous: "G", next: "C", parent: "G");
            TestEntityItem(expression[11], name: "C", index: 11, indexAtLevel: 1, level: 5, levelAtExpression: 5, previous: "A", next: "A", parent: "G");
            TestEntityItem(expression[12], name: "A", index: 12, indexAtLevel: 0, level: 6, levelAtExpression: 5, previous: "C", next: "Y", parent: "C");
            TestEntityItem(expression[13], name: "Y", index: 13, indexAtLevel: 1, level: 4, levelAtExpression: 3, previous: "A", next: "Z", parent: "F");
            TestEntityItem(expression[14], name: "Z", index: 14, indexAtLevel: 3, level: 3, levelAtExpression: 2, previous: "Y", next: "G", parent: "D");
            TestEntityItem(expression[15], name: "G", index: 15, indexAtLevel: 2, level: 2, levelAtExpression: 2, previous: "Z", next: "A", parent: "A");
            TestEntityItem(expression[16], name: "A", index: 16, indexAtLevel: 0, level: 3, levelAtExpression: 2, previous: "G", next: "C", parent: "G");
            TestEntityItem(expression[17], name: "C", index: 17, indexAtLevel: 1, level: 3, levelAtExpression: 3, previous: "A", next: "A", parent: "G");
            TestEntityItem(expression[18], name: "A", index: 18, indexAtLevel: 0, level: 4, levelAtExpression: 3, previous: "C", next: null, parent: "C");
        }

        private void TestEntityItem(EntityItem item, string name, int index, int indexAtLevel, int level, int levelAtExpression, string previous, string next, string parent)
        {
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
