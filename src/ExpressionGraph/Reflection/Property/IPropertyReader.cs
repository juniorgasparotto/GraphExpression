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
        bool CanRead(ReflectInstance value, Type type, PropertyInfo property);
        IEnumerable<MethodValue> GetValues(ReflectInstance value, Type type, PropertyInfo property);
    }
}