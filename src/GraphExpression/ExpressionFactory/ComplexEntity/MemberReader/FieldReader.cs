using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    /// Default class to read field member
    /// </summary>
    public class FieldReader : IMemberReader
    {
        /// <summary>
        /// This method will be called to retrieve the members (Properties and Fields) of a given entity
        /// </summary>
        /// <param name="factory">Factory instance that can help in reading</param>
        /// <param name="expression">Expression instance to be used in ComplexItem constructor</param>
        /// <param name="entity">Real entity</param>
        /// <returns>Returns the members of the entity</returns>
        public IEnumerable<ComplexEntity> GetMembers(ComplexExpressionFactory factory, Expression<object> expression, object entity)
        {
            var fields = factory.GetFields(entity);
            foreach (var f in fields)
                yield return new FieldEntity(expression, entity, f);
        }
    }
}