using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public static class AncestorsExtensions
    {
        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? depthStart = null, int? depthEnd = null)
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
            return Ancestors(references, null, null, depthStart, depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> AncestorsUntil<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> stop, Func<ExpressionItem<T>, int, bool> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Ancestors(references, filter, stop);
        }

        public static IEnumerable<ExpressionItem<T>> Parents<T>(this IEnumerable<ExpressionItem<T>> references)
        {
            return Ancestors(references, 1);
        }
    }
}