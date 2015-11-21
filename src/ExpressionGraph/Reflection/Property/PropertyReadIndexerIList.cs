using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class PropertyReadIndexerIList : IPropertyRead
    {
        public bool CanRead(object obj, PropertyInfo property)
        {

            // verify if property is "this[int i]", in real use is: "myList[0]'
            if (obj is System.Collections.IList)
            { 
                var parameters = property.GetIndexParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
                    return true;
            }

            return false;
        }

        public IEnumerable<MethodValue> GetValues(object obj, PropertyInfo property)
        {
            var converted = obj as System.Collections.ICollection;
            var parameters = property.GetIndexParameters();
            var len = converted.Count;
            //var len = ReflectionHelper.GetPropertyValue<int>(obj, "Count");
            
            for (int i = 0; i < len; i++)
            {
                var value = property.GetValue(obj, new object[] { i });
                var parameter = new MethodValueParam(parameters[0].Name, parameters[0], i);
                yield return new MethodValue(value, parameter);
            }
        }
    }
}