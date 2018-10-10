using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Extensions of IEnumerable<EntityItem<T>> to get all real entities
    /// </summary>
    public static class ConvertersExtensions
    {
        /// <summary>
        /// Get all real entities
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="items">All items to retriave the real entities</param>
        /// <param name="distinct">If TRUE the entities not repeated</param>
        /// <returns>Return only the real entities</returns>
        public static IEnumerable<T> ToEntities<T>(this IEnumerable<EntityItem<T>> items, bool distinct = true)
        {
            var query = items.Select(f => f.Entity);
            if (distinct)
                return query.Distinct();

            return query;
        }
    }
}