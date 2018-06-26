using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class AncestorsExtensions
    {
        #region Ancestors

        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(filter, stop, depthStart, depthEnd))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(filter, stop, depthStart, depthEnd))
                    yield return item;
        }
        
        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, int depthEnd)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(depthEnd))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, int depthStart, int depthEnd)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(depthStart, depthEnd))
                    yield return item;
        }

        #endregion

        #region AncestorsUntil

        public static IEnumerable<EntityItem<T>> AncestorsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.AncestorsUntil(stop, filter))
                    yield return item;
        }

        public static IEnumerable<EntityItem<T>> AncestorsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.AncestorsUntil(stop, filter))
                    yield return item;
        }

        #endregion

        public static IEnumerable<EntityItem<T>> Parents<T>(this IEnumerable<EntityItem<T>> references)
        {
            foreach (var reference in references)
                yield return reference.Parent;
        }
    }
}