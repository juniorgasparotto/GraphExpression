using System;
using System.Collections.Generic;

namespace GraphExpression
{
    public class CircularEntityFactory<T> : IDeserializeFactory<T>
    {
        public IDictionary<string, T> Entities { get; set; }
        protected Func<string, T> createEntityCallback;

        public CircularEntityFactory(Func<string, T> createEntityCallback = null, Dictionary<string, T> entities = null)
        {
            this.createEntityCallback = createEntityCallback;
            this.Entities = entities ?? new Dictionary<string, T>();
        }

        #region IDeserializeFactory

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
