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
            return await DeserializeAsync<T>(expression, new ComplexEntityFactoryDeserializer(typeof(T)));
        }

        public object Deserialize(string expression, Type type = null)
        {
            return DeserializeAsync(expression, type).Result;
        }

        public async Task<object> DeserializeAsync(string expression, Type type = null)
        {
            return await DeserializeAsync(expression, new ComplexEntityFactoryDeserializer(type));
        }

        public T Deserialize<T>(string expression, ComplexEntityFactoryDeserializer factory)
        {
            return DeserializeAsync<T>(expression, factory).Result;
        }

        public async Task<T> DeserializeAsync<T>(string expression, ComplexEntityFactoryDeserializer factory)
        {
            var root = await DeserializeAsync(expression, factory);
            return (T)root;
        }

        public async Task<object> DeserializeAsync(string expression, ComplexEntityFactoryDeserializer factory)
        {
            var deserializer = new ExpressionDeserializer<ComplexEntityDeserializer>();

            var runner = deserializer.GetDelegate(expression, factory.GetType());
            factory.DeserializationTime = DeserializationTime.Creation;
            await runner(factory);

            factory.DeserializationTime = DeserializationTime.AssignChildInParent;
            var root = await runner(factory);
            return root?.Entity;
        }
    }
}