using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class ExpressionFromCircularEntityTest : EntitiesData
    {
        [Fact]
        public void CreateManualExpression_Surface_ReturnExpressionAsString()
        {
            var expression = new Expression<CircularEntity>();
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = A, Index = 0, IndexAtLevel = 0, Level = 1 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = B, Index = 1, IndexAtLevel = 0, Level = 2 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = C, Index = 2, IndexAtLevel = 1, Level = 2 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = Y, Index = 3, IndexAtLevel = 0, Level = 3 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = D, Index = 4, IndexAtLevel = 2, Level = 2 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = E, Index = 5, IndexAtLevel = 0, Level = 3 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = F, Index = 6, IndexAtLevel = 1, Level = 3 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = G, Index = 7, IndexAtLevel = 0, Level = 4 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = B, Index = 8, IndexAtLevel = 0, Level = 5 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = C, Index = 9, IndexAtLevel = 1, Level = 5 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = Y, Index = 10, IndexAtLevel = 1, Level = 4 });
            expression.Add(new EntityItem<CircularEntity>(expression) { Entity = Z, Index = 11, IndexAtLevel = 2, Level = 3 });
            var expressionString = expression.ToExpressionAsString();
            Assert.Equal("A + B + (C + Y) + (D + E + (F + (G + B + C) + Y) + Z)", expressionString);
        }

        [Fact]
        public void CreateAutomaticExpression_Surface_VerifyMatrix()
        {
            var r = A + (B + (C + A) + A) + (D + D + E + (F + (G + A + C) + Y) + Z) + G;
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Equal(15, expression.Count);
            TestEntityItem(expression[0], isRoot: true, isLast: false, isFirstInParent: true, isLastInParent: false, name: "A", index: 0, indexAtLevel: 0, level: 1, levelAtExpression: 1, previous: null, next: "B", parent: null);
            TestEntityItem(expression[1], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "B", index: 1, indexAtLevel: 0, level: 2, levelAtExpression: 2, previous: "A", next: "C", parent: "A");
            TestEntityItem(expression[2], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "C", index: 2, indexAtLevel: 0, level: 3, levelAtExpression: 3, previous: "B", next: "A", parent: "B");
            TestEntityItem(expression[3], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "A", index: 3, indexAtLevel: 0, level: 4, levelAtExpression: 3, previous: "C", next: "A", parent: "C");
            TestEntityItem(expression[4], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "A", index: 4, indexAtLevel: 1, level: 3, levelAtExpression: 2, previous: "A", next: "D", parent: "B");
            TestEntityItem(expression[5], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "D", index: 5, indexAtLevel: 1, level: 2, levelAtExpression: 2, previous: "A", next: "D", parent: "A");
            TestEntityItem(expression[6], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, name: "D", index: 6, indexAtLevel: 0, level: 3, levelAtExpression: 2, previous: "D", next: "E", parent: "D");
            TestEntityItem(expression[7], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, name: "E", index: 7, indexAtLevel: 1, level: 3, levelAtExpression: 2, previous: "D", next: "F", parent: "D");
            TestEntityItem(expression[8], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "F", index: 8, indexAtLevel: 2, level: 3, levelAtExpression: 3, previous: "E", next: "G", parent: "D");
            TestEntityItem(expression[9], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "G", index: 9, indexAtLevel: 0, level: 4, levelAtExpression: 4, previous: "F", next: "A", parent: "F");
            TestEntityItem(expression[10], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, name: "A", index: 10, indexAtLevel: 0, level: 5, levelAtExpression: 4, previous: "G", next: "C", parent: "G");
            TestEntityItem(expression[11], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "C", index: 11, indexAtLevel: 1, level: 5, levelAtExpression: 4, previous: "A", next: "Y", parent: "G");
            TestEntityItem(expression[12], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "Y", index: 12, indexAtLevel: 1, level: 4, levelAtExpression: 3, previous: "C", next: "Z", parent: "F");
            TestEntityItem(expression[13], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "Z", index: 13, indexAtLevel: 3, level: 3, levelAtExpression: 2, previous: "Y", next: "G", parent: "D");
            TestEntityItem(expression[14], isRoot: false, isLast: true, isFirstInParent: false, isLastInParent: true, name: "G", index: 14, indexAtLevel: 2, level: 2, levelAtExpression: 1, previous: "Z", next: null, parent: "A");
        }

        [Fact]
        public void CreateAutomaticExpression_Deep_VerifyMatrix()
        {
            var r = A + (B + (C + A) + A) + (D + D + E + (F + (G + A + C) + Y) + Z) + G;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal(19, expression.Count);
            TestEntityItem(expression[0], isRoot: true, isLast: false, isFirstInParent: true, isLastInParent: false, name: "A", index: 0, indexAtLevel: 0, level: 1, levelAtExpression: 1, previous: null, next: "B", parent: null);
            TestEntityItem(expression[1], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "B", index: 1, indexAtLevel: 0, level: 2, levelAtExpression: 2, previous: "A", next: "C", parent: "A");
            TestEntityItem(expression[2], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "C", index: 2, indexAtLevel: 0, level: 3, levelAtExpression: 3, previous: "B", next: "A", parent: "B");
            TestEntityItem(expression[3], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "A", index: 3, indexAtLevel: 0, level: 4, levelAtExpression: 3, previous: "C", next: "A", parent: "C");
            TestEntityItem(expression[4], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "A", index: 4, indexAtLevel: 1, level: 3, levelAtExpression: 2, previous: "A", next: "D", parent: "B");
            TestEntityItem(expression[5], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "D", index: 5, indexAtLevel: 1, level: 2, levelAtExpression: 2, previous: "A", next: "D", parent: "A");
            TestEntityItem(expression[6], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, name: "D", index: 6, indexAtLevel: 0, level: 3, levelAtExpression: 2, previous: "D", next: "E", parent: "D");
            TestEntityItem(expression[7], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, name: "E", index: 7, indexAtLevel: 1, level: 3, levelAtExpression: 2, previous: "D", next: "F", parent: "D");
            TestEntityItem(expression[8], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "F", index: 8, indexAtLevel: 2, level: 3, levelAtExpression: 3, previous: "E", next: "G", parent: "D");
            TestEntityItem(expression[9], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "G", index: 9, indexAtLevel: 0, level: 4, levelAtExpression: 4, previous: "F", next: "A", parent: "F");
            TestEntityItem(expression[10], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, name: "A", index: 10, indexAtLevel: 0, level: 5, levelAtExpression: 4, previous: "G", next: "C", parent: "G");
            TestEntityItem(expression[11], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "C", index: 11, indexAtLevel: 1, level: 5, levelAtExpression: 5, previous: "A", next: "A", parent: "G");
            TestEntityItem(expression[12], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "A", index: 12, indexAtLevel: 0, level: 6, levelAtExpression: 5, previous: "C", next: "Y", parent: "C");
            TestEntityItem(expression[13], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "Y", index: 13, indexAtLevel: 1, level: 4, levelAtExpression: 3, previous: "A", next: "Z", parent: "F");
            TestEntityItem(expression[14], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: true, name: "Z", index: 14, indexAtLevel: 3, level: 3, levelAtExpression: 2, previous: "Y", next: "G", parent: "D");
            TestEntityItem(expression[15], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "G", index: 15, indexAtLevel: 2, level: 2, levelAtExpression: 2, previous: "Z", next: "A", parent: "A");
            TestEntityItem(expression[16], isRoot: false, isLast: false, isFirstInParent: false, isLastInParent: false, name: "A", index: 16, indexAtLevel: 0, level: 3, levelAtExpression: 2, previous: "G", next: "C", parent: "G");
            TestEntityItem(expression[17], isRoot: false, isLast: false, isFirstInParent: true, isLastInParent: false, name: "C", index: 17, indexAtLevel: 1, level: 3, levelAtExpression: 3, previous: "A", next: "A", parent: "G");
            TestEntityItem(expression[18], isRoot: false, isLast: true, isFirstInParent: false, isLastInParent: true, name: "A", index: 18, indexAtLevel: 0, level: 4, levelAtExpression: 3, previous: "C", next: null, parent: "C");
        }

        [Fact]
        public void CreateExpressionAsString_2Entities_WithoutParameterEncloseParenthesisInRoot_ReturnExpressionWithoutRootParenthesis()
        {
            var r = A + B;
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + B", expression.ToExpressionAsString());
        }

        [Fact]
        public void CreateExpressionAsString_2Entities_WithParameterEncloseParenthesisInRoot_ReturnExpressionWithRootParenthesis()
        {
            var r = A + B;
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("(A + B)", expression.ToExpressionAsString(true));
        }

        [Fact]
        public void CreateExpressionAsString_1Entities_WithoutParameterEncloseParenthesisInRoot_ReturnExpressionWithoutRootParenthesis()
        {
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Single(expression);
            Assert.Equal("A", expression.ToExpressionAsString());
        }

        [Fact]
        public void CreateExpressionAsString_1Entities_WithParameterEncloseParenthesisInRoot_ReturnExpressionWithRootParenthesis()
        {
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Single(expression);
            Assert.Equal("(A)", expression.ToExpressionAsString(true));
        }

        [Fact]
        public void CreateCiclicalExpression_Deep_Direct_ReturnNotRepeat()
        {
            var r = A + A;
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + A", expression.ToExpressionAsString());
        }

        [Fact]
        public void CreateCiclicalExpression_Deep_Indirect_ReturnNotRepeat()
        {
            var r = A + (C + A) + C;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal(5, expression.Count);
            Assert.Equal("A + (C + A) + (C + A)", expression.ToExpressionAsString());
        }

        [Fact]
        public void CreateCiclicalExpression_Surface_Direct_ReturnNotRepeat()
        {
            var r = A + A;
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + A", expression.ToExpressionAsString());
        }

        [Fact]
        public void CreateCiclicalExpression_Surface_Indirect_ReturnNotRepeat()
        {
            var r = A + (C + A) + C;
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Equal(4, expression.Count);
            Assert.Equal("A + (C + A) + C", expression.ToExpressionAsString());
        }

        [Fact]
        public void CreateCiclicalExpression_Surface_IndirectMultiLevel_ReturnNotRepeat()
        {
            var r = A + (B + (C + (D + B))) + C;
            var expression = new Expression<CircularEntity>(A, f => f.Children);
            Assert.Equal(6, expression.Count);
            Assert.Equal("A + (B + (C + (D + B))) + C", expression.ToExpressionAsString());
        }

        [Fact]
        public void CreateCiclicalExpression_Deep_IndirectMultiLevel_ReturnNotRepeat()
        {
            var r = A + (B + (C + (D + B))) + C;
            var expression = new Expression<CircularEntity>(A, f => f.Children, true);
            Assert.Equal(9, expression.Count);
            Assert.Equal("A + (B + (C + (D + B))) + (C + (D + (B + C)))", expression.ToExpressionAsString());
        }

        //[Fact]
        //public void CreateAutomaticExpression_Deep_VerifyMatrix2()
        //{
        //    var r = A + (B + (C + C));
        //    Test("A + (B + (C + C))");

        //    r = A + (B + (C + C) + D);
        //    Test("A + (B + (C + C) + D)");

        //    r = A + (B + (C + C) + C);
        //    Test("A + (B + (C + C) + (C + C))", true);

        //    r = A + (B + (C + C)) + D;
        //    Test("A + (B + (C + C)) + D");

        //    r = A + (B + (C + C)) + C;
        //    Test("A + (B + (C + C)) + (C + C)", true);

        //    r = A + (B + (C + (D + C))) + C;
        //    Test("A + (B + (C + (D + C))) + (C + (D + C))", true);

        //    r = A + (B + (C + (D + C)) + I) + C;
        //    Test("A + (B + (C + (D + C)) + I) + (C + (D + C))", true);

        //    r = A + (B + (C + (D + C) + P) + I) + C;
        //    Test("A + (B + (C + (D + C) + P) + I) + (C + (D + C) + P)", true);

        //    r = A + (B + (C + (D + C) + P) + I + P) + C;
        //    Test("A + (B + (C + (D + C) + P) + I + P) + (C + (D + C) + P)", true);

        //    r = A + (B + (C + (D + C) + P + G) + I + P) + C;
        //    Test("A + (B + (C + (D + C) + P + G) + I + P) + (C + (D + C) + P + G)", true);

        //    r = A + (B + (C + (D + C) + P + G)) + C;
        //    Test("A + (B + (C + (D + C) + P + G)) + (C + (D + C) + P + G)", true);

        //    r = A + (B + (C + C) + B) + A;
        //    Test("A + (B + (C + C) + B) + A");

        //    r = A + A;
        //    Test("A + A");

        //    r = A + (B + B + A);
        //    Test("A + (B + B + A)");

        //    r = A + (A + B);
        //    Test("A + B + A");

        //    r = A + (B + C) + A + A + F;
        //    Test("A + (B + C) + A + A + F");

        //    r = A + (B + C) + A + V + F;
        //    Test("A + (B + C) + A + V + F");

        //    r = A + (B + K + (C + B + P)) + A + V + F;
        //    Test("A + (B + K + (C + B + P)) + A + V + F");

        //    r = A + (B + C + D) + D + (E + B) + F + (G + G + C) + (H + C);
        //    Test("A + (B + C + D) + D + (E + (B + C + D)) + F + (G + G + C) + (H + C)", true);

        //    r = A + B + A;
        //    Test("A + B + A");

        //    r = A + (B + A);
        //    Test("A + (B + A)");

        //    r = A + (B + A + (B + A));
        //    Test("A + (B + A + A + B)");

        //    r = A + (B + A) + B + A;
        //    Test("A + (B + A) + (B + A) + A", true);

        //    r = A + (B + A) + A + B;
        //    Test("A + (B + A) + A + (B + A)", true);

        //    r = A + (B + A) + A + (C + B);
        //    Test("A + (B + A) + A + (C + (B + A))", true);

        //    r = A + (B + A) + (I + A + D + B + D) + (C + B);
        //    Test("A + (B + A) + (I + A + D + (B + A) + D) + (C + (B + A))", true);
        //}

        //private void Test(string str, bool deep = false)
        //{
        //    var expression = new Expression(A, deep);
        //    Assert.Equal(str, expression.ToExpressionAsString());
        //    A.Clear();
        //    B.Clear();
        //    C.Clear();
        //    D.Clear();
        //    E.Clear();
        //    F.Clear();
        //    G.Clear();
        //    H.Clear();
        //    I.Clear();
        //    J.Clear();
        //    L.Clear();
        //    K.Clear();
        //    M.Clear();
        //    N.Clear();
        //    O.Clear();
        //    P.Clear();
        //    Q.Clear();
        //    R.Clear();
        //    S.Clear();
        //    T.Clear();
        //    U.Clear();
        //    V.Clear();
        //    X.Clear();
        //    Z.Clear();
        //    W.Clear();
        //    Y.Clear();
        //}
    }
}
