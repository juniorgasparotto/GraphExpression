using System;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Create a circular expression serializer
    /// </summary>
    /// <typeparam name="T">Type of circular entity</typeparam>
    public class CircularEntityExpressionSerializer<T> : ExpressionSerializerBase<T>
    {
        private readonly Expression<T> expression;

        /// <summary>
        /// Delegates the action of retrieving the entity name that will be used in the default serialization and also in the debug.
        /// </summary>
        public Func<T, object> EntityNameCallback { get; set; }

        /// <summary>
        /// Create a circular expression serializer
        /// </summary>
        /// <param name="expression">Expression to serialize</param>
        /// <param name="entityNameCallback">Delegates the action of retrieving the entity name that will be used in the default serialization and also in the debug.</param>
        public CircularEntityExpressionSerializer(Expression<T> expression, Func<T, object> entityNameCallback)
            : base(expression)
        {
            this.expression = expression;
            this.EntityNameCallback = entityNameCallback;
            this.ForceQuoteEvenWhenValidIdentified = false;
        }

        /// <summary>
        /// Serialize a unique EntityItem
        /// </summary>
        /// <param name="item">EntityItem to serialize</param>
        /// <returns>Entity item as string</returns>
        public override string SerializeItem(EntityItem<T> item)
        {
            string value = null;

            if (item.Entity != null)
                value = ValueFormatter.Format(item.Entity.GetType(), EntityNameCallback(item.Entity));

            return value;
        }
    }
}
