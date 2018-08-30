using System.Reflection;

namespace GraphExpression
{
    public class FieldEntity : ComplexEntity
    {
        public FieldInfo Field { get; private set; }

        public FieldEntity(Expression<object> expression, object parent, FieldInfo field) 
            : base(expression)
        {
            this.Field = field;
            if (parent != null)
                this.Entity = field.GetValue(parent);
        }
    }
}
