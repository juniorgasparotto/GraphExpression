using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class SiblingsExtensions
    {
        #region Siblings

        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, EntityItem<T>.SiblingDirection direction, EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            return Siblings(references, direction, EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), positionStart, positionEnd);
        }

        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, EntityItem<T>.SiblingDirection direction, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            if (positionStart <= 0)
                throw new ArgumentException("The 'positionStart' parameter can not be lower than 1.");

            if (positionEnd <= 0)
                throw new ArgumentException("The 'positionEnd' parameter can not be lower than 1.");

            if (positionStart > positionEnd)
                throw new ArgumentException("The 'positionStart' parameter can not be greater than the 'depthEnd' parameter.");

            foreach (var reference in references)
                foreach (var item in reference.Siblings(direction, filter, stop, positionStart, positionEnd))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, EntityItem<T>.SiblingDirection direction, int positionEnd)
        {
            return Siblings(references, direction, 1, positionEnd);
        }

        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, EntityItem<T>.SiblingDirection direction, int positionStart, int positionEnd)
        {
            return Siblings(references, direction, null, (EntityItemFilterDelegate2<T>)null, positionStart, positionEnd);
        }

        #endregion

        #region SiblingsUntil

        public static IEnumerable<EntityItem<T>> SiblingsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItem<T>.SiblingDirection direction, EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            return SiblingsUntil(references, direction, EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        public static IEnumerable<EntityItem<T>> SiblingsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItem<T>.SiblingDirection direction, EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Siblings(references, direction, filter, stop);
        }

        #endregion
    }
}