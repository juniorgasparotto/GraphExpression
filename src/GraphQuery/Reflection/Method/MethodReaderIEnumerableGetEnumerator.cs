using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphQuery.Reflection
{
    public class MethodReaderIEnumerableGetEnumerator : IMethodReader
    {
        public bool CanRead(ReflectedInstance value, Type type, MethodInfo method)
        {
            return value.Object is System.Collections.IEnumerable
                && method.Name == "GetEnumerator" || method.Name == "System.Collections.IEnumerable.GetEnumerator";
        }

        public IEnumerable<MethodValue> GetValues(ReflectedInstance value, Type type, MethodInfo method)
        {
            var enumarable = value.Object as System.Collections.IEnumerable;
            var i = 0;
            foreach (var item in enumarable)
            {
                var parameter = new MethodValueParam("indices", null, i++);
                yield return new MethodValue(item, parameter);
            }
        }
    }
}