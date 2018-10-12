using System.Collections.Generic;
using System.Linq;

namespace GraphExpression.Utils
{
    internal static class DictionaryUtils
    {
        public static Dictionary<TKey, TValue> InvertDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            var reverse = new Dictionary<TKey, TValue>();

            for (var i = dictionary.Count - 1; i >= 0; i--)
                reverse.Add(dictionary.Keys.ElementAt(i), dictionary.Values.ElementAt(i));

            return reverse;
        }
    }
}
