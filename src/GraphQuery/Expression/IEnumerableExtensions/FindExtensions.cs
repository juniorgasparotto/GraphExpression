using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphQuery
{
    public static class FindExtensions
    {
        public static IEnumerable<ExpressionItem<T>> Find<T>(this Expression<T> expression, T entity)
        {
            return expression.Where(f => f.Entity != null && f.Entity.Equals(entity));
        }
        
        public static IEnumerable<ExpressionItem<T>> Find<T>(this Expression<T> expression, ExpressionItem<T> item)
        {
            return expression.Where(f => f == item);
        }

        public static IEnumerable<ExpressionItem<T>> Find<T>(this Expression<T> expression, Func<ExpressionItem<T>, bool> filter)
        {
            return expression.Where(filter);
        }
    }
}