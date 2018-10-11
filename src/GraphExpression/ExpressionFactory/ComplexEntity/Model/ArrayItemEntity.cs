using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Class model representing an array
    /// </summary>
    public class ArrayItemEntity : ComplexEntity
    {
        /// <summary>
        /// Indexes of array
        /// </summary>
        public int[] Indexes { get; private set; }

        /// <summary>
        /// Create a array model
        /// </summary>
        /// <param name="expression">Expression container</param>
        /// <param name="index">Index of array item</param>
        /// <param name="value">Item value</param>
        public ArrayItemEntity(Expression<object> expression, int index, object value)
            : base(expression)
        {
            this.Indexes = new int[] { index };
            this.Entity = value;
        }

        /// <summary>
        /// Create a array model
        /// </summary>
        /// <param name="expression">Expression container</param>
        /// <param name="indexes">Item index</param>
        /// <param name="value">Item value</param>
        public ArrayItemEntity(Expression<object> expression, int[] indexes, object value)
            : base(expression)
        {
            this.Indexes = indexes;
            this.Entity = value;
        }
    }
}
