using System.Collections.Generic;
using System.Diagnostics;

namespace GraphExpression
{
    [DebuggerDisplay("{Name}")]
    public class CircularEntity
    {
        private readonly List<CircularEntity> children;

        public string Name { get; private set; }
        public int Count => children.Count;
        public CircularEntity this[int index] => children[index];

        public CircularEntity(string name)
        {
            this.children = new List<CircularEntity>();
            this.Name = name;
        }

        public IEnumerable<CircularEntity> Children { get => children; }

        public static CircularEntity operator +(CircularEntity a, CircularEntity b)
        {
            a.children.Add(b);
            return a;
        }

        public static CircularEntity operator -(CircularEntity a, CircularEntity b)
        {
            a.children.Remove(b);
            return a;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
