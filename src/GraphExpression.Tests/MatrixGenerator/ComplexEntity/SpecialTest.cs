using System.Collections.Generic;
using Xunit;

namespace GraphExpression.Tests
{
    public class SpecialTest
    {
        public class Empty
        {

        }

        public class A
        {
            public string A_PropString { get; set; }
            public B A_PropB { get; set; }
        }

        public class B
        {
            public string B_PropString { get; set; }
            public A A_Circular { get; set; }
            public A A_NonCircular { get; set; }
        }

        [Fact]
        public void CreateCircularAnNonCircularGraph_ReturnExpressionAsString()
        {
            var a = new A
            {
                A_PropString = "Value",
                A_PropB = new B
                {
                    B_PropString = "B_Value"
                },
            };

            a.A_PropB.A_Circular = a;
            a.A_PropB.A_NonCircular = new A();

            var expression = a.AsExpression();

            Assert.Equal(8, expression.Count);
            TestEntityItem(expression[0], isRoot: true, isLast: false, isFirstInParent: true, isLastInParent: false, entity: a, index: 0, indexAtLevel: 0, level: 1, levelAtExpression: 1, previous: null, next: "Value", parent: null);
            TestEntityItem(expression[1], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, entity: "Value", index: 1, indexAtLevel: 0, level: 2, levelAtExpression: 1, previous: a, next: a.A_PropB, parent: a);
            TestEntityItem(expression[2], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, entity: a.A_PropB, index: 2, indexAtLevel: 1, level: 2, levelAtExpression: 2, previous: "Value", next: "B_Value", parent: a);
            TestEntityItem(expression[3], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, entity: "B_Value", index: 3, indexAtLevel: 0, level: 3, levelAtExpression: 2, previous: a.A_PropB, next: a.A_PropB.A_Circular, parent: a.A_PropB);
            TestEntityItem(expression[4], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, entity: a.A_PropB.A_Circular, index: 4, indexAtLevel: 1, level: 3, levelAtExpression: 2, previous: "B_Value", next: a.A_PropB.A_NonCircular, parent: a.A_PropB);
            TestEntityItem(expression[5], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, entity: a.A_PropB.A_NonCircular, index: 5, indexAtLevel: 2, level: 3, levelAtExpression: 3, previous: a.A_PropB.A_Circular, next: null, parent: a.A_PropB);
            TestEntityItem(expression[6], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, entity: null, index: 6, indexAtLevel: 0, level: 4, levelAtExpression: 3, previous: a.A_PropB.A_NonCircular, next: null, parent: a.A_PropB.A_NonCircular);
            TestEntityItem(expression[7], isRoot: false, isLast: true, isFirstInParent: false, isLastInParent: true, entity: null, index: 7, indexAtLevel: 1, level: 4, levelAtExpression: 3, previous: null, next: null, parent: a.A_PropB.A_NonCircular);

            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{a.GetHashCode()}}} + {{@A_PropString: \"Value\"}} + ({{@A_PropB.{a.A_PropB.GetHashCode()}}} + {{@B_PropString: \"B_Value\"}} + {{@A_Circular.{a.A_PropB.A_Circular.GetHashCode()}}} + ({{@A_NonCircular.{a.A_PropB.A_NonCircular.GetHashCode()}}} + {{@A_PropString: null}} + {{@A_PropB: null}}))";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateEmptyEntity_ReturnExpressionAsString()
        {
            var empty = new Empty();
            var expression = empty.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{empty.GetHashCode()}}}";
            Assert.Single(expression);
            Assert.Equal(expected, result);
        }

        protected void TestEntityItem(EntityItem<object> item, bool isRoot, bool isLast, bool isFirstInParent, bool isLastInParent, object entity, int index, int indexAtLevel, int level, int levelAtExpression, object previous, object next, object parent)
        {
            Assert.Equal(isRoot, item.IsRoot);
            Assert.Equal(isLast, item.IsLast);
            Assert.Equal(isFirstInParent, item.IsFirstInParent);
            Assert.Equal(isLastInParent, item.IsLastInParent);
            Assert.Equal(entity, item.Entity);
            Assert.Equal(index, item.Index);
            Assert.Equal(indexAtLevel, item.IndexAtLevel);
            Assert.Equal(level, item.Level);
            Assert.Equal(levelAtExpression, item.LevelAtExpression);
            Assert.Equal(next, item.Next?.Entity);
            Assert.Equal(parent, item.Parent?.Entity);
            Assert.Equal(previous, item.Previous?.Entity);
        }
    }
}
