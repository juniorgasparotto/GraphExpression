using System;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public class FunctionsDeserializer<T>
    {
        private readonly Func<string, T> createEntityCallback;
        public IDictionary<string, T> Entities { get; set; }

        public FunctionsDeserializer(Func<string, T> createEntityCallback = null, Dictionary<string, T> entities = null)
        {
            this.createEntityCallback = createEntityCallback;
            this.Entities = entities ?? new Dictionary<string, T>();
        }

        public virtual T GetEntity(string name)
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
    }
}
