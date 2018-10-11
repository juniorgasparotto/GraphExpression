using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Class model representing a property member
    /// </summary>
    public class PropertyEntity : ComplexEntity
    {
        /// <summary>
        /// Property info
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Create a member property entity
        /// </summary>
        /// <param name="expression">Expression container</param>
        /// <param name="parent">Parent instance (PropertyInfo container)<</param>
        /// <param name="property">Property info</param>
        public PropertyEntity(Expression<object> expression, object parent, PropertyInfo property) 
            : base(expression)
        {
            this.Property = property;
            if (parent != null)
                this.Entity = property.GetValue(parent);
        }
    }
}
