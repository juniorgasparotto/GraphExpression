using Xunit;

namespace GraphExpression.Tests
{
    public class ArrayTest
    {
        [Fact]
        public void CreateDirectArray_ReturnExpressionAsString()
        {
            string[] values = new string[]
            {
                "value1",
                "value2",
                "value3"
            };

            var expression = values.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{values.GetHashCode()}}} + {{[0]: \"value1\"}} + {{[1]: \"value2\"}} + {{[2]: \"value3\"}}";
            Assert.Equal(expected, result);
        }
    }
}
