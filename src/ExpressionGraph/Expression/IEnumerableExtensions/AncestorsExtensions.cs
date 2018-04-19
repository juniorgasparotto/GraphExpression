using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph
{
    public static class AncestorsExtensions
    {
        #region Ancestors

        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> filter, ExpressionFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            return Ancestors(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), depthStart, depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> filter = null, ExpressionFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(filter, stop, depthStart, depthEnd))
                    yield return item;
        }
        
        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, int depthEnd)
        {
            return Ancestors(references, 1, depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, int depthStart, int depthEnd)
        {
            return Ancestors(references, (ExpressionFilterDelegate2 <T>)null, (ExpressionFilterDelegate2<T>)null, depthStart, depthEnd);
        }

        #endregion

        #region AncestorsUntil

        public static IEnumerable<ExpressionItem<T>> AncestorsUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> stop, ExpressionFilterDelegate<T> filter = null)
        {
            return AncestorsUntil(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        public static IEnumerable<ExpressionItem<T>> AncestorsUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> stop, ExpressionFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Ancestors(references, filter, stop);
        }

        #endregion

        public static IEnumerable<ExpressionItem<T>> Parents<T>(this IEnumerable<ExpressionItem<T>> references)
        {
            return Ancestors(references, 1);
        }
    }
}