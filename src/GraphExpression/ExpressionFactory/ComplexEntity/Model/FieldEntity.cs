using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Class model representing a field member
    /// </summary>
    public class FieldEntity : ComplexEntity
    {
        /// <summary>
        /// Field info
        /// </summary>
        public FieldInfo Field { get; private set; }

        /// <summary>
        /// Create a member field entity
        /// </summary>
        /// <param name="expression">Expression container</param>
        /// <param name="parent">Parent instance (FieldInfo container)</param>
        /// <param name="field">Field info</param>
        public FieldEntity(Expression<object> expression, object parent, FieldInfo field) 
            : base(expression)
        {
            this.Field = field;
            if (parent != null)
                this.Entity = field.GetValue(parent);
        }
    }
}
