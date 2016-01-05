using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class MethodReaderArrayGetValue : IMethodReader
    {
        public bool CanRead(ReflectedInstance value, Type type, MethodInfo method)
        {
            return value.Object is Array && method.Name == "GetValue";
        }

        public IEnumerable<MethodValue> GetValues(ReflectedInstance value, Type type, MethodInfo method)
        {
            var array = value.Object as Array;
            var keysValues = ReflectionUtils.ArrayToDictionary(array);

            foreach (var keyValue in keysValues)
            {
                var parameter = new MethodValueParam("indices", null, keyValue.Key);
                yield return new MethodValue(keyValue.Value, parameter);
            }
        }
    }
}