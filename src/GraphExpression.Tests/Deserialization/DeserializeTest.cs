using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class DeserializeTest
    {
        [Fact]
        public void Deserialize_WithRootParenthesis()
        {
            var strExp = "(A + B + C + D)";
            var serializer = new ExpressionSerializer();
            var entity = serializer.FromString(strExp).ToList();

            Assert.Equal(4, entity.Count);
            Assert.Equal(3, entity[0].Children.Count());
            Assert.Equal("A", entity[0].Identity);
            Assert.Equal("B", entity[0].Children.ElementAt(0).Identity);
            Assert.Equal("C", entity[0].Children.ElementAt(1).Identity);
            Assert.Equal("D", entity[0].Children.ElementAt(2).Identity);

            Assert.Equal("B", entity[1].Identity);
            Assert.Empty(entity[1].Children);

            Assert.Equal("C", entity[2].Identity);
            Assert.Empty(entity[1].Children);

            Assert.Equal("D", entity[3].Identity);
            Assert.Empty(entity[1].Children);
        }
    }
}
