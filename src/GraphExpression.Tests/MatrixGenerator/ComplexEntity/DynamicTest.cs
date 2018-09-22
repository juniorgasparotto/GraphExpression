using System.Dynamic;
using Xunit;

namespace GraphExpression.Tests.MatrixGenerator
{
    public class DynamicTest
    {
        [Fact]
        public void CreateDynamicWithEmptyEntity_ReturnExpressionAsString()
        {
            dynamic dyn = new ExpandoObject();

            dyn.A = 123;
            dyn.B = new ExpandoObject();
            dyn.B.C = "abc";

            var expression = ((object)dyn).AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{dyn.GetType().Name}.{dyn.GetHashCode()}}} + {{@A: 123}} + ({{@B.{dyn.B.GetHashCode()}}} + {{@C: \"abc\"}})";

            Assert.Equal(4, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<DynamicItemEntity>(expression[1]);
            Assert.IsType<DynamicItemEntity>(expression[2]);
            Assert.IsType<DynamicItemEntity>(expression[3]);
            Assert.Equal(expected, result);
        }
    }
}
