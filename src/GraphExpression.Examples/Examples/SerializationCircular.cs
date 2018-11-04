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
        public void SerializationCircular1()
        {
            // create a simple object
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");

            A = A + B + (C + D);

            var expression = A.AsExpression(c => c.Children);
            var serialization = new CircularEntityExpressionSerializer<CircularEntity>(expression, f => f.Name);
            var expressionAsString = serialization.Serialize();
            System.Console.WriteLine(expressionAsString);
        }

        [Action(Help = "")]
        public void SerializationCircular2()
        {
            // create a simple object
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("BigName");

            A = A + B + (C + D);

            var expression = A.AsExpression(c => c.Children);            
            var serialization = new CircularEntityExpressionSerializer<CircularEntity>(expression, f => f.Name);
            serialization.EncloseParenthesisInRoot = true;
            serialization.ForceQuoteEvenWhenValidIdentified = true;
            serialization.ValueFormatter = new TruncateFormatter(3);

            var expressionAsString = serialization.Serialize();
            System.Console.WriteLine(expressionAsString);
        }
    }
}
