using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class DictionaryTest
    {
        [Fact]
        public void CreateDictionary_ReturnExpressionAsString()
        {
            var dic = new Dictionary<string, string>();
            dic["key1"] = "value1";
            dic["key2"] = "value2";
            dic["key3"] = "value3";

            var expression = dic.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{dic.GetHashCode()}}} + {{[{dic.Keys.ElementAt(0).GetHashCode()}]: \"value1\"}} + {{[{dic.Keys.ElementAt(1).GetHashCode()}]: \"value2\"}} + {{[{dic.Keys.ElementAt(2).GetHashCode()}]: \"value3\"}}";
            Assert.Equal(4, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<DictionaryItemEntity>(expression[1]);
            Assert.IsType<DictionaryItemEntity>(expression[2]);
            Assert.IsType<DictionaryItemEntity>(expression[3]);
            Assert.Equal(expected, result);
        }
    }
}
