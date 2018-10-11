using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Default class to read property member
    /// </summary>
    public class PropertyReader : IMemberReader
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
            // get all propertis: 
            // 1) ignore indexed (this[...]) with GetIndexParameters > 0
            // 2) ignore properties with only setters
            var properties = factory.GetProperties(entity);
            foreach (var p in properties)
                if (!p.GetIndexParameters().Any() && p.GetGetMethod() != null)
                    yield return new PropertyEntity(expression, entity, p);
        }
    }
}