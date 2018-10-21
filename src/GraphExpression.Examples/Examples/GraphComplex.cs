using GraphExpression.Examples.Models;
using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using System;

namespace GraphExpression.Examples
{
    public partial class Examples : Command
    {
        [Action(Help = "Show all public properties/fields in all levels from the root model")]
        public void GraphComplex()
        {
            // create a simple object
            var model = new Class1
            {
                Class1_Prop1 = "Value1",
                Class1_Prop2 = new Class2()
                {
                    Class2_Field1 = 1000,
                    Class2_Prop2 = "Value2"
                }
            };

            // transversal navigation
            Expression<object> expression = model.AsExpression();
            foreach (EntityItem<object> item in expression)
            {
                var ident = new string(' ', item.Level * 2);
                var output = $"{ident}[{item.Index}] => Item: {GetEntity(item)}, Parent: {GetEntity(item.Parent)}, Previous: {GetEntity(item.Previous)}, Next: {GetEntity(item.Next)}, Level: {item.Level}";
                System.Console.WriteLine(output);
            }

            // Serialize to expression
            System.Console.WriteLine(expression.DefaultSerializer.Serialize());
        }

        private string GetEntity(EntityItem<object> item)
        {   
            if (item is PropertyEntity prop)
                return $"Property.{prop.Property.Name}";

            if (item is FieldEntity field)
                return $"Field.{field.Field.Name}";

            if (item is ComplexEntity root)
                return root.Entity.GetType().Name;

            return null;
        }
    }
}
