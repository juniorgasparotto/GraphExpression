using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class PropertyReaderIndexerInt32InAnyClass : IPropertyReader
    {
        public bool CanRead(ReflectInstance value, Type type, PropertyInfo property)
        {
            // verify if property is "this[int i]", in real use is: "myList[0]', exclude array objects 
            // because if array is multidimensional will happen a error when call 
            // "property.GetValue(obj, new object[] { 0, 0 });" the class "PropertyReaderIndexerInArray" fix it.
            var parameters = property.GetIndexParameters();
            //
            return !(value.Object is Array) && (parameters.Length == 1)
                && (parameters[0].ParameterType == typeof(int));
        }

        public IEnumerable<MethodValue> GetValues(ReflectInstance value, Type type, PropertyInfo property)
        {
            //var converted = obj as System.Collections.ICollection;
            //var len = converted.Count;

            var parameters = property.GetIndexParameters();
            var len = 0;
            if (!ReflectionUtils.TryGetPropertyValue<int>(value.Object, "Count", out len))
                if (!ReflectionUtils.TryGetPropertyValue<int>(value.Object, "Length", out len))
                    throw new Exception("It could not find the size of the indexed property '" + property.Name + "' to iterate.");

            for (int i = 0; i < len; i++)
            {
                var propValue = property.GetValue(value.Object, new object[] { i });
                var parameter = new MethodValueParam(parameters[0].Name, parameters[0], i);
                yield return new MethodValue(propValue, parameter);
            }
        }
    }
}