using GraphExpression.Serialization;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class DeserializationExpressionTest
    {
        [Fact]
        public void Deserialize_WithRootParenthesis()
        {
            var strExp = "(A + B + C + D)";
            var serializer = new DeserializationExpression();
            var entity = serializer.FromString(strExp).ToList();

            Assert.Equal(4, entity.Count);
            Assert.Equal(3, entity[0].Children.Count());
            Assert.Equal("A", entity[0].Name);
            Assert.Equal("B", entity[0].Children.ElementAt(0).Name);
            Assert.Equal("C", entity[0].Children.ElementAt(1).Name);
            Assert.Equal("D", entity[0].Children.ElementAt(2).Name);

            Assert.Equal("B", entity[1].Name);
            Assert.Empty(entity[1].Children);

            Assert.Equal("C", entity[2].Name);
            Assert.Empty(entity[1].Children);

            Assert.Equal("D", entity[3].Name);
            Assert.Empty(entity[1].Children);
        }
    }
}
