using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Extensions of IEnumerable<EntityItem<T>> to get ancestors
    /// </summary>
    public static class AncestorsExtensions
    {
        #region Ancestors

        /// <summary>
        /// Returns the ancestors of all references simultaneously.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find ancestors</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the parent of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the parent and grandfather of the current occurrence.</param>
        /// <returns>Returns a list of ancestors</returns>
        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(filter, stop, depthStart, depthEnd))
                    yield return item;
        }

        /// <summary>
        /// Returns the ancestors of all references simultaneously.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find ancestors</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the parent of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the parent and grandfather of the current occurrence.</param>
        /// <returns>Returns a list of ancestors</returns>
        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(filter, stop, depthStart, depthEnd))
                    yield return item;
        }

        /// <summary>
        /// Returns the ancestors of all references simultaneously.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find ancestors</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the parent and grandfather of the current occurrence.</param>
        /// <returns>Returns a list of ancestors</returns>
        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, int depthEnd)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(depthEnd))
                    yield return item;
        }

        /// <summary>
        /// Returns the ancestors of all references simultaneously.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find ancestors</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the parent of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the parent and grandfather of the current occurrence.</param>
        /// <returns>Returns a list of ancestors</returns>
        public static IEnumerable<EntityItem<T>> Ancestors<T>(this IEnumerable<EntityItem<T>> references, int depthStart, int depthEnd)
        {
            foreach (var reference in references)
                foreach (var item in reference.Ancestors(depthStart, depthEnd))
                    yield return item;
        }

        #endregion

        #region AncestorsUntil

        /// <summary>
        /// Returns the ancestors of all references simultaneously until stop.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find ancestors</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <returns>Returns a list of ancestors</returns>
        public static IEnumerable<EntityItem<T>> AncestorsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.AncestorsUntil(stop, filter))
                    yield return item;
        }

        /// <summary>
        /// Returns the ancestors of all references simultaneously until stop.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find ancestors</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <returns>Returns a list of ancestors</returns>
        public static IEnumerable<EntityItem<T>> AncestorsUntil<T>(this IEnumerable<EntityItem<T>> references, EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            foreach (var reference in references)
                foreach (var item in reference.AncestorsUntil(stop, filter))
                    yield return item;
        }

        #endregion

        /// <summary>
        /// Get all parents of all references simultaneously
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="references">References to find ancestors</param>
        /// <returns>Returns a list of ancestors</returns>
        public static IEnumerable<EntityItem<T>> Parents<T>(this IEnumerable<EntityItem<T>> references)
        {
            foreach (var reference in references)
                yield return reference.Parent;
        }
    }
}