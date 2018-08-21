
using System.Collections.Generic;
using Xunit;

namespace GraphExpression.Tests
{
    public class ListItemTest
    {
        [Fact]
        public void CreateList_ReturnExpressionAsString()
        {
            var values = new List<string>
            {
                "value1",
                "value2",
                "value3"
            };

            var expression = values.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{values.GetHashCode()}}} + {{[0]: \"value1\"}} + {{[1]: \"value2\"}} + {{[2]: \"value3\"}}";
            Assert.Equal(4, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<ListItemEntity>(expression[1]);
            Assert.IsType<ListItemEntity>(expression[2]);
            Assert.IsType<ListItemEntity>(expression[3]);
            Assert.Equal(expected, result);
        }
    }
}
