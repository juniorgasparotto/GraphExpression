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
        public void DeserializationCircular1()
        {
            var expressionAsString = "A + B + (C + D)";

            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var A = serializer.Deserialize(expressionAsString);

            // A
            System.Console.WriteLine(A.Name);

            // B
            System.Console.WriteLine(A.Children[0].Name);

            // C
            System.Console.WriteLine(A.Children[1].Name);

            // C - children 
            System.Console.WriteLine(A.Children[1].Children[0].Name);
        }

        [Action(Help = "")]
        public void DeserializationCircular2()
        {
            var strExp = "NewEntity('my entity name1') + NewEntity('my entity name2')";
            var factory = new CircularEntityFactoryExtend();
            var serializer = new CircularEntityExpressionDeserializer<CircularEntity>();
            var root = serializer.Deserialize(strExp, factory);
            var entities = factory.Entities.Values.ToList();

            System.Console.WriteLine(root.Name);
            System.Console.WriteLine(root.Children[0].Name);
        }

        #region auxs

        public class CircularEntityFactoryExtend : CircularEntityFactory<CircularEntity>
        {
            public CircularEntity NewEntity(string name)
            {
                return new CircularEntity(name);
            }
        }

        #endregion
    }
}
