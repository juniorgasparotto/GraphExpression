using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public static class ExpressionExtensions
    {
        public static IEnumerable<ExpressionItem<T>> Find<T>(this Expression<T> expression, T entity)
        {
            return expression.Where(f => f.Entity != null && f.Entity.Equals(entity));
        }

        public static IEnumerable<ExpressionItem<T>> Find<T>(this Expression<T> expression, Func<ExpressionItem<T>, bool> filter)
        {
            return expression.Where(filter);
        }

        #region Descendants Methods

        public static IEnumerable<ExpressionItem<T>> Descendants<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            if (depthStart <= 0)
                throw new ArgumentException("The 'depthStart' parameter can not be lower than 1.");

            if (depthEnd <= 0)
                throw new ArgumentException("The 'depthEnd' parameter can not be lower than 1.");

            if (depthStart > depthEnd)
                throw new ArgumentException("The 'depthStart' parameter can not be greater than the 'depthEnd' parameter.");

            // Group by 'Entity' because all occurrences will bring the same result.
            var referencesFiltered = (from reference in references
                                      group reference by new { reference.Entity } into distinctByEntity
                                      select distinctByEntity.FirstOrDefault()).ToList();

            foreach (var reference in references)
            {
                var descendant = reference.Next2;

                while (descendant != null && reference.Level < descendant.Level)
                {
                    var depth = descendant.Level - reference.Level;

                    if (!depthStart.HasValue || !depthEnd.HasValue || (depth >= depthStart && depth <= depthEnd))
                    {
                        var filterResult = (filter == null || filter(descendant, depth));
                        var stopResult = (stop != null && stop(descendant, depth));

                        if (filterResult)
                            yield return descendant;

                        if (stopResult)
                            break;
                    }

                    descendant = descendant.Next2;
                }
            }
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

        #endregion

        #region Nexts Methods

        public static IEnumerable<ExpressionItem<T>> Nexts<T>(ExpressionItem<T> reference, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            var descendant = reference.Next2;

            var position = 1;
            while (descendant != null && reference.Level <= descendant.Level)
            {
                var depth = descendant.Level - reference.Level;
                if (depth == 0)
                {
                    if (!positionStart.HasValue || !positionEnd.HasValue || (position >= positionStart && position <= positionEnd))
                    {
                        var filterResult = (filter == null || filter(descendant, position));
                        var stopResult = (stop != null && stop(descendant, position));

                        if (filterResult)
                            yield return descendant;

                        if (stopResult)
                            break;
                    }

                    position++;
                }

                descendant = descendant.Next2;
            }
        }

        public static IEnumerable<ExpressionItem<T>> Nexts<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            if (positionStart <= 0)
                throw new ArgumentException("The 'positionStart' parameter can not be lower than 1.");

            if (positionEnd <= 0)
                throw new ArgumentException("The 'positionEnd' parameter can not be lower than 1.");

            if (positionStart > positionEnd)
                throw new ArgumentException("The 'positionStart' parameter can not be greater than the 'depthEnd' parameter.");

            foreach (var reference in references)
                foreach (var item in Nexts(reference, filter, stop, positionStart, positionEnd))
                    yield return item;
        }

        public static IEnumerable<ExpressionItem<T>> Nexts<T>(this IEnumerable<ExpressionItem<T>> references, int positionEnd)
        {
            return Nexts(references, null, null, 1, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Nexts<T>(this IEnumerable<ExpressionItem<T>> references, int positionStart, int positionEnd)
        {
            return Nexts(references, null, null, positionStart, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> NextsUntil<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> stop, Func<ExpressionItem<T>, int, bool> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Nexts(references, filter, stop);
        }
        
        #endregion

        #region Previous Methods

        public static IEnumerable<ExpressionItem<T>> Previous<T>(ExpressionItem<T> reference, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            var previous = reference.Previous2;

            var position = 1;
            while (previous != null && reference.Level <= previous.Level)
            {
                var depth = previous.Level - reference.Level;
                if (depth == 0)
                {
                    if (!positionStart.HasValue || !positionEnd.HasValue || (position >= positionStart && position <= positionEnd))
                    {
                        var filterResult = (filter == null || filter(previous, position));
                        var stopResult = (stop != null && stop(previous, position));

                        if (filterResult)
                            yield return previous;

                        if (stopResult)
                            break;
                    }

                    position++;
                }

                previous = previous.Previous2;
            }
        }

        public static IEnumerable<ExpressionItem<T>> Previous<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            if (positionStart <= 0)
                throw new ArgumentException("The 'positionStart' parameter can not be lower than 1.");

            if (positionEnd <= 0)
                throw new ArgumentException("The 'positionEnd' parameter can not be lower than 1.");

            if (positionStart > positionEnd)
                throw new ArgumentException("The 'positionStart' parameter can not be greater than the 'depthEnd' parameter.");

            foreach (var reference in references)
                foreach (var item in Previous(reference, filter, stop, positionStart, positionEnd))
                    yield return item;
        }

        public static IEnumerable<ExpressionItem<T>> Previous<T>(this IEnumerable<ExpressionItem<T>> references, int positionEnd)
        {
            return Previous(references, null, null, 1, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Previous<T>(this IEnumerable<ExpressionItem<T>> references, int positionStart, int positionEnd)
        {
            return Previous(references, null, null, positionStart, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> PreviousUntil<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> stop, Func<ExpressionItem<T>, int, bool> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Previous(references, filter, stop);
        }

        #endregion

        #region Siblings Methods

        public static IEnumerable<ExpressionItem<T>> Siblings<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            if (positionStart <= 0)
                throw new ArgumentException("The 'positionStart' parameter can not be lower than 1.");

            if (positionEnd <= 0)
                throw new ArgumentException("The 'positionEnd' parameter can not be lower than 1.");

            if (positionStart > positionEnd)
                throw new ArgumentException("The 'positionStart' parameter can not be greater than the 'depthEnd' parameter.");

            foreach (var reference in references)
            {
                foreach (var item in Previous(reference, filter, stop, positionStart, positionEnd))
                    yield return item;

                foreach (var item in Nexts(reference, filter, stop, positionStart, positionEnd))
                    yield return item;
            }
        }
        
        public static IEnumerable<ExpressionItem<T>> Siblings<T>(this IEnumerable<ExpressionItem<T>> references, int positionEnd)
        {
            return Siblings(references, null, null, 1, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Siblings<T>(this IEnumerable<ExpressionItem<T>> references, int positionStart, int positionEnd)
        {
            return Siblings(references, null, null, positionStart, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> SiblingsUntil<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> stop, Func<ExpressionItem<T>, int, bool> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Siblings(references, filter, stop);
        }

        #endregion

        #region Ancestors Methods
        
        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            if (depthStart <= 0)
                throw new ArgumentException("The 'depthStart' parameter can not be lower than 1.");

            if (depthEnd <= 0)
                throw new ArgumentException("The 'depthEnd' parameter can not be lower than 1.");

            if (depthStart > depthEnd)
                throw new ArgumentException("The 'depthStart' parameter can not be greater than the 'depthEnd' parameter.");

            foreach (var reference in references)
            {
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
        }
        
        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, int depthStart, int depthEnd)
        {
            return Ancestors(references, (ancestor, depth) => depth >= depthStart && depth <= depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, int depthEnd)
        {
            return Ancestors(references, 1, depthEnd);
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

        #endregion

        
        public static IEnumerable<T> ToEntities<T>(this IEnumerable<ExpressionItem<T>> itemsToConvert, bool distinct = true)
        {
            var query = itemsToConvert.Where(f => f.GetType() == typeof(ExpressionItem<T>)).Select(f => f.Entity);
            if (distinct)
                return query.Distinct();

            return query;
        }
    }
}