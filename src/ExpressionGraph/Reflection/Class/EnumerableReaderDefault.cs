using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class EnumerableReaderDefault : IEnumerableReader
    {
        public bool CanRead(object value, Type type)
        {
            return value is System.Collections.IEnumerable;
        }

        public IEnumerable<MethodValue> GetValues(object value, Type type)
        {
            var enumarable = value as System.Collections.IEnumerable;
            var i = 0;
            foreach (var item in enumarable)
            {
                var parameter = new MethodValueParam("indices", null, i++);
                yield return new MethodValue(item, parameter);
            }
        }
    }
}