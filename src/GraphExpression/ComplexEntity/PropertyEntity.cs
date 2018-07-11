using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class PropertyEntity : ComplexEntity
    {
        public PropertyInfo Property { get; private set; }

        public PropertyEntity(Expression<object> expression, object parent, PropertyInfo property) 
            : base(expression)
        {
            this.Property = property;
            this.Entity = property.GetValue(parent);
        }
    }
}
