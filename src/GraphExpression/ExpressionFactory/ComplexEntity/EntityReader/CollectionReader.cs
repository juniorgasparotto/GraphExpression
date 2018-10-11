using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    /// Default class to read a collection object
    /// </summary>
    public class CollectionReader : IEntityReader
    {
        /// <summary>
        /// This method will determine if the entity can be read, if yes the "GetChildren" method will be called in sequence.
        /// </summary>
        /// <param name="factory">Factory instance that can help in reading</param>
        /// <param name="entity">Real entity</param>
        /// <returns>Return TRUE if can read</returns>
        public bool CanRead(ComplexExpressionFactory factory, object entity)
        {
            return entity is System.Collections.ICollection;
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
            var list = (System.Collections.ICollection)entity;
            var enumerator = list.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
                yield return new CollectionItemEntity(expression, count++, enumerator.Current);

            // read members, it may happen to be an instance of the 
            // user that inherits from IList, so you need to read the members.
            foreach (var memberReader in factory.MemberReaders)
            {
                var items = memberReader.GetMembers(factory, expression, entity);
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}