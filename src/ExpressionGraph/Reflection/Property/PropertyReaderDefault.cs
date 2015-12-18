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
        public bool CanRead(ReflectInstance value, Type type, PropertyInfo property)
        {
            return property.GetIndexParameters().Length == 0;
        }

        public IEnumerable<MethodValue> GetValues(ReflectInstance value, Type type, PropertyInfo property)
        {
            var propValue = property.GetValue(value.Object, null);
            yield return new MethodValue(propValue, null);
        }
    }
}