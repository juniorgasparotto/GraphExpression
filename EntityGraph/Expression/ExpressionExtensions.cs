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

        public static IEnumerable<ExpressionItem<T>> Siblings<T>(this IEnumerable<ExpressionItem<T>> references)
        {
            return null;
        }

        #region Ancestors Methods

        private static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null)
        {
            foreach (var reference in references)
            {
                var ancestor = reference.Parent;

                while (ancestor != null)
                {
                    var depth = reference.Level - ancestor.Level;
                    var filterResult = (filter == null || filter(ancestor, depth));
                    var stopResult = (stop != null && stop(ancestor, depth));

                    if (filterResult)
                        yield return ancestor;

                    if (stopResult)
                        break;

                    ancestor = ancestor.Parent;
                }
            }
        }

        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, int depth)
        {
            return Ancestors(references, 1, depth);
        }

        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, int depthStart, int depthEnd)
        {
            if (depthStart <= 0)
                throw new ArgumentException("The 'depthStart' parameter can not be lower than 1.");

            if (depthEnd <= 0)
                throw new ArgumentException("The 'depthEnd' parameter can not be lower than 1.");

            if (depthStart > depthEnd)
                throw new ArgumentException("The 'depthStart' parameter can not be greater than the 'depthEnd' parameter.");

            return Ancestors(references, (ancestor, depth) => depth >= depthStart && depth <= depthEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Ancestors<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null)
        {
            return Ancestors(references, filter, null);
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

        public static IEnumerable<ExpressionItem<T>> Descendants<T>(this IEnumerable<ExpressionItem<T>> references, int depth = 0)
        {
            var referencesFiltered = (from reference in references
                            where reference.Previous == null || reference.Previous.GetType() == typeof(ExpressionItemOpenParenthesis<T>)
                            group reference by new { reference.Entity } into distinctByEntity
                            select distinctByEntity.FirstOrDefault()).ToList();

            if (referencesFiltered.Count > 0)
            {
                foreach (var reference in referencesFiltered)
                {
                    var next = reference.Next;

                    while(next != null)
                    { 
                        if (next.GetType() == typeof(ExpressionItem<T>) && (depth == 0 || reference.Level >= next.Level - depth))
                            yield return next;
                        else if (next.LevelInExpression == reference.LevelInExpression && next.GetType() == typeof(ExpressionItemCloseParenthesis<T>))
                            break;

                        next = next.Next;
                    }
                }
            }
        }

        public static IEnumerable<ExpressionItem<T>> Children<T>(this IEnumerable<ExpressionItem<T>> references)
        {
            return Descendants(references, 1);
        }

        public static IEnumerable<T> ToEntities<T>(this IEnumerable<ExpressionItem<T>> itemsToConvert, bool distinct = true)
        {
            var query = itemsToConvert.Where(f => f.GetType() == typeof(ExpressionItem<T>)).Select(f => f.Entity);
            if (distinct)
                return query.Distinct();

            return query;
        }
    }
}