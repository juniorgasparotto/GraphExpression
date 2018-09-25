using GraphExpression.Serialization;
using Xunit;

namespace GraphExpression.Tests.Serialization
{
    public class SerializationCircularEntityTest : EntitiesData
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
            var expressionString = new CircularEntityExpressionSerializer<CircularEntity>(expression, f=> f.Name).Serialize();
            Assert.Equal("A + B + (C + Y) + (D + E + (F + (G + B + C) + Y) + Z)", expressionString);
        }

        [Fact]
        public void CreateExpressionAsString_2Entities_WithoutParameterEncloseParenthesisInRoot_ReturnExpressionWithoutRootParenthesis()
        {
            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + B", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionAsString_2Entities_WithParameterEncloseParenthesisInRoot_ReturnExpressionWithRootParenthesis()
        {
            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(2, expression.Count);
            var serializer = expression.DefaultSerializer as CircularEntityExpressionSerializer<CircularEntity>;
            serializer.EncloseParenthesisInRoot = true;
            Assert.Equal("(A + B)", serializer.Serialize());
        }

        [Fact]
        public void CreateExpressionAsString_1Entities_WithoutParameterEncloseParenthesisInRoot_ReturnExpressionWithoutRootParenthesis()
        {
            var expression = A.AsExpression(f => f.Children);
            Assert.Single(expression);
            Assert.Equal("A", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionAsString_1Entities_WithParameterEncloseParenthesisInRoot_ReturnExpressionWithRootParenthesis()
        {
            var expression = A.AsExpression(f => f.Children);
            Assert.Single(expression);
            var serializer = expression.DefaultSerializer as CircularEntityExpressionSerializer<CircularEntity>;
            serializer.EncloseParenthesisInRoot = true;
            Assert.Equal("(A)", serializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpression_Deep_Direct_ReturnNotRepeat()
        {
            var r = A + A;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + A", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpression_Deep_Indirect_ReturnNotRepeat()
        {
            var r = A + (C + A) + C;
            var expression = A.AsExpression(f => f.Children, true);
            Assert.Equal(5, expression.Count);
            Assert.Equal("A + (C + A) + (C + A)", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpression_Surface_Direct_ReturnNotRepeat()
        {
            var r = A + A;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + A", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpression_Surface_Indirect_ReturnNotRepeat()
        {
            var r = A + (C + A) + C;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(4, expression.Count);
            Assert.Equal("A + (C + A) + C", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpression_Surface_IndirectMultiLevel_ReturnNotRepeat()
        {
            var r = A + (B + (C + (D + B))) + C;
            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(6, expression.Count);
            Assert.Equal("A + (B + (C + (D + B))) + C", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpression_Deep_IndirectMultiLevel_ReturnNotRepeat()
        {
            var r = A + (B + (C + (D + B))) + C;
            var expression = A.AsExpression(f => f.Children, true);
            var serializer = expression.DefaultSerializer as CircularEntityExpressionSerializer<CircularEntity>;
            Assert.Equal(9, expression.Count);
            Assert.Equal("A + (B + (C + (D + B))) + (C + (D + (B + C)))", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpression_Deep_CustomSerializeItem_ReturnExpression()
        {
            var r = A + (B + (C + (D + B))) + C;
            var expression = A.AsExpression(f => f.Children, true);
            var serializer = expression.DefaultSerializer as CircularEntityExpressionSerializer<CircularEntity>;
            serializer.EntityNameCallback = (item) => item.Name.ToLower();
            Assert.Equal(9, expression.Count);
            Assert.Equal("a + (b + (c + (d + b))) + (c + (d + (b + c)))", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpressionConstructor_Deep_CustomSerializeItem_ReturnExpression()
        {
            var r = A + (B + (C + (D + B))) + C;
            var expression = A.AsExpression(f => f.Children, (item) => item.Name.ToLower(), true);
            Assert.Equal(9, expression.Count);
            Assert.Equal("a + (b + (c + (d + b))) + (c + (d + (b + c)))", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateCiclicalExpression_NullEntity_SerializeNullFormat()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity(null);

            var r = A + B;

            var expression = A.AsExpression(f => f.Children);
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + null", expression.DefaultSerializer.Serialize());
        }
    }
}