using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    ///  Default class to read a dynamic object
    /// </summary>
    public class DynamicReader : IEntityReader
    {
        /// <summary>
        /// This method will determine if the entity can be read, if yes the "GetChildren" method will be called in sequence.
        /// </summary>
        /// <param name="factory">Factory instance that can help in reading</param>
        /// <param name="entity">Real entity</param>
        /// <returns>Return TRUE if can read</returns>
        public bool CanRead(ComplexExpressionFactory factory, object entity)
        {
            return entity is System.Dynamic.ExpandoObject;
        }

        /// <summary>
        /// This method will be called after the "CanRead" method returns TRUE. It should be responsible for returning a list of ComplexItem. Each ComplexItem represents an element in the expression.
        /// </summary>
        /// <param name="factory">Factory instance that can help in reading</param>
        /// <param name="expression">Expression instance to be used in ComplexItem constructor</param>
        /// <param name="entity">Real entity</param>
        /// <returns>Returns the children of the current entity</returns>
        public IEnumerable<ComplexEntity> GetChildren(ComplexExpressionFactory factory, Expression<object> expression, object entity)
        {
            var dyn = (System.Collections.IEnumerable)entity;
            foreach (KeyValuePair<string, object> item in dyn)
                yield return new DynamicItemEntity(expression, item.Key, item.Value);
        }
    }
}