using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    /// <summary>
    /// Interface that customizes the reading of an entity
    /// </summary>
    public interface IEntityReader
    {
        /// <summary>
        /// This method will determine if the entity can be read, if yes the "GetChildren" method will be called in sequence.
        /// </summary>
        /// <param name="factory">Factory instance that can help in reading</param>
        /// <param name="entity">Real entity</param>
        /// <returns>Return TRUE if can read</returns>
        bool CanRead(ComplexExpressionFactory factory, object entity);

        /// <summary>
        /// This method will be called after the "CanRead" method returns TRUE. It should be responsible for returning a list of ComplexItem. Each ComplexItem represents an element in the expression.
        /// </summary>
        /// <param name="factory">Factory instance that can help in reading</param>
        /// <param name="expression">Expression instance to be used in ComplexItem constructor</param>
        /// <param name="entity">Real entity</param>
        /// <returns>Returns the children of the current entity</returns>
        IEnumerable<ComplexEntity> GetChildren(ComplexExpressionFactory factory, Expression<object> expression, object entity);
    }
}