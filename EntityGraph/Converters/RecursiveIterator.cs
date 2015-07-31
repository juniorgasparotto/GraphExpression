using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public delegate IEnumerable<T> RecursiveIterator<T>(T current, IEnumerable<T> nexts, object data = null);
}
