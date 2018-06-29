using ExpressionGraph.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class ConvertTests : EntitiesData
    {
        [Fact]
        public void ConvertTest_Surface_CheckAllItems()
        {
            
        }

        private static void TestItem(EntityItem<Entity> item, string name, int index)
        {
            Assert.Equal(name, item.Entity.Name);
            Assert.Equal(index, item.Index);
        }
    }
}
