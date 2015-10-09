using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    public static class GraphExtension
    {
        public static IEnumerable<Graph<T>> ToGraphs<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenCallback, GraphConfiguration<T> configuration = null)
        {
            return Graph<T>.ToGraphs(source, childrenCallback, configuration);
        }

        public static IEnumerable<Graph<T>> RemoveCoexistents<T>(this IEnumerable<Graph<T>> source)
        {
            var listRet = source.ToList();
            foreach (var graph in source)
                listRet.RemoveAll(g => g != graph && graph.ContainsGraph(g));
            return (IEnumerable<Graph<T>>)listRet;
        }

        public static IEnumerable<Vertex<T>> Descendants<T>(this IEnumerable<Graph<T>> source, T entity)
        {
            return null;
        }
    }
}
