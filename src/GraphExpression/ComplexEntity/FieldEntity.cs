using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class FieldEntity : ComplexEntity
    {
        public FieldInfo Field { get; private set; }

        public FieldEntity(ComplexEntity parent, FieldInfo field)
        {
            this.Field = field;
            this.Entity = field.GetValue(parent.Entity);
        }
    }
}
