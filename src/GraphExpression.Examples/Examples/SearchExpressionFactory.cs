using GraphExpression.Examples.Models;
using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphExpression.Examples
{
    public partial class Examples
    {
        [Action(Help = "")]
        public void ComplexExpressionFactory()
        {
            var factory = new ComplexExpressionFactory();
            factory.MemberReaders.Add(new MethodReader());
            var model = new Model();
            var expression = model.AsExpression(factory);

            foreach(ComplexEntity item in expression)
            {
                var output = GetEntity(item);
                if (item is MethodEntity method)
                    output = $"MethodEntity.{method.MethodInfo.Name}({method.Parameters[0]}, {method.Parameters[1]})";
                System.Console.WriteLine(output);
            }
        }

        public class MethodReader : IMemberReader
        {
            public IEnumerable<ComplexEntity> GetMembers(ComplexExpressionFactory factory, GraphExpression.Expression<object> expression, object entity)
            {
                if (entity is Model)
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

        private class MethodEntity : ComplexEntity
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

        private class Model
        {
            public string HelloWorld(string val1, string val2)
            {
                return $"{val1}-{val2}";
            }
        }
    }
}
