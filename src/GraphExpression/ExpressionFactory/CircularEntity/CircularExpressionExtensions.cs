using GraphExpression.Serialization;
using System;
using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    /// Extension to add methods of creating circular expressions on any object. 
    /// </summary>
    public static class CircularExpressionExtensions
    {
        /// <summary>
        /// Creates a circular expression, that is, where all entities of the expression are of the same type. It is very common between Parent and Child relationships.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="obj">Object to convert to a expression</param>
        /// <param name="childrenCallback">Method to return the children of the current object</param>
        /// <param name="deep">Determines whether the expression constructed will be a deep one, that is, if it will navigate objects that have already been navigated, except for cyclic references.</param>
        /// <returns>Return a circular expression</returns>
        public static Expression<T> AsExpression<T>(this T obj, Func<T, IEnumerable<T>> childrenCallback, bool deep = false)
        {
            var expression = new Expression<T>(obj, childrenCallback, deep);
            expression.DefaultSerializer = new CircularEntityExpressionSerializer<T>(expression, (i => i?.ToString()));
            return expression;
        }

        /// <summary>
        /// Creates a circular expression, that is, where all entities of the expression are of the same type. It is very common between Parent and Child relationships.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="obj">Object to convert to a expression</param>
        /// <param name="childrenCallback">Method to return the children of the current object</param>
        /// <param name="entityNameCallback">Delegates the action of retrieving the entity name that will be used in the default serialization and also in the debug.</param>
        /// <param name="deep">Determines whether the expression constructed will be a deep one, that is, if it will navigate objects that have already been navigated, except for cyclic references.</param>
        /// <returns>Return a circular expression</returns>
        public static Expression<T> AsExpression<T>(this T obj, Func<T, IEnumerable<T>> childrenCallback, Func<T, object> entityNameCallback, bool deep = false)
        {
            var expression = new Expression<T>(obj, childrenCallback, deep);
            entityNameCallback = entityNameCallback ?? (i => i?.ToString());
            expression.DefaultSerializer = new CircularEntityExpressionSerializer<T>(expression, entityNameCallback);
            return expression;
        }
    }
}
