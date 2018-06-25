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
            return Ancestors(references, EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), depthStart, depthEnd);
        }

        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(filter, stop, depthStart, depthEnd))
                    yield return item;
        }
        
        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, int depthEnd)
        {
            return Ancestors(references, 1, depthEnd);
        }

        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, int depthStart, int depthEnd)
        {
            return Ancestors(references, (EntityItemFilterDelegate2 <T>)null, (EntityItemFilterDelegate2<T>)null, depthStart, depthEnd);
        }

        #endregion

        #region AncestorsUntil

        public static IEnumerable<EntityItem<T>> AncestorsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            return AncestorsUntil(references, EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        public static IEnumerable<EntityItem<T>> AncestorsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Ancestors(references, filter, stop);
        }

        #endregion

        public static IEnumerable<EntityItem<T>> Parents<T>(this IEnumerable<EntityItem<T>> references)
        {
            return Ancestors(references, 1);
        }
    }
}