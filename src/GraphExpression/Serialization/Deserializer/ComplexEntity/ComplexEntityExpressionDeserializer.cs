using System;
using System.Threading.Tasks;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Create a complex expression deserializer
    /// </summary>
    public class ComplexEntityExpressionDeserializer
    {
        /// <summary>
        /// Deserializes a string expression for the inferred type.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="expression">Expressin as string</param>
        /// <returns>Return a typed complety entity</returns>
        public T Deserialize<T>(string expression)
        {
            return DeserializeAsync<T>(expression).Result;
        }

        /// <summary>
        /// Deserializes a string expression for the inferred type.
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="expression">Expressin as string</param>
        /// <returns>Return a typed complety entity</returns>
        public async Task<T> DeserializeAsync<T>(string expression)
        {
            return await DeserializeAsync<T>(expression, new ComplexEntityFactory(typeof(T)));
        }

        /// <summary>
        /// Deserializes a string expression for the Type parameter
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <param name="type">Type of real entity</param>
        /// <returns>Return a typed complety entity</returns>
        public object Deserialize(string expression, Type type = null)
        {
            return DeserializeAsync(expression, type).Result;
        }

        /// <summary>
        /// Deserializes a string expression for the Type parameter.
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <param name="type">Type of real entity</param>
        /// <returns>Return a object complety entity</returns>
        public async Task<object> DeserializeAsync(string expression, Type type = null)
        {
            return await DeserializeAsync(expression, new ComplexEntityFactory(type));
        }

        /// <summary>
        /// Deserializes a string expression with custom complex entity factory
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="expression">Expressin as string</param>
        /// <param name="factory">This factory is util to customize entity creation</param>
        /// <returns>Return a typed complety entity</returns>
        public T Deserialize<T>(string expression, ComplexEntityFactory factory)
        {
            return DeserializeAsync<T>(expression, factory).Result;
        }

        /// <summary>
        /// Deserializes a string expression with custom complex entity factory
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="expression">Expressin as string</param>
        /// <param name="factory">This factory is util to customize entity creation</param>
        /// <returns>Return a typed complety entity</returns>
        public async Task<T> DeserializeAsync<T>(string expression, ComplexEntityFactory factory)
        {
            var root = await DeserializeAsync(expression, factory);
            return (T)root;
        }

        /// <summary>
        /// Deserializes a string expression with custom complex entity factory
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <param name="factory">This factory is util to customize entity creation</param>
        /// <returns>Return a complety entity</returns>
        public object Deserialize(string expression, ComplexEntityFactory factory)
        {
            return DeserializeAsync(expression, factory).Result;
        }

        /// <summary>
        /// Deserializes a string expression with custom complex entity factory
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <param name="factory">This factory is util to customize entity creation</param>
        /// <returns>Return a complety entity</returns>
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