using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public static class IEnumerableExtension
    {
        public static ListOfHierarchicalEntity ToListOfHierarchicalEntity(this IEnumerable<HierarchicalEntity> source)
        {
            var list = new ListOfHierarchicalEntity();
            foreach (var item in source)
                list.Add(item);

            return list;
        }
    }
}
