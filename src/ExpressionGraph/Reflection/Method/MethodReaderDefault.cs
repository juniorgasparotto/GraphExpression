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
        public bool CanRead(UnitReflaction obj, Type type, MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Length == 0;
        }

        public IEnumerable<MethodValue> GetValues(UnitReflaction obj, Type type, MethodInfo methodInfo)
        {
            var value = methodInfo.Invoke(obj.Object, null);
            yield return new MethodValue(value, null);
        }
    }
}