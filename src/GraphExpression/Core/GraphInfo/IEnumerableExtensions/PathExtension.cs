using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public static class PathExtension
    {
        public static IEnumerable<Path<T>> ToPaths<T>(this IEnumerable<GraphInfo<T>> source)
        {
            return source.SelectMany(f => f.Paths);
        }

        public static IEnumerable<Path<T>> RemoveCoexistents<T>(this IEnumerable<Path<T>> source)
        {
            var listRet = source.ToList();
            foreach (var path in source)
                listRet.RemoveAll(g => g != path && path.ContainsPath(g));
            return listRet;
        }
    }
}
