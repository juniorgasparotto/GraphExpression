using GraphExpression.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphExpression.Serialization
{
    public class CircularEntityExpressionDeserializer<T>
    {
        public T Deserialize(string expression)
        {
            return DeserializeAsync(expression).Result;
        }

        public async Task<T> DeserializeAsync(string expression)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));

            var factory = new CircularEntityFactory<T>(null, new Dictionary<string, T>());
            return await DeserializeAsync(expression, factory);
        }

        public T Deserialize(string expression, Func<string, T> createEntityCallback)
        {
            return DeserializeAsync(expression, createEntityCallback).Result;
        }

        public async Task<T> DeserializeAsync(string expression, Func<string, T> createEntityCallback)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));
            Validation.ArgumentNotNull(createEntityCallback, nameof(createEntityCallback));

            var factory = new CircularEntityFactory<T>(createEntityCallback, new Dictionary<string, T>());
            return await DeserializeAsync(expression, factory);
        }

        public T Deserialize(string expression, CircularEntityFactory<T> factory)
        {
            return DeserializeAsync(expression, factory).Result;
        }

        public async Task<T> DeserializeAsync(string expression, CircularEntityFactory<T> factory)
        {
            var roslyn = new RoslynExpressionDeserializer<T>();
            var runner = roslyn.GetDelegate(expression, factory.GetType());
            return await runner(factory);
        }
    }
}