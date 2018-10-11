using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace GraphExpression.Tests.MatrixGenerator
{
    public class ReadersCustomTest
    {
        public class MethodEntity : ComplexEntity
        {
            public MethodInfo MethodInfo { get; }
            public object[] Parameters { get; }

            public MethodEntity(Expression<object> expression, MethodInfo methodInfo, object[] parameters, object value)
                : base(expression)
            {
                this.MethodInfo = methodInfo;
                this.Parameters = parameters;
                this.Entity = value;
            }
        }

        public class MethodReader : IMemberReader
        {
            public IEnumerable<ComplexEntity> GetMembers(ComplexExpressionFactory factory, GraphExpression.Expression<object> expression, object entity)
            {
                if (entity is Test)
                {
                    var method = entity
                        .GetType()
                        .GetMethods().Where(f => f.Name == "HelloWorld")
                        .First();

                    var parameters = new object[] { "value1", "value2" };
                    var methodValue = method.Invoke(entity, parameters);
                    yield return new MethodEntity(expression, method, parameters, methodValue);
                }
            }
        }

        public class MethodSerialize : IEntitySerialize
        {
            public string Symbol { get; set; } = null;

            public bool CanSerialize(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
            {
                return item is MethodEntity;
            }

            public (Type Type, string ContainerName) GetSerializeInfo(ComplexEntityExpressionSerializer serializer, EntityItem<object> item)
            {
                var cast = (MethodEntity)item;
                return (
                    item.Entity?.GetType(),
                    $"{cast.MethodInfo.Name}({string.Join(",", cast.Parameters)})"
                );
            }
        }

        public class Test : List<string>
        {
            public string HelloWorld(string val1, string val2)
            {
                return $"{val1}-{val2}";
            }
        }

        [Fact]
        public void CreateCustomSerialize_ReturnExpressionAsString()
        {
            var factory = new ComplexExpressionFactory();
            factory.MemberReaders.Add(new MethodReader());

            var test = new Test()
            {
                "value1",
            };

            var expression = test.AsExpression(factory);
            var serialization = Utils.GetSerialization(expression);
            serialization.ItemsSerialize.Add(new MethodSerialize());

            var result = serialization.Serialize();
            var expected = $"\"{test.GetType().Name}.{test.GetHashCode()}\"" + " + \"[0]: value1\" + \"Capacity: 4\" + \"Count: 1\" + \"HelloWorld(value1,value2): value1-value2\"";
            Assert.Equal(5, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<CollectionItemEntity>(expression[1]);
            Assert.IsType<PropertyEntity>(expression[2]);
            Assert.IsType<PropertyEntity>(expression[3]);
            Assert.IsType<MethodEntity>(expression[4]);
            Assert.Equal(expected, result);
        }
    }
}