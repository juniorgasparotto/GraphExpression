using GraphExpression.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Create a circular expression deserializer
    /// </summary>
    /// <typeparam name="T">Type of circular entity. This type should override the "+"</typeparam>
    public class CircularEntityExpressionDeserializer<T>
    {
        /// <summary>
        /// Deserializes a string expression for the inferred type. This type should contain a "string name" parameter that will come from the expression: "A" + "B"
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <returns>Return a circular entity</returns>
        public T Deserialize(string expression)
        {
            return DeserializeAsync(expression).Result;
        }

        /// <summary>
        /// Deserializes a string expression for the inferred type. This type should contain a "string name" parameter that will come from the expression: "A" + "B"
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <returns>Return a circular entity</returns>
        public async Task<T> DeserializeAsync(string expression)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));

            var factory = new CircularEntityFactory<T>(null, new Dictionary<string, T>());
            return await DeserializeAsync(expression, factory);
        }

        /// <summary>
        /// Deserializes a string expression for the inferred type. This type should contain a "string name" parameter that will come from the expression: "A" + "B"
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <param name="createEntityCallback">Delegates the creation of the entity to the caller. The caller will receive the item name in the expression: "A" + "B"; A and B are the names of the respective entities.</param>
        /// <returns>Return a circular entity</returns>
        public T Deserialize(string expression, Func<string, T> createEntityCallback)
        {
            return DeserializeAsync(expression, createEntityCallback).Result;
        }

        /// <summary>
        /// Deserializes a string expression for the inferred type. This type should contain a "string name" parameter that will come from the expression: "A" + "B"
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <param name="createEntityCallback">Delegates the creation of the entity to the caller. The caller will receive the item name in the expression: "A" + "B"; A and B are the names of the respective entities.</param>
        /// <returns>Return a circular entity</returns>
        public async Task<T> DeserializeAsync(string expression, Func<string, T> createEntityCallback)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));
            Validation.ArgumentNotNull(createEntityCallback, nameof(createEntityCallback));

            var factory = new CircularEntityFactory<T>(createEntityCallback, new Dictionary<string, T>());
            return await DeserializeAsync(expression, factory);
        }

        /// <summary>
        /// Deserializes a string expression for the inferred type. This type should contain a "string name" parameter that will come from the expression: "A" + "B"
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <param name="factory">This factory is necessary for the creation of entities. You can inherit this class and add new methods to be used in the string expression: "A" + NewMethod()</param>
        /// <returns>Return a circular entity</returns>
        public T Deserialize(string expression, CircularEntityFactory<T> factory)
        {
            return DeserializeAsync(expression, factory).Result;
        }

        /// <summary>
        /// Deserializes a string expression for the inferred type. This type should contain a "string name" parameter that will come from the expression: "A" + "B"
        /// </summary>
        /// <param name="expression">Expressin as string</param>
        /// <param name="factory">This factory is necessary for the creation of entities. You can inherit this class and add new methods to be used in the string expression: "A" + NewMethod()</param>
        /// <returns>Return a circular entity</returns>
        public async Task<T> DeserializeAsync(string expression, CircularEntityFactory<T> factory)
        {
            var roslyn = new RoslynExpressionDeserializer<T>();
            var runner = roslyn.GetDelegateExpression(expression, factory.GetType());
            return await runner(factory);
        }
    }
}