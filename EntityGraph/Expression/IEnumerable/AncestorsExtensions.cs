using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public static class AncestorsExtensions
    {
        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this ExpressionItem<T> reference, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            if (depthStart <= 0)
                throw new ArgumentException("The 'depthStart' parameter can not be lower than 1.");

            if (depthEnd <= 0)
                throw new ArgumentException("The 'depthEnd' parameter can not be lower than 1.");

            if (depthStart > depthEnd)
                throw new ArgumentException("The 'depthStart' parameter can not be greater than the 'depthEnd' parameter.");

            var ancestor = reference.Parent;

            while (ancestor != null)
            {
                var depth = reference.Level - ancestor.Level;

                if (!depthStart.HasValue || !depthEnd.HasValue || (depth >= depthStart && depth <= depthEnd))
                {
                    var filterResult = (filter == null || filter(ancestor, depth));
                    var stopResult = (stop != null && stop(ancestor, depth));

                    if (filterResult)
                        yield return ancestor;

                    if (stopResult)
                        break;
                }

                ancestor = ancestor.Parent;
            }
        }

        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in Ancestors(reference, filter, stop, depthStart, depthEnd))
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