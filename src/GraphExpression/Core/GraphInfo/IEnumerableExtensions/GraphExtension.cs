using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Extensions of IEnumerable<Graph<T>> to works with graphs
    /// </summary>
    public static class GraphInfoExtension
    {
        /// <summary>
        /// Removes the graphs that are contained within other graphs, keeping only the graphs that are not contained in any other graphs.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="source">List of graphs to clean</param>
        /// <returns>Return a collection of graphs keeping only the graphs that are not contained in any other graphs.</returns>
        public static IEnumerable<Graph<T>> RemoveCoexistents<T>(this IEnumerable<Graph<T>> source)
        {
            var listRet = source.ToList();
            foreach (var graph in source)
                listRet.RemoveAll(g => g != graph && graph.ContainsGraph(g));
            return listRet;
        }
    }
}
