using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class FindExtensions
    {
        public static IEnumerable<EntityItem<T>> Find<T>(this Expression<T> expression, T entity)
        {
            return expression.Where(f => f.Entity != null && f.Entity.Equals(entity));
        }
        
        public static IEnumerable<EntityItem<T>> Find<T>(this Expression<T> expression, EntityItem<T> item)
        {
            return expression.Where(f => f == item);
        }

        public static IEnumerable<EntityItem<T>> Find<T>(this Expression<T> expression, Func<EntityItem<T>, bool> filter)
        {
            return expression.Where(filter);
        }
    }
}