
using System.Collections.Generic;
using Xunit;

namespace GraphExpression.Tests
{
    public class ListItemTest
    {
        private class MyList : List<string>
        {
            public int MyProp { get; set; }
        }

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

        [Fact]
        public void CreateCustomList_ReturnExpressionAsString()
        {
            var values = new MyList
            {
                "value1",
                "value2",
                "value3"
            };

            var expression = values.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{values.GetHashCode()}}} + {{[0]: \"value1\"}} + {{[1]: \"value2\"}} + {{[2]: \"value3\"}} + {{@MyProp: 0}} + {{@Capacity: 4}} + {{@Count: 3}}";
            Assert.Equal(7, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<ListItemEntity>(expression[1]);
            Assert.IsType<ListItemEntity>(expression[2]);
            Assert.IsType<ListItemEntity>(expression[3]);
            Assert.IsType<PropertyEntity>(expression[4]);
            Assert.IsType<PropertyEntity>(expression[5]);
            Assert.IsType<PropertyEntity>(expression[6]);
            Assert.Equal(expected, result);
        }

    }
}
