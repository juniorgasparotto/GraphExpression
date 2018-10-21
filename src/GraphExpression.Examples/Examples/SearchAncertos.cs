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
        [Action(Help = "Search ancertos - get all from last item")]
        public void Ancertor1()
        {
            // create a simple object
            var model = new Class1
            {
                Class1_Prop1 = "Value1",
                Class1_Prop2 = new Class2()
                {
                    Class2_Prop2 = "ValueChild",
                    Class2_Field1 = 1000
                }
            };

            // transversal navigation
            Expression<object> expression = model.AsExpression();
            EntityItem<object> lastItem = expression.Last();
            IEnumerable<EntityItem<object>> result = lastItem.Ancestors();

            foreach (EntityItem<object> item in result)
                System.Console.WriteLine(GetEntity(item));

            System.Console.WriteLine("-> Parent");

            // Get first ancertos (parent)
            result = lastItem.Ancestors((item, depth) => depth == 1);

            foreach (var item in result)
                System.Console.WriteLine(GetEntity(item));
        }
    }
}
