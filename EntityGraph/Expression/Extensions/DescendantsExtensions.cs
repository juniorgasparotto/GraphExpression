using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public static class DescendantsExtensions
    {

        public static IEnumerable<ExpressionItem<T>> Descendants<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? depthStart = null, int? depthEnd = null)
        {   
            // Group by 'Entity' because all occurrences will bring the same result.
            var referencesFiltered = (from reference in references
                                      group reference by new { reference.Entity } into distinctByEntity
                                      select distinctByEntity.FirstOrDefault()).ToList();

            foreach (var reference in references)
                foreach (var item in reference.Descendants(filter, stop, depthStart, depthEnd))
                    yield return item;
        }
        
        public static IEnumerable<ExpressionItem<T>> Descendants<T>(this IEnumerable<ExpressionItem<T>> references, int depthEnd)
        {
            return Descendants(references, null, null, 1, depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Descendants<T>(this IEnumerable<ExpressionItem<T>> references, int depthStart, int depthEnd)
        {
            return Descendants(references, null, null, depthStart, depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> DescendantsUntil<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> stop, Func<ExpressionItem<T>, int, bool> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Descendants(references, filter, stop);
        }

        public static IEnumerable<ExpressionItem<T>> Children<T>(this IEnumerable<ExpressionItem<T>> references)
        {
            return Descendants(references, 1);
        }
    }
}