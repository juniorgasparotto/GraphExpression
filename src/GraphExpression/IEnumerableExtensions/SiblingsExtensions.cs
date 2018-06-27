using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class SiblingsExtensions
    {
        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, SiblingDirection direction = SiblingDirection.Both, int? positionStart = null, int? positionEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Siblings(filter, stop, direction, positionStart, positionEnd))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, SiblingDirection direction = SiblingDirection.Both, int? positionStart = null, int? positionEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Siblings(filter, stop, direction, positionStart, positionEnd))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, int positionStart, int positionEnd, SiblingDirection direction = SiblingDirection.Both)
        {
            foreach (var reference in references)
                foreach (var item in reference.Siblings(positionStart, positionEnd, direction))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, int positionEnd, SiblingDirection direction = SiblingDirection.Both)
        {
            foreach (var reference in references)
                foreach (var item in reference.Siblings(positionEnd, direction))
                    yield return item;
        }

        #region SiblingsUntil

        public static IEnumerable<EntityItem<T>> SiblingsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null, SiblingDirection direction = SiblingDirection.Both)
        {
            foreach (var reference in references)
                foreach (var item in reference.SiblingsUntil(stop, filter, direction))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> SiblingsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null, SiblingDirection direction = SiblingDirection.Both)
        {
            foreach (var reference in references)
                foreach (var item in reference.SiblingsUntil(stop, filter, direction))
                    yield return item;
        }

        #endregion

    }
}