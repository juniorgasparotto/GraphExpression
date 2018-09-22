using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GraphExpression.Tests.MatrixGenerator
{
    public class CollectionItemTest
    {
        private class MyList : List<string>
        {
            public int MyProp { get; set; }
        }

        private class MyList2 : List<StringBuilder>
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
            var expected = $"{{{values.GetType().Name}.{values.GetHashCode()}}} + {{[0]: \"value1\"}} + {{[1]: \"value2\"}} + {{[2]: \"value3\"}} + {{@Capacity: 4}} + {{@Count: 3}}";
            Assert.Equal(6, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<CollectionItemEntity>(expression[1]);
            Assert.IsType<CollectionItemEntity>(expression[2]);
            Assert.IsType<CollectionItemEntity>(expression[3]);
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
            var expected = $"{{{values.GetType().Name}.{values.GetHashCode()}}} + {{[0]: \"value1\"}} + {{[1]: \"value2\"}} + {{[2]: \"value3\"}} + {{@MyProp: 0}} + {{@Capacity: 4}} + {{@Count: 3}}";
            Assert.Equal(7, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<CollectionItemEntity>(expression[1]);
            Assert.IsType<CollectionItemEntity>(expression[2]);
            Assert.IsType<CollectionItemEntity>(expression[3]);
            Assert.IsType<PropertyEntity>(expression[4]);
            Assert.IsType<PropertyEntity>(expression[5]);
            Assert.IsType<PropertyEntity>(expression[6]);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateComplexList_ReturnExpressionAsString()
        {
            var values = new MyList2
            {
                MyProp = 100
            };


            values.Add(new StringBuilder("value1"));
            values.Add(new StringBuilder("value2"));

            var expression = values.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{values.GetType().Name}.{values.GetHashCode()}}} + {{[0].{values[0].GetHashCode()}}} + {{[1].{values[1].GetHashCode()}}} + {{@MyProp: 100}} + {{@Capacity: 4}} + {{@Count: 2}}";
            Assert.Equal(6, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<CollectionItemEntity>(expression[1]);
            Assert.IsType<CollectionItemEntity>(expression[2]);
            Assert.IsType<PropertyEntity>(expression[3]);
            Assert.IsType<PropertyEntity>(expression[4]);
            Assert.IsType<PropertyEntity>(expression[5]);
            Assert.Equal(expected, result);
        }
    }
}
