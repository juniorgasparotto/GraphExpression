using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class PropertyEntity : ComplexEntity
    {
        public PropertyInfo Property { get; private set; }

        public PropertyEntity(ComplexEntity parent, PropertyInfo property)
        {
            this.Property = property;
            this.Entity = property.GetValue(parent.Entity);
        }
    }
}
