using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    /// <summary>
    /// Without parameters
    /// </summary>
    public class MethodReaderDefault : IMethodReader
    {
        public bool CanRead(object obj, MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Length == 0;
        }

        public IEnumerable<MethodValue> GetValues(object obj, MethodInfo methodInfo)
        {
            var value = methodInfo.Invoke(obj, null);
            yield return new MethodValue(value, null);
        }
    }
}