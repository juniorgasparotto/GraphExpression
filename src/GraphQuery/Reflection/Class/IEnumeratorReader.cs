using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphQuery.Reflection
{
    public interface IEnumerableReader
    {
        bool CanRead(object value, Type type);
        IEnumerable<MethodValue> GetValues(object value, Type type);
    }
}