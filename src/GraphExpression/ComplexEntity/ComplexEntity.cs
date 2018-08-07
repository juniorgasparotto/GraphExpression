namespace GraphExpression
{
    public class ComplexEntity : EntityItem<object>
    {
        public ComplexEntity(Expression<object> expression)
            : base(expression)
        {
        }

        public ComplexEntity(Expression<object> expression, object entity)
            : base(expression)
        {
            this.Entity = entity;
        }
    }
}