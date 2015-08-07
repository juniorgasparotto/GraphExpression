using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public static class GraphExtension
    {
        #region Graph extensions

        public static IEnumerable<Graph<T>> ToGraphs<T>(this IEnumerable<T> source, Func<T, List<T>> nextVertexCallback, Action<Edge<T>> edgeCreatedCallback = null, bool encloseRootTokenInParenthesis = false)
        {
            return Graph<T>.ToGraphs(source, nextVertexCallback, edgeCreatedCallback, encloseRootTokenInParenthesis);
        }

        public static IEnumerable<Graph<T>> RemoveCoexistents<T>(this IEnumerable<Graph<T>> source)
        {
            var listRet = source.ToList();
            foreach (var graph in source)
                listRet.RemoveAll(g => g != graph && graph.ContainsGraph(g));
            return (IEnumerable<Graph<T>>)listRet;
        }

        #endregion

        #region Paths extensions

        public static IEnumerable<Path<T>> ToPaths<T>(this IEnumerable<Graph<T>> source)
        {
            return source.SelectMany(f => f.Paths);
        }

        public static IEnumerable<Path<T>> RemoveCoexistents<T>(this IEnumerable<Path<T>> source)
        {
            var listRet = source.ToList();
            foreach (var path in source)
                listRet.RemoveAll(g => g != path && path.ContainsPath(g));
            return (IEnumerable<Path<T>>)listRet;
        }

        #endregion
    }
}
