using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphQuery
{
    public static class NextsOrPreviousOrSiblingsExtensions
    {
        #region Nexts Methods

        public static IEnumerable<ExpressionItem<T>> Nexts<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            if (positionStart <= 0)
                throw new ArgumentException("The 'positionStart' parameter can not be lower than 1.");

            if (positionEnd <= 0)
                throw new ArgumentException("The 'positionEnd' parameter can not be lower than 1.");

            if (positionStart > positionEnd)
                throw new ArgumentException("The 'positionStart' parameter can not be greater than the 'depthEnd' parameter.");

            foreach (var reference in references)
                foreach (var item in reference.Nexts(filter, stop, positionStart, positionEnd))
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

        public static IEnumerable<ExpressionItem<T>> Previous<T>(this IEnumerable<ExpressionItem<T>> references, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            if (positionStart <= 0)
                throw new ArgumentException("The 'positionStart' parameter can not be lower than 1.");

            if (positionEnd <= 0)
                throw new ArgumentException("The 'positionEnd' parameter can not be lower than 1.");

            if (positionStart > positionEnd)
                throw new ArgumentException("The 'positionStart' parameter can not be greater than the 'depthEnd' parameter.");

            foreach (var reference in references)
                foreach (var item in reference.Previous(filter, stop, positionStart, positionEnd))
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
                foreach (var item in reference.Previous(filter, stop, positionStart, positionEnd))
                    yield return item;

                foreach (var item in reference.Nexts(filter, stop, positionStart, positionEnd))
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
    }
}