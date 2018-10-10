using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Extensions of IEnumerable<EntityItem<T>> to get siblings
    /// </summary>
    public static class SiblingsExtensions
    {
        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find siblings</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <param name="positionStart">Determines start position</param>
        /// <param name="positionEnd">Determines end position</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Siblings(filter, stop, direction, positionStart, positionEnd))
                    yield return item;
        }

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find siblings</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <param name="positionStart">Determines start position</param>
        /// <param name="positionEnd">Determines end position</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Siblings(filter, stop, direction, positionStart, positionEnd))
                    yield return item;
        }

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find siblings</param>
        /// <param name="positionStart">Determines start position</param>
        /// <param name="positionEnd">Determines end position</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, int positionStart, int positionEnd, SiblingDirection direction = SiblingDirection.Start)
        {
            foreach (var reference in references)
                foreach (var item in reference.Siblings(positionStart, positionEnd, direction))
                    yield return item;
        }

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find siblings</param>
        /// <param name="positionEnd">Determines end position</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public static IEnumerable<EntityItem<T>> Siblings<T>(this IEnumerable<EntityItem<T>> references, int positionEnd, SiblingDirection direction = SiblingDirection.Start)
        {
            foreach (var reference in references)
                foreach (var item in reference.Siblings(positionEnd, direction))
                    yield return item;
        }

        #region SiblingsUntil

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction until stop
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find siblings</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public static IEnumerable<EntityItem<T>> SiblingsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
        {
            foreach (var reference in references)
                foreach (var item in reference.SiblingsUntil(stop, filter, direction))
                    yield return item;
        }

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction until stop
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find siblings</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public static IEnumerable<EntityItem<T>> SiblingsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
        {
            foreach (var reference in references)
                foreach (var item in reference.SiblingsUntil(stop, filter, direction))
                    yield return item;
        }

        #endregion
    }
}