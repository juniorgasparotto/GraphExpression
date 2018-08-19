using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class DynamicItem : ComplexEntity
    {
        public string Property { get; private set; }

        public DynamicItem(Expression<object> expression, object parent, string property, object value) 
            : base(expression)
        {
            this.Property = property;
            this.Entity = value;
        }
    }
}
