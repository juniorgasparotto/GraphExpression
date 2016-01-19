using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class EnumerableReaderIDictionary : IEnumerableReader
    {
        public bool CanRead(object value, Type type)
        {
            return value is IDictionary;
        }

        public IEnumerable<MethodValue> GetValues(object value, Type type)
        {
            var dictionary = value as IDictionary;

            foreach (var key in dictionary.Keys)
            {
                var parameter = new MethodValueParam("key", null, key);
                yield return new MethodValue(dictionary[key], parameter);
            }
        }
    }
}