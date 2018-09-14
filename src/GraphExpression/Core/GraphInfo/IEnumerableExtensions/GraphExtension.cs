using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class GraphInfoExtension
    {
        public static IEnumerable<Graph<T>> RemoveCoexistents<T>(this IEnumerable<Graph<T>> source)
        {
            var listRet = source.ToList();
            foreach (var graph in source)
                listRet.RemoveAll(g => g != graph && graph.ContainsGraph(g));
            return listRet;
        }
    }
}
