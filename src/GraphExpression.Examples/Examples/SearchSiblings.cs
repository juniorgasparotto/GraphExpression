using GraphExpression.Examples.Models;
using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression.Examples
{
    public partial class Examples
    {
        [Action(Help = "Search Siblings - get all from next/previous")]
        public void Siblings1()
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

            // Get Siblings1 from C - Start direction
            System.Console.WriteLine("-> Start direction");
            Expression<object> expression = model.AsExpression();
            var C = expression.Where(f => f.Entity as string == "C");
            IEnumerable<EntityItem<object>> result = C.Siblings(direction: SiblingDirection.Start);
            foreach (EntityItem<object> item in result)
                System.Console.WriteLine(item.ToString());

            // Get Siblings1 from C - Next direction            
            System.Console.WriteLine("-> Next direction");
            result = C.Siblings(direction: SiblingDirection.Next);
            foreach (EntityItem<object> item in result)
                System.Console.WriteLine(item.ToString());

            // Get Siblings1 from C - Previous direction
            System.Console.WriteLine("-> Previous direction");
            result = C.Siblings(direction: SiblingDirection.Previous);
            foreach (EntityItem<object> item in result)
                System.Console.WriteLine(item.ToString());
        }
    }
}
