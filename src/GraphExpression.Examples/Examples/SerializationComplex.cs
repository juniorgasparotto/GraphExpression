using GraphExpression.Examples.Models;
using GraphExpression.Serialization;
using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression.Examples
{
    public partial class Examples
    {
        [Action(Help = "")]
        public void SerializationComplex1()
        {
            // create a simple object
            var model = new
            {
                A = "A",
                B = "B",
                C = "C",
                D = "D",
                E = "E",
            };

            var expression = model.AsExpression();
            var serialization = new ComplexEntityExpressionSerializer(expression);
            var expressionAsString = serialization.Serialize();
            System.Console.WriteLine(expressionAsString);
        }

        [Action(Help = "")]
        public void SerializationComplex2()
        {
            // create a simple object
            var model = new
            {
                A = "A",
                B = "B",
                C = "C",
                D = "D",
                E = "BIG VALUE",
            };

            var expression = model.AsExpression();
            var serialization = new ComplexEntityExpressionSerializer(expression);
            serialization.EncloseParenthesisInRoot = true;
            serialization.ValueFormatter = new TruncateFormatter(3);
            serialization.ShowType = ShowTypeOptions.TypeName;
            var expressionAsString = serialization.Serialize();
            System.Console.WriteLine(expressionAsString);
        }

        [Action(Help = "")]
        public void SerializationComplex3()
        {
            var factory = new ComplexExpressionFactory();
            factory.MemberReaders.Add(new MethodReader());

            var model = new Model();
            var expression = model.AsExpression(factory);
            var serialization = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            serialization.ItemsSerialize.Add(new MethodSerialize());
            System.Console.WriteLine(serialization.Serialize());
        }

        #region auxs

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
     
        #endregion
    }
}
