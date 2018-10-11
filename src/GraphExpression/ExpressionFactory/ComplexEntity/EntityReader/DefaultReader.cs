using GraphExpression.Utils;
using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    ///  Default class to read a commom object
    /// </summary>
    public class DefaultReader : IEntityReader
    {
        /// <summary>
        /// This method will determine if the entity can be read, if yes the "GetChildren" method will be called in sequence.
        /// </summary>
        /// <param name="factory">Factory instance that can help in reading</param>
        /// <param name="entity">Real entity</param>
        /// <returns>Return TRUE if can read</returns>
        public bool CanRead(ComplexExpressionFactory factory, object entity)
        {
            return !ReflectionUtils.IsSystemType(entity.GetType());
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
            // read members
            foreach (var memberReader in factory.MemberReaders)
            {
                var items = memberReader.GetMembers(factory, expression, entity);
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}