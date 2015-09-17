using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public static class FindExtensions
    {
        public static IEnumerable<ExpressionItem<T>> Find<T>(this ExpressionBuilder<T> expression, T entity)
        {
            return expression.Where(f => f.Entity != null && f.Entity.Equals(entity));
        }

        public static IEnumerable<ExpressionItem<T>> Find<T>(this ExpressionBuilder<T> expression, Func<ExpressionItem<T>, bool> filter)
        {
            return expression.Where(filter);
        }
    }
}