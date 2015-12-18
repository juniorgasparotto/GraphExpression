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
        public bool CanRead(UnitReflaction obj, Type type, PropertyInfo property)
        {
            return property.GetIndexParameters().Length == 0;
        }

        public IEnumerable<MethodValue> GetValues(UnitReflaction obj, Type type, PropertyInfo property)
        {
            var value = property.GetValue(obj.Object, null);
            yield return new MethodValue(value, null);
        }
    }
}