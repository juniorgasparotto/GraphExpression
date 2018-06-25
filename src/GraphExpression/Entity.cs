using System.Collections.Generic;
using System.Diagnostics;

namespace GraphExpression
{
    [DebuggerDisplay("{ToString()}")]
    public class Entity : List<Entity>
    {
        public string Name { get; private set; }
        public Entity(string name) => this.Name = name;

        // only didatic
        public IEnumerable<Entity> Children { get => this; }

        public static Entity operator +(Entity a, Entity b)
        {
            a.Add(b);
            return a;
        }

        public static Entity operator -(Entity a, Entity b)
        {
            a.Remove(b);
            return a;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
