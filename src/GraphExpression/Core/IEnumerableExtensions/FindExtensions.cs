using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Extensions of Expression<T> to find a EntityItem
    /// </summary>
    public static class FindExtensions
    {
        /// <summary>
        /// Find a EntityItem in expression that contain a specify entity
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="expression">Expression to find</param>
        /// <param name="entity">Real entity to search</param>
        /// <returns>Return all EntityItem from the real entity</returns>
        public static IEnumerable<EntityItem<T>> Find<T>(this Expression<T> expression, T entity)
        {
            return expression.Where(f => f.Entity != null && f.Entity.Equals(entity));
        }

        /// <summary>
        /// Find a EntityItem in expression that contain a specify EntityItem
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="expression">Expression to find</param>
        /// <param name="item">EntityItem to search</param>
        /// <returns>Return all EntityItem from the EntityItem</returns>
        public static IEnumerable<EntityItem<T>> Find<T>(this Expression<T> expression, EntityItem<T> item)
        {
            return expression.Where(f => f == item);
        }

        /// <summary>
        /// Find a EntityItem in expression that contain a custom search
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="expression">Expression to find</param>
        /// <param name="filter">Custom filter</param>
        /// <returns>Return all EntityItem from a custom filter</returns>
        public static IEnumerable<EntityItem<T>> Find<T>(this Expression<T> expression, Func<EntityItem<T>, bool> filter)
        {
            return expression.Where(filter);
        }
    }
}