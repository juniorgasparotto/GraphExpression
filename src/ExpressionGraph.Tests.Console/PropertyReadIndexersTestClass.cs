using ExpressionGraph.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Tests.Console
{
    public class PropertyReadIndexersTestClass : IPropertyRead
    {
        public bool CanRead(object obj, PropertyInfo property)
        {
            return obj is TestClass && property.GetIndexParameters().Length > 0;
        }

        public IEnumerable<MethodValue> GetValues(object obj, PropertyInfo property)
        {
            var converted = obj as TestClass;
            var len = converted.List.Count;
            var parameters = property.GetIndexParameters();

            // this[int index]
            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
            {
                for (int i = 0; i < len; i++)
                {
                    var value = property.GetValue(obj, new object[] { i });
                    var parameter = new MethodValueParam(parameters[0].Name, parameters[0], i);
                    yield return new MethodValue(value, parameter);
                }
            }

            // this[int index, int index2]
            if (parameters.Length == 2 && parameters[0].ParameterType == typeof(int) && parameters[1].ParameterType == typeof(int))
            {
                for (int i = 0; i < len; i++)
                {
                    var value = property.GetValue(obj, new object[] { i, 0 });
                    var parameter1 = new MethodValueParam(parameters[0].Name, parameters[0], i);
                    var parameter2 = new MethodValueParam(parameters[1].Name, parameters[1], 0);
                    yield return new MethodValue(value, parameter1, parameter2);
                }
            }

            // this[string contains]
            if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
            {
                for (int i = 0; i < len; i++)
                {
                    var value = property.GetValue(obj, new object[] { "list " + i });
                    var parameter1 = new MethodValueParam(parameters[0].Name, parameters[0], "list " + i);
                    yield return new MethodValue(value, parameter1);
                }
            }
        }
    }
}