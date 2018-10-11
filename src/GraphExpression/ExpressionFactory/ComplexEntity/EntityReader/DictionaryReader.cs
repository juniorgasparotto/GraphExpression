using System.Collections;
using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    ///  Default class to read a dictionary object
    /// </summary>
    public class DictionaryReader : IEntityReader
    {
        /// <summary>
        /// This method will determine if the entity can be read, if yes the "GetChildren" method will be called in sequence.
        /// </summary>
        /// <param name="factory">Factory instance that can help in reading</param>
        /// <param name="entity">Real entity</param>
        /// <returns>Return TRUE if can read</returns>
        public bool CanRead(ComplexExpressionFactory factory, object entity)
        {
            return entity is IDictionary || entity is DictionaryEntry;
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
            if (entity is IDictionary dic)
            {
                var count = 0;
                foreach (DictionaryEntry entry in dic)
                    yield return new CollectionItemEntity(expression, count++, entry);

                // read members, it may happen to be an instance of the 
                // user that inherits from IDictionary, so you need to read the members.
                foreach (var memberReader in factory.MemberReaders)
                {
                    var items = memberReader.GetMembers(factory, expression, entity);
                    foreach (var item in items)
                    {
                        // Ignore property "Values|Keys" because the values already specify 
                        if (item is PropertyEntity property
                            && (property.Property.Name == "Values" || property.Property.Name == "Keys"))
                            continue;

                        yield return item;
                    }
                }
            }
            else if (entity is DictionaryEntry entry)
            {
                // Read properties: "Key" and "Value"
                foreach (var memberReader in factory.MemberReaders)
                {
                    var items = memberReader.GetMembers(factory, expression, entity);
                    foreach (var item in items)
                        yield return item;
                }
            }
        }
    }
}