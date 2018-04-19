using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph
{
    public static class NextsOrPreviousOrSiblingsExtensions
    {
        #region Nexts

        public static IEnumerable<ExpressionItem<T>> Nexts<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> filter, ExpressionFilterDelegate<T> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            return Nexts(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), positionStart, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Nexts<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> filter = null, ExpressionFilterDelegate2<T> stop = null, int? positionStart = null, int? positionEnd = null)
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
            return Nexts(references, 1, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Nexts<T>(this IEnumerable<ExpressionItem<T>> references, int positionStart, int positionEnd)
        {
            return Nexts(references, (ExpressionFilterDelegate2<T>)null, (ExpressionFilterDelegate2<T>)null, positionStart, positionEnd);
        }

        #endregion

        #region NextsUntil

        public static IEnumerable<ExpressionItem<T>> NextsUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> stop, ExpressionFilterDelegate<T> filter = null)
        {
            return NextsUntil(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        public static IEnumerable<ExpressionItem<T>> NextsUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> stop, ExpressionFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Nexts(references, filter, stop);
        }

        #endregion

        #region Previous

        public static IEnumerable<ExpressionItem<T>> Previous<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> filter, ExpressionFilterDelegate<T> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            return Previous(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), positionStart, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Previous<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> filter = null, ExpressionFilterDelegate2<T> stop = null, int? positionStart = null, int? positionEnd = null)
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
            return Previous(references, 1, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Previous<T>(this IEnumerable<ExpressionItem<T>> references, int positionStart, int positionEnd)
        {
            return Previous(references, (ExpressionFilterDelegate2 <T>)null, (ExpressionFilterDelegate2<T>)null, positionStart, positionEnd);
        }

        #endregion

        #region PreviousUntil

        public static IEnumerable<ExpressionItem<T>> PreviousUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> stop, ExpressionFilterDelegate<T> filter = null)
        {
            return PreviousUntil(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        public static IEnumerable<ExpressionItem<T>> PreviousUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> stop, ExpressionFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Previous(references, filter, stop);
        }

        #endregion

        #region Siblings

        public static IEnumerable<ExpressionItem<T>> Siblings<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> filter, ExpressionFilterDelegate<T> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            return Siblings(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), positionStart, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Siblings<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> filter = null, ExpressionFilterDelegate2<T> stop = null, int? positionStart = null, int? positionEnd = null)
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
            return Siblings(references, 1, positionEnd);
        }

        public static IEnumerable<ExpressionItem<T>> Siblings<T>(this IEnumerable<ExpressionItem<T>> references, int positionStart, int positionEnd)
        {
            return Siblings(references, (ExpressionFilterDelegate2 <T>)null, (ExpressionFilterDelegate2<T>)null, positionStart, positionEnd);
        }

        #endregion

        #region SiblingsUntil

        public static IEnumerable<ExpressionItem<T>> SiblingsUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate<T> stop, ExpressionFilterDelegate<T> filter = null)
        {
            return SiblingsUntil(references, ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), ExpressionFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        public static IEnumerable<ExpressionItem<T>> SiblingsUntil<T>(this IEnumerable<ExpressionItem<T>> references, ExpressionFilterDelegate2<T> stop, ExpressionFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Siblings(references, filter, stop);
        }

        #endregion
    }
}