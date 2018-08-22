using System.Collections.Generic;
using Xunit;

namespace GraphExpression.Tests
{
    public class PropertyTest
    {
        public class PropertyOnlySet
        {
            public string Prop1
            {
                set { }
            }
        }

        public class Index
        {
            public string this[int index]
            {
                get => "this value";
            }
        }

        [Fact]
        public void EntityWithIndexedProperty_ReturnOnlyRootEntityAndIgnoreThisProperty()
        {
            var a = new Index();

            var expression = a.AsExpression();
            Assert.Single(expression);
            Assert.IsType<ComplexEntity>(expression[0]);

            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{a.GetHashCode()}}}";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EntityWithPropertyOnlySet_ReturnOnlyRootEntityAndIgnoreThisProperty()
        {
            var a = new PropertyOnlySet();

            var expression = a.AsExpression();
            Assert.Single(expression);
            Assert.IsType<ComplexEntity>(expression[0]);

            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{a.GetHashCode()}}}";
            Assert.Equal(expected, result);
        }

    }
}
