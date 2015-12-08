using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class PropertyReaderDefault : IPropertyReader
    {
        public bool CanRead(object obj, PropertyInfo property)
        {
            return property.GetIndexParameters().Length == 0;
        }

        public IEnumerable<MethodValue> GetValues(object obj, PropertyInfo property)
        {
            var value = property.GetValue(obj, null);
            yield return new MethodValue(value, null);
        }
    }
}