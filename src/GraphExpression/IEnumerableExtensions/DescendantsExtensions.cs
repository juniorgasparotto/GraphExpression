using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class DescendantsExtensions
    {
        #region Descendants

        public static IEnumerable<EntityItem<T>> Descendants<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Descendants(filter, stop, depthStart, depthEnd))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Descendants<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Descendants(filter, stop, depthStart, depthEnd))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Descendants<T>(this IEnumerable<EntityItem<T>> references, int depthEnd)
        {
            foreach (var reference in references)
                foreach (var item in reference.Descendants(depthEnd))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Descendants<T>(this IEnumerable<EntityItem<T>> references, int depthStart, int depthEnd)
        {
            foreach (var reference in references)
                foreach (var item in reference.Descendants(depthStart, depthEnd))
                    yield return item;
        }

        #endregion

        #region DescendantsUntil

        public static IEnumerable<EntityItem<T>> DescendantsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.DescendantsUntil(stop, filter))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> DescendantsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.DescendantsUntil(stop, filter))
                    yield return item;
        }

        #endregion

        public static IEnumerable<EntityItem<T>> Children<T>(this IEnumerable<EntityItem<T>> references)
        {
            return Descendants(references, 1);
        }
    }
}