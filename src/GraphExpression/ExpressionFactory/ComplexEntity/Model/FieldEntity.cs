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
        /// Check if field is static
        /// </summary>
        public bool IsStatic => Field.IsStatic;

        /// <summary>
        /// Check if field is public
        /// </summary>
        public bool IsPublic => Field.IsPublic;

        /// <summary>
        /// Check if field is private
        /// </summary>
        public bool IsPrivate => Field.IsPrivate;

        /// <summary>
        /// Check if field is protected
        /// </summary>
        public bool IsProtected => Field.IsFamily;

        /// <summary>
        /// Check if field is internal
        /// </summary>
        public bool IsInternal => Field.IsAssembly;

        /// <summary>
        /// Check if field is ready only
        /// </summary>
        public bool IsReadOnly => Field.IsInitOnly;

        /// <summary>
        /// Check if field is a constant
        /// </summary>
        public bool IsConstant => Field.IsLiteral && !Field.IsInitOnly;

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
