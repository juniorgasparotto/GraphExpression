using GraphExpression.Serialization;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Serialization
{
    public class DeserializationExpressionTest
    {
        [Fact]
        public void Deserialize_WithRootParenthesis()
        {
            var strExp = "(A + B + C + D)";
            var container = new ContainerDeserializer<CircularEntity>(name => new CircularEntity(name));
            var serializer = new ExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp, container);
            var entities = container.Entities.Values.ToList();

            Assert.Equal(4, entities.Count);
            Assert.Equal(3, entities[0].Children.Count());
            Assert.Same(root, entities.First());
            Assert.Equal("A", entities[0].Name);
            Assert.Equal("B", entities[0].Children.ElementAt(0).Name);
            Assert.Equal("C", entities[0].Children.ElementAt(1).Name);
            Assert.Equal("D", entities[0].Children.ElementAt(2).Name);

            Assert.Equal("B", entities[1].Name);
            Assert.Empty(entities[1].Children);

            Assert.Equal("C", entities[2].Name);
            Assert.Empty(entities[1].Children);

            Assert.Equal("D", entities[3].Name);
            Assert.Empty(entities[1].Children);
        }
    }
}
