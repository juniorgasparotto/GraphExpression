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
        public void DeserializationComplex1()
        {
            var expressionAsString = "\"Int32[].1\" + \"[0]: 1\" + \"[1]: 2\" + \"[2]: 3\"";
            var deserializer = new ComplexEntityExpressionDeserializer();
            var array = deserializer.Deserialize<int[]>(expressionAsString);
            System.Console.WriteLine(array[0]);
            System.Console.WriteLine(array[1]);
            System.Console.WriteLine(array[2]);
        }
    }
}
