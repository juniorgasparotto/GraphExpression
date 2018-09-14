using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class ConvertersExtensions
    {
        public static IEnumerable<T> ToEntities<T>(this IEnumerable<EntityItem<T>> itemsToConvert, bool distinct = true)
        {
            var query = itemsToConvert.Select(f => f.Entity);
            if (distinct)
                return query.Distinct();

            return query;
        }
    }
}