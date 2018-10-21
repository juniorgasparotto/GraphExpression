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
        [Action(Help = "Search  without references - get all properties from class2")]
        public void Search1()
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
            IEnumerable<EntityItem<object>> result = expression.Descendants(e => e is PropertyEntity && e.Parent.Entity is Class2);
            foreach (EntityItem<object> item in result)
                System.Console.WriteLine(GetEntity(item));
        }

        [Action(Help = "Search  with references - get all properties from class2")]
        public void Search2()
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
            IEnumerable<EntityItem<object>> result = root.Descendants(e => e is PropertyEntity && e.Parent.Entity is Class2);
            foreach (EntityItem<object> item in result)
                System.Console.WriteLine(GetEntity(item));
        }
    }
}
