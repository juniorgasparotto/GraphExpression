using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph
{
    public static class DescendantsExtensions
    {
        #region Descendants

        public static IEnumerable<ExpressionItem<T>> Descendants<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> filter, ExpressionFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            return Descendants(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), depthStart, depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Descendants<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> filter = null, ExpressionFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
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
            return Descendants(references, 1, depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Descendants<T>(this IEnumerable<ExpressionItem<T>> references, int depthStart, int depthEnd)
        {
            return Descendants(references, (ExpressionFilterDelegate2<T>)null, (ExpressionFilterDelegate2<T>)null, depthStart, depthEnd);
        }

        #endregion

        #region DescendantsUntil

        public static IEnumerable<ExpressionItem<T>> DescendantsUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> stop, ExpressionFilterDelegate<T> filter = null)
        {
            return DescendantsUntil(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        public static IEnumerable<ExpressionItem<T>> DescendantsUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> stop, ExpressionFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Descendants(references, filter, stop);
        }

        #endregion

        public static IEnumerable<ExpressionItem<T>> Children<T>(this IEnumerable<ExpressionItem<T>> references)
        {
            return Descendants(references, 1);
        }
    }
}