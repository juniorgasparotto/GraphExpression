using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public static class RecursiveExtensions
    {
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> startLevel, Func<T, IEnumerable<T>> nextLevels = null, RecursiveIterator<T> selectMethod = null, object data = null)
        {
            if (selectMethod == null)
                selectMethod = new RecursiveIterator<T>(SelectAll);

            return Traverse(startLevel, nextLevels, selectMethod, data, new List<object>());
        }

        private static IEnumerable<T> Traverse<T>(IEnumerable<T> startLevel, Func<T, IEnumerable<T>> nextLevels, RecursiveIterator<T> selectMethod, object data, List<object> iterateds)
        {
            foreach (T current in startLevel)
            {
                if (!iterateds.Contains(current))
                {
                    iterateds.Add(current);

                    IEnumerable<T> nexts;
                    if (nextLevels != null)
                        nexts = nextLevels(current);
                    else
                        nexts = (IEnumerable<T>)current;
                    
                    // Main return
                    foreach (var ret in selectMethod(current, nexts, data))
                        yield return ret;

                    // Return all descendants if exists
                    if (nexts != null)
                        foreach (T descendant in Traverse(nexts, nextLevels, selectMethod, data, iterateds))
                            yield return descendant;
                }
            }
        }

        #region Iterators

        public static IEnumerable<T> SelectAll<T>(T current, IEnumerable<T> nexts, object data = null)
        {
            yield return current;
        }

        public static IEnumerable<T> SelectAllWithChildren<T>(T current, IEnumerable<T> nexts, object data = null)
        {
            if (nexts.Count() > 0)
                yield return current;
        }

        public static IEnumerable<T> SelectAllWithoutChildren<T>(T current, IEnumerable<T> nexts, object data = null)
        {
            if (nexts.Count() == 0)
                yield return current;
        }

        public static IEnumerable<HierarchicalEntity> SelectAllOrphans(HierarchicalEntity current, IEnumerable<HierarchicalEntity> nexts, object data = null)
        {
            if (current.Parents == null || current.Parents.Count == 0)
                yield return current;
        }

        #endregion
    }
}
