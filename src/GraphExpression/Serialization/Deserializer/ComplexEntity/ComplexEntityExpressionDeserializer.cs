using System;
using System.Threading.Tasks;

namespace GraphExpression.Serialization
{
    public class ComplexEntityExpressionDeserializer
    {    
        public T Deserialize<T>(string expression)
        {
            return DeserializeAsync<T>(expression).Result;
        }

        public async Task<T> DeserializeAsync<T>(string expression)
        {
            return await DeserializeAsync<T>(expression, new ComplexEntityFactory(typeof(T)));
        }

        public object Deserialize(string expression, Type type = null)
        {
            return DeserializeAsync(expression, type).Result;
        }

        public async Task<object> DeserializeAsync(string expression, Type type = null)
        {
            return await DeserializeAsync(expression, new ComplexEntityFactory(type));
        }

        public T Deserialize<T>(string expression, ComplexEntityFactory factory)
        {
            return DeserializeAsync<T>(expression, factory).Result;
        }

        public async Task<T> DeserializeAsync<T>(string expression, ComplexEntityFactory factory)
        {
            var root = await DeserializeAsync(expression, factory);
            return (T)root;
        }

        public async Task<object> DeserializeAsync(string expression, ComplexEntityFactory factory)
        {
            var roslyn = new RoslynExpressionDeserializer<Entity>();
            var runner = roslyn.GetDelegateExpression(expression, factory.GetType());
            var root = await runner(factory);

            factory.Root = root;
            return factory.Build().Value;
        }
    }
}