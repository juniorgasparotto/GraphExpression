using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using System;

namespace GraphExpression.Examples
{
    public partial class Examples : Command
    {
        [Action(Help = "Show all public properties/fields in all levels from the root model")]
        public void GraphCircular()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");
            var C = new CircularEntity("C");
            var D = new CircularEntity("D");

            // ACTION: ADD
            A = A + B + (C + D);

            // PRINT 'A'
            Expression<CircularEntity> expression = A.AsExpression(e => e.Children, entityNameCallback: o => o.Name);
            foreach (EntityItem<CircularEntity> item in expression)
            {
                var ident = new string(' ', item.Level * 2);
                var output = $"{ident}[{item.Index}] => Item: {item.Entity.Name}, Parent: {item.Parent?.Entity.Name}, Previous: {item.Previous?.Entity.Name}, Next: {item.Next?.Entity.Name}, Level: {item.Level}";
                System.Console.WriteLine(output);
            }

            System.Console.WriteLine(expression.DefaultSerializer.Serialize());


            // ACTION: REMOVE
            C = C - D;

            // PRINT 'A' AGAIN
            expression = A.AsExpression(e => e.Children, entityNameCallback: o => o.Name);
            foreach (EntityItem<CircularEntity> item in expression)
            {
                var ident = new string(' ', item.Level * 2);
                var output = $"{ident}[{item.Index}] => Item: {item.Entity.Name}, Parent: {item.Parent?.Entity.Name}, Previous: {item.Previous?.Entity.Name}, Next: {item.Next?.Entity.Name}, Level: {item.Level}";
                System.Console.WriteLine(output);
            }

            // PRINT EXPRESSION
            System.Console.WriteLine(expression.DefaultSerializer.Serialize());
        }
    }
}
