using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Extensions of IEnumerable<Graph<T>> to works with paths
    /// </summary>
    public static class PathExtension
    {
        /// <summary>
        /// Return all paths from all graphs
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="source">List of graphs that contains the paths to be returned</param>
        /// <returns>Return all paths from all graphs</returns>
        public static IEnumerable<Path<T>> ToPaths<T>(this IEnumerable<Graph<T>> source)
        {
            return source.SelectMany(f => f.Paths);
        }

        /// <summary>
        /// Removes all repeated paths from a list of paths.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="source">List of paths</param>
        /// <returns>Return a collection without repeated paths</returns>
        public static IEnumerable<Path<T>> RemoveCoexistents<T>(this IEnumerable<Path<T>> source)
        {
            var listRet = source.ToList();
            foreach (var path in source)
                listRet.RemoveAll(g => g != path && path.ContainsPath(g));
            return listRet;
        }
    }
}
