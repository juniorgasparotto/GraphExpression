using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Extensions of IEnumerable<EntityItem<T>> to get descendants
    /// </summary>
    public static class DescendantsExtensions
    {
        #region Descendants

        /// <summary>
        /// Returns descendants of this occurrence of all references simultaneously
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find descendants</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the child of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the child and grandchild of the current occurrence.</param>
        /// <returns>Returns a list of descendants</returns>
        public static IEnumerable<EntityItem<T>> Descendants<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Descendants(filter, stop, depthStart, depthEnd))
                    yield return item;
        }

        /// <summary>
        /// Returns descendants of this occurrence of all references simultaneously
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find descendants</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the child of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the child and grandchild of the current occurrence.</param>
        /// <returns>Returns a list of descendants</returns>
        public static IEnumerable<EntityItem<T>> Descendants<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Descendants(filter, stop, depthStart, depthEnd))
                    yield return item;
        }

        /// <summary>
        /// Returns descendants of this occurrence of all references simultaneously
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find descendants</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the child and grandchild of the current occurrence.</param>
        /// <returns>Returns a list of descendants</returns>
        public static IEnumerable<EntityItem<T>> Descendants<T>(this IEnumerable<EntityItem<T>> references, int depthEnd)
        {
            foreach (var reference in references)
                foreach (var item in reference.Descendants(depthEnd))
                    yield return item;
        }

        /// <summary>
        /// Returns descendants of this occurrence of all references simultaneously
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find descendants</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the child of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the child and grandchild of the current occurrence.</param>
        /// <returns>Returns a list of descendants</returns>
        public static IEnumerable<EntityItem<T>> Descendants<T>(this IEnumerable<EntityItem<T>> references, int depthStart, int depthEnd)
        {
            foreach (var reference in references)
                foreach (var item in reference.Descendants(depthStart, depthEnd))
                    yield return item;
        }

        #endregion

        #region DescendantsUntil

        /// <summary>
        /// Returns descendants of this occurrence of all references simultaneously until stop
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find descendants</param>
        /// <param name="stop">Action to stop the search when it returns TRUE</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <returns>Returns a list of descendants</returns>
        public static IEnumerable<EntityItem<T>> DescendantsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.DescendantsUntil(stop, filter))
                    yield return item;
        }

        /// <summary>
        /// Returns descendants of this occurrence of all references simultaneously until stop
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find descendants</param>
        /// <param name="stop">Action to stop the search when it returns TRUE</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <returns>Returns a list of descendants</returns>
        public static IEnumerable<EntityItem<T>> DescendantsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.DescendantsUntil(stop, filter))
                    yield return item;
        }

        #endregion

        /// <summary>
        /// Returns children of this occurrence of all references simultaneously
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find children</param>
        /// <returns>Returns a list of children</returns>
        public static IEnumerable<EntityItem<T>> Children<T>(this IEnumerable<EntityItem<T>> references)
        {
            foreach (var reference in references)
                foreach (var item in reference.Children())
                    yield return item;
        }
    }
}