using Xunit;

namespace GraphExpression.Tests.MatrixGenerator
{
    public class AnonimousTest
    {
        [Fact]
        public void CreateAnonimous_ReturnExpressionAsString()
        {
            var anonimous = new
            {
                Prop1 = 124m,
                Prop2 = (int?)null
            };

            var expression = anonimous.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"\"{anonimous.GetType().Name}.{anonimous.GetHashCode()}\" + \"@Prop1: 124\" + \"@Prop2: null\"";
            Assert.Equal(3, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<PropertyEntity>(expression[1]);
            Assert.IsType<PropertyEntity>(expression[2]);
            Assert.Equal(expected, result);
        }
    }
}
