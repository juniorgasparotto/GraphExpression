﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class PropertyReaderIndexerInArray : IPropertyReader
    {
        public bool CanRead(object obj, PropertyInfo property)
        {
            // verify if property is "this[int i]", in real use is: "array[0]" or multi "array[0,0,0,0]"
            var parameters = property.GetIndexParameters();
            return (obj is Array) && (parameters.Length == 1)
                && (parameters[0].ParameterType == typeof(int));
        }

        public IEnumerable<MethodValue> GetValues(object obj, PropertyInfo property)
        {
            var array = obj as Array;
            var keysValues = ReflectionHelper.ArrayToDictionary(array);
            var parameters = property.GetIndexParameters();

            foreach(var keyValue in keysValues)
            {
                var parameter = new MethodValueParam("indices", parameters[0], keyValue.Key);
                yield return new MethodValue(keyValue.Value, parameter);
            }
        }
    }
}