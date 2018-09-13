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
            var pathTest = expression.GraphInfo.Paths[0].ToString();
            var pathItemTest = expression.GraphInfo.Paths[0].Items[0].ToString();

            var expected = $"{{{values.GetHashCode()}}} + {{[0]: \"value1\"}} + {{[1]: \"value2\"}} + {{[2]: \"value3\"}}";
            Assert.Equal(4, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<ArrayItemEntity>(expression[1]);
            Assert.IsType<ArrayItemEntity>(expression[2]);
            Assert.IsType<ArrayItemEntity>(expression[3]);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateMultidimensionalArray_ReturnExpressionAsString()
        {
            // Three-dimensional array.
            var array3D = new int[2,2,3] 
            { 
                { { 1, 2, 3 }, { 4, 5, 6 } },
                { { 7, 8, 9 }, { 10, 11, 12 } }
            };

            var expression = array3D.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{array3D.GetHashCode()}}}" + " + {[0,0,0]: 1} + {[0,0,1]: 2} + {[0,0,2]: 3} + {[0,1,0]: 4} + {[0,1,1]: 5} + {[0,1,2]: 6} + {[1,0,0]: 7} + {[1,0,1]: 8} + {[1,0,2]: 9} + {[1,1,0]: 10} + {[1,1,1]: 11} + {[1,1,2]: 12}";
            Assert.Equal(13, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<ArrayItemEntity>(expression[1]);
            Assert.IsType<ArrayItemEntity>(expression[2]);
            Assert.IsType<ArrayItemEntity>(expression[3]);
            Assert.IsType<ArrayItemEntity>(expression[4]);
            Assert.IsType<ArrayItemEntity>(expression[5]);
            Assert.IsType<ArrayItemEntity>(expression[6]);
            Assert.IsType<ArrayItemEntity>(expression[7]);
            Assert.IsType<ArrayItemEntity>(expression[8]);
            Assert.IsType<ArrayItemEntity>(expression[9]);
            Assert.IsType<ArrayItemEntity>(expression[10]);
            Assert.IsType<ArrayItemEntity>(expression[11]);
            Assert.IsType<ArrayItemEntity>(expression[12]);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateJaggedArray_ReturnExpressionAsString()
        {
            // Three-dimensional array.
            int[][] jaggedArray = new int[2][];
            jaggedArray[0] = new int[2];
            jaggedArray[0][0] = 10;
            jaggedArray[0][1] = 20;
            jaggedArray[1] = new int[1];
            jaggedArray[1][0] = 10;

            var expression = jaggedArray.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{jaggedArray.GetHashCode()}}} + ({{[0].{jaggedArray[0].GetHashCode()}}} + {{[0]: 10}} + {{[1]: 20}}) + ({{[1].{jaggedArray[1].GetHashCode()}}} + {{[0]: 10}})";
            Assert.Equal(6, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<ArrayItemEntity>(expression[1]);
            Assert.IsType<ArrayItemEntity>(expression[2]);
            Assert.IsType<ArrayItemEntity>(expression[3]);
            Assert.IsType<ArrayItemEntity>(expression[4]);
            Assert.IsType<ArrayItemEntity>(expression[5]);
            Assert.Equal(expected, result);
        }
    }
}
