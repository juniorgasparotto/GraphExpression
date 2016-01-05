using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public interface IMethodReader
    {
        bool CanRead(ReflectedInstance value, Type type, MethodInfo methodInfo);
        IEnumerable<MethodValue> GetValues(ReflectedInstance value, Type type, MethodInfo methodInfo);
    }
}