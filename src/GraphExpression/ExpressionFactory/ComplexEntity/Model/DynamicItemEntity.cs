using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Class model representing a dictionary
    /// </summary>
    public class DynamicItemEntity : ComplexEntity
    {
        /// <summary>
        /// Dynamic property name
        /// </summary>
        public string Property { get; private set; }

        /// <summary>
        /// Create a dynamic item entity
        /// </summary>
        /// <param name="expression">Expression container</param>
        /// <param name="property">Dynamic property name</param>
        /// <param name="value">Dynamic value</param>
        public DynamicItemEntity(Expression<object> expression, string property, object value) 
            : base(expression)
        {
            this.Property = property;
            this.Entity = value;
        }
    }
}
