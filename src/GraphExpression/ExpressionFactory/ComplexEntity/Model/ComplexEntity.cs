namespace GraphExpression
{
    /// <summary>
    /// Class model representing an complex item, All complex models must inherit from this class.
    /// </summary>
    public class ComplexEntity : EntityItem<object>
    {
        /// <summary>
        /// Create a complex entity item
        /// </summary>
        /// <param name="expression">Expression container</param>
        public ComplexEntity(Expression<object> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Create a complex entity item
        /// </summary>
        /// <param name="expression">Expression container</param>
        /// <param name="entity">Entity value</param>
        public ComplexEntity(Expression<object> expression, object entity)
            : base(expression)
        {
            this.Entity = entity;
        }
    }
}