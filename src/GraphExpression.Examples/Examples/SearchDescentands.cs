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
        [Action(Help = "Search with references - get all properties from class2")]
        public void Descendants1()
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

            // filter
            Expression<object> expression = model.AsExpression();
            EntityItem<object> root = expression.First();
            IEnumerable<EntityItem<object>> result = root.Descendants(2, 2);
            foreach (EntityItem<object> item in result)
                System.Console.WriteLine(GetEntity(item));
        }

        [Action(Help = "Search  with references - get all properties from class2")]
        public void Children()
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

            // filter
            Expression<object> expression = model.AsExpression();
            EntityItem<object> root = expression.First();
            IEnumerable<EntityItem<object>> result = root.Children();
            foreach (EntityItem<object> item in result)
                System.Console.WriteLine(GetEntity(item));
        }
    }
}
