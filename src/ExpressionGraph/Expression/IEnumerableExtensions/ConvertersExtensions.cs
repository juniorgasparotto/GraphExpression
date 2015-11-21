using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph
{
    public static class ConvertersExtensions
    {
        public static IEnumerable<T> ToEntities<T>(this IEnumerable<ExpressionItem<T>> itemsToConvert, bool distinct = true)
        {
            var query = itemsToConvert.Where(f => f.GetType() == typeof(ExpressionItem<T>)).Select(f => f.Entity);
            if (distinct)
                return query.Distinct();

            return query;
        }
    }
}