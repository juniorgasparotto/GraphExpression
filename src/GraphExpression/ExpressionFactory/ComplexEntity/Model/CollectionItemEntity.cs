namespace GraphExpression
{
    /// <summary>
    /// Class model representing an collection
    /// </summary>
    public class CollectionItemEntity : ComplexEntity
    {
        /// <summary>
        /// Item index 
        /// </summary>
        public long Key { get; private set; }

        /// <summary>
        /// Create a collection model
        /// </summary>
        /// <param name="expression">Expression container</param>
        /// <param name="key">Item index</param>
        /// <param name="value">Item value</param>
        public CollectionItemEntity(Expression<object> expression, long key, object value) 
            : base(expression)
        {
            this.Key = key;
            this.Entity = value;
        }
    }
}
