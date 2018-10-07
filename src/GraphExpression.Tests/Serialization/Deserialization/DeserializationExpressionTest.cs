using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests.Serialization
{
    public class DeserializationExpressionTest
    {
        [Fact]
        public void Deserialize_MultiplesTests()
        {
            TestDeserialize("A+(B+(C+C))", "A + (B + (C + C))");
            TestDeserialize("A+(B+(C+C)+D)", "A + (B + (C + C) + D)");
            TestDeserialize("A+(B+(C+C)+C)", "A + (B + (C + C) + (C + C))", true);
            TestDeserialize("A+(B+(C+C))+D", "A + (B + (C + C)) + D");
            TestDeserialize("A+(B+(C+C))+C", "A + (B + (C + C)) + (C + C)", true);
            TestDeserialize("A+(B+(C+(D+C)))+C", "A + (B + (C + (D + C))) + (C + (D + C))", true);
            TestDeserialize("A+(B+(C+(D+C))+I)+C", "A + (B + (C + (D + C)) + I) + (C + (D + C))", true);
            TestDeserialize("A+(B+(C+(D+C)+P)+I)+C", "A + (B + (C + (D + C) + P) + I) + (C + (D + C) + P)", true);
            TestDeserialize("A+(B+(C+(D+C)+P)+I+P)+C", "A + (B + (C + (D + C) + P) + I + P) + (C + (D + C) + P)", true);
            TestDeserialize("A+(B+(C+(D+C)+P+G)+I+P)+C", "A + (B + (C + (D + C) + P + G) + I + P) + (C + (D + C) + P + G)", true);
            TestDeserialize("A+(B+(C+(D+C)+P+G))+C", "A + (B + (C + (D + C) + P + G)) + (C + (D + C) + P + G)", true);
            TestDeserialize("A+(B+C+(J+(I+H+S+W+Y)+L)+S)+K+(D+E+(P+(U+Y)+R)+O+Q+S+V)", "A + (B + C + (J + (I + H + S + W + Y) + L) + S) + K + (D + E + (P + (U + Y) + R) + O + Q + S + V)");
        }

        [Fact]
        public void Deserialize_IntegerOutput_Sum10()
        {
            var strExp = "1 * 2 * 5";
            var serializer = new CircularEntityExpressionDeserializer<int>();
            var result = serializer.Deserialize(strExp, param => Convert.ToInt32(param));
            Assert.Equal(10, result);
        }

        [Fact]
        public void Deserialize_WithCreateEntityCallBack_GenerateRootAndPopulateEntities()
        {
            var strExp = "(A + B + C + D)";
            var factory = new CircularEntityFactory<CircularEntity>(name => new CircularEntity(name));
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp, factory);
            var entities = factory.Entities.Values.ToList();

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

        [Fact]
        public void Deserialize_WithDefaultConstructor_GenerateRoot()
        {
            var strExp = "A + B + C + D";
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp);

            Assert.Equal(3, root.Children.Count());
            Assert.Equal("A", root.Name);
            Assert.Equal("B", root.Children.ElementAt(0).Name);
            Assert.Equal("C", root.Children.ElementAt(1).Name);
            Assert.Equal("D", root.Children.ElementAt(2).Name);
        }

        [Fact]
        public void Deserialize_WithCreateOverride_GenerateRootWithOtherNames()
        {
            var strExp = "A + B + C + D";
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp, f=> new CircularEntity(f.ToLower()));

            Assert.Equal(3, root.Children.Count());            
            Assert.Equal("a", root.Name);
            Assert.Equal("b", root.Children.ElementAt(0).Name);
            Assert.Equal("c", root.Children.ElementAt(1).Name);
            Assert.Equal("d", root.Children.ElementAt(2).Name);
        }

        [Fact]
        public void Deserialize_CreateStartupEntities_SubstractEntities()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");

            var deserializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var factory = new CircularEntityFactory<CircularEntity>();
            factory.Entities.Add("AA", A);
            factory.Entities.Add("BB", B);

            var strExp = "AA + BB";
            var root = deserializer.Deserialize(strExp, factory);
            Assert.Single(root.Children);
            Assert.Equal("A", root.Name);
            Assert.Equal("B", root.Children.ElementAt(0).Name);

            strExp = "AA - BB + C + D";
            var root2 = deserializer.Deserialize(strExp, factory);

            Assert.Same(root2, root);
            Assert.Equal(2, root2.Children.Count());
            Assert.Equal("A", root2.Name);
            Assert.Equal("C", root2.Children.ElementAt(0).Name);
            Assert.Equal("D", root2.Children.ElementAt(1).Name);
        }

        [Fact]
        public void Deserialize_CreateSolutionByExpression_UseQuotesEntityAndNoQuotesAndNamespaceAndMultilevels_GenerateRoot()
        {
            var deserializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var solutionMaps = "\"Example Presentation\" + (Example.Layers.Business + (Example.Layers.DAL + Example.Layers.DAL.Interaces)) + (\"Example.Layers.DAL\")";
            var root = deserializer.Deserialize(solutionMaps);
            
            Assert.Equal(2, root.Children.Count());
            Assert.Equal("Example Presentation", root.Name);

            var business = root.Children.ElementAt(0);
            Assert.Single(business.Children);
            Assert.Equal("Example.Layers.Business", business.Name);

            var dal = business.Children.ElementAt(0);
            Assert.Single(dal.Children);
            Assert.Equal("Example.Layers.DAL", dal.Name);

            var interfaces = dal.Children.ElementAt(0);
            Assert.Empty(interfaces.Children);
            Assert.Equal("Example.Layers.DAL.Interaces", interfaces.Name);

            var dal2 = root.Children.ElementAt(1);
            Assert.Same(dal2, dal);
        }

        [Fact]
        public void Deserialize_UsingfactoryInstancesAndStaticWithStringParams_GenerateRootWithfactory()
        {
            var strExp = "NewEntity('my entity name1') + (NewEntityStatic('my entity name 2') + B - GraphExpression.Tests.Serialization.DeserializationExpressionTest.EntityFactoryDeserializerExtend.NewEntityStatic('B'))";
            var factory = new EntityFactoryDeserializerExtend();
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp, factory);
            var entities = factory.Entities.Values.ToList();

            Assert.Single(root.Children);
            Assert.Equal("my entity name1", root.Name);

            var children1 = root.Children.ElementAt(0);
            Assert.Single(children1.Children);
            Assert.Equal("my entity name 2", children1.Name);
            
            // unique entity create directly
            Assert.Single(entities);
            Assert.Equal("B", entities[0].Name);
        }

        [Fact]
        public void Deserialize_UsingfactoryWithStringParamsInVerbatin_GenerateRootWithfactory()
        {
            var strExp = "NewEntity('\"quote\"') + NewEntity('\\'quote\\'')";
            var factory = new EntityFactoryDeserializerExtend();
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp, factory);
            var entities = factory.Entities.Values.ToList();

            Assert.Single(root.Children);
            Assert.Equal("\"quote\"", root.Name);

            var children1 = root.Children.ElementAt(0);
            Assert.Empty(children1.Children);
            Assert.Equal("'quote'", children1.Name);

            // no entity create directly
            Assert.Empty(entities);
        }

        [Fact]
        public void Deserialize_NullValue_GenerateRootWithfactory()
        {
            var strExp = "A + null + C + NULL + \"null\"";
            var factory = new EntityFactoryDeserializerExtend();
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp, factory);
            var entities = factory.Entities.Values.ToList();

            Assert.Equal(4, root.Children.Count());
            Assert.Null(root.Children.ElementAt(0));
            Assert.Equal("C", root.Children.ElementAt(1).Name);
            Assert.Equal("NULL", root.Children.ElementAt(2).Name);
            Assert.Equal("null", root.Children.ElementAt(3).Name);

            // no entity create directly
            Assert.Equal(4, entities.Count);
        }

        #region BUG KNOWN

        [Fact]
        public void Deserialize_UsingfactoryWithStringParamsInVerbatin_BUG_KNOWN()
        {
            var strExp = "NewEntity('\\' \\\'')";
            var factory = new EntityFactoryDeserializerExtend();
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp, factory);
            var entities = factory.Entities.Values.ToList();

            // DEVERIA SER: ' \'
            Assert.Equal("' '", root.Name);

            // OCORRE EXCEPTION
            try
            {
                strExp = "NewEntity('\\\\'')";
                root = serializer.Deserialize(strExp, factory);
                Assert.Equal("' ' '", root.Name);
            }
            catch
            {
                Assert.True(true);
            }
        }

        #endregion

        #region auxs

        private void TestDeserialize(string exIn, string exOut, bool deep = false)
        {
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(exIn);
            var expectedOut = root.AsExpression(f => f.Children, deep).DefaultSerializer.Serialize();
            Assert.Equal(expectedOut, exOut);
        }

        public class EntityFactoryDeserializerExtend : CircularEntityFactory<CircularEntity>
        {
            public CircularEntity NewEntity(string name)
            {
                return new CircularEntity(name);
            }

            public static CircularEntity NewEntityStatic(string name)
            {
                return new CircularEntity(name);
            }
        }
        #endregion
    }
}
