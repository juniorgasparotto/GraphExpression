using System.Collections.Generic;
using System.Diagnostics;

namespace GraphExpression
{
    public class CircularEntity : List<CircularEntity>
    {
        public string Name { get; private set; }
        public CircularEntity(string name) => this.Name = name;

        // only didatic
        public IEnumerable<CircularEntity> Children { get => this; }

        public static CircularEntity operator +(CircularEntity a, CircularEntity b)
        {
            a.Add(b);
            return a;
        }

        public static CircularEntity operator -(CircularEntity a, CircularEntity b)
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
