using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class DynamicItemEntity : ComplexEntity
    {
        public string Property { get; private set; }

        public DynamicItemEntity(Expression<object> expression, string property, object value) 
            : base(expression)
        {
            this.Property = property;
            this.Entity = value;
        }
    }
}
