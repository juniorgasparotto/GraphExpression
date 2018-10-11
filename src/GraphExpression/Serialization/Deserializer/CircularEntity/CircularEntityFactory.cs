using System;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// This factory is necessary for the creation of entities. You can inherit this class and add new methods to be used in the string expression: "A" + NewMethod()
    /// </summary>
    /// <typeparam name="T">Type of circular entity. This type should override the "+"</typeparam>
    public class CircularEntityFactory<T> : IDeserializeFactory<T>
    {
        protected Func<string, T> createEntityCallback;

        /// <summary>
        /// All entities loaded
        /// </summary>
        public IDictionary<string, T> Entities { get; set; }

        /// <summary>
        /// Create a circular factory
        /// </summary>
        /// <param name="createEntityCallback">Delegates the creation of the entity to the caller. The caller will receive the item name in the expression: "A" + "B"; A and B are the names of the respective entities.</param>
        /// <param name="entities">It can be useful when there are already entities loaded in memory and the string expression will use these entities if the names are the same.</param>
        public CircularEntityFactory(Func<string, T> createEntityCallback = null, Dictionary<string, T> entities = null)
        {
            this.createEntityCallback = createEntityCallback;
            this.Entities = entities ?? new Dictionary<string, T>();
        }

        #region IDeserializeFactory - used only in deserialize flow

        /// <summary>
        /// This method is exclusive for deserialization moment and should create a new entity to compose a expression
        /// new Entity("A") + new Entity("B");
        /// </summary>
        /// <param name="name">Name that is used as Raw</param>
        /// <param name="index">Index at expression</param>
        /// <returns>Return a new Entity to compose the expression</returns>
        public T GetEntity(string name, int index)
        {
            if (!Entities.TryGetValue(name, out T val))
            {
                if (createEntityCallback == null)
                    val = (T)Activator.CreateInstance(typeof(T), name);
                else
                    val = createEntityCallback(name);
                Entities.Add(name, val);
            }

            return val;
        }

        #endregion
    }
}
