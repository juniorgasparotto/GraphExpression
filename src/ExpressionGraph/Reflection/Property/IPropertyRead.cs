using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public interface IPropertyRead
    {
        bool CanRead(object obj, PropertyInfo property);
        IEnumerable<MethodValue> GetValues(object obj, PropertyInfo property);
    }
}