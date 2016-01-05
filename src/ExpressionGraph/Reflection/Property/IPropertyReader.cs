using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public interface IPropertyReader
    {
        bool CanRead(ReflectedInstance value, Type type, PropertyInfo property);
        IEnumerable<MethodValue> GetValues(ReflectedInstance value, Type type, PropertyInfo property);
    }
}